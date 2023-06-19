using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HtmlAgilityPack;
using System.Data;
using System.Data.SqlClient;

namespace KYRSOVA
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _gamesParsed; // Флаг, що показує, чи були ігри спарсені
        private List<Game> _games; // Список ігор
        private List<Game> _selectedGames; // Список вибраних ігор
        private List<string> _genres; // Список жанрів
        private string _selectedGenre; // Обраний жанр

        public List<Game> Games // Список ігор
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public List<string> Genres // Список жанрів
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public string SelectedGenre // Обраний жанр
        {
            get { return _selectedGenre; }
            set
            {
                _selectedGenre = value;
                OnPropertyChanged();
                RefreshGames();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _selectedGames = new List<Game>();
            _gamesParsed = false;
            ParseButton.IsEnabled = !_gamesParsed;
        }

        private async void ParseButton_Click(object sender, RoutedEventArgs e) // Парсинг ігор
        {
            await ParseGamesAsync();
            ParseButton.IsEnabled = false;
            _gamesParsed = true;
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Вибір ігор
        {
            var selectedItems = GamesListBox.SelectedItems;
            _selectedGames.Clear();

            foreach (var item in selectedItems) // Додавання вибраних ігор в список вибраних ігор
            {
                if (item is Game game)
                {
                    _selectedGames.Add(game);
                }
            }
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e) // Переміщення вибраних ігор в список вибраних ігор
        {
            foreach (var game in _selectedGames)
            {
                if (!SelectedGamesListBox.Items.Contains(game))
                {
                    Games.Remove(game);
                    SelectedGamesListBox.Items.Add(game);
                }
            }

            SaveGamesToDatabase();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що бажаєте очистити свою бібліотеку?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SelectedGamesListBox.Items.Clear();
                SaveGamesToDatabase();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGamesListBox.Items.Remove(SelectedGamesListBox.SelectedItem);
            SaveGamesToDatabase();
        }

        private async Task ParseGamesAsync() // Парсинг ігор
        {
            var games = new List<Game>();
            var genres = new List<string>();
            var url = "https://store.steampowered.com/search/?sort_by=_ASC&category1=998&page=1";

            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='responsive_search_name_combined']");

            foreach (var node in nodes) // Парсинг ігор
            {
                var nameNode = node.SelectSingleNode(".//span[@class='title']");
                var priceNode = node.SelectSingleNode(".//div[@class='col search_price_discount_combined responsive_secondrow']");
                var genreNode = node.SelectSingleNode(".//span[@class='search_tag']/a");
                var iconNode = node.SelectSingleNode(".//div[@class='col search_capsule']/img");

                if (nameNode != null && priceNode != null)
                {
                    var game = new Game
                    {
                        Name = nameNode.InnerText.Trim(),
                        Price = priceNode.InnerText.Trim(),
                        Icon = iconNode?.GetAttributeValue("src", null) // Assign the icon URL if available
                    };

                    if (genreNode != null)
                    {
                        var genre = genreNode.InnerText.Trim();

                        if (!genres.Contains(genre))
                        {
                            genres.Add(genre);
                        }

                        game.Genre = genre;
                    }

                    games.Add(game);
                }
            }

            Games = games;
            Genres = genres.OrderBy(g => g).ToList();
            SelectedGenre = string.Empty;
        }

        private void RefreshGames() // Оновлення списку ігор
        {
            if (string.IsNullOrEmpty(SelectedGenre)) // Перевірка на обраний жанр
            {
                Games = _games; // Оновлення списку ігор на основі вибраного жанру
            }
            else
            {
                Games = _games.Where(g => g.Genre == SelectedGenre).ToList(); // Фільтрування ігор за обраним жанром
            }
        }

        private void SaveGamesToDatabase() // Збереження ігор в базу даних
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Librery;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False"; // Рядок підключення до бази даних
            string tableName = "GameLibrery"; // Назва таблиці

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Очищення таблиці
                string clearTableQuery = $"DELETE FROM {tableName}";
                SqlCommand clearTableCommand = new SqlCommand(clearTableQuery, connection);
                clearTableCommand.ExecuteNonQuery();

                // Вставка нових даних
                foreach (var game in SelectedGamesListBox.Items)
                {
                    if (game is Game selectedGame)
                    {
                        string insertQuery = $"INSERT INTO {tableName} (Name, Price, Genre) " +
                            $"VALUES ('{selectedGame.Name}', '{selectedGame.Price}', '{selectedGame.Genre}')";

                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged; // Подія зміни властивостей

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) // Оновлення властивостей
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Game // Клас ігор
    {
        public string Name { get; set; } // Назва гри
        public string Price { get; set; } // Ціна гри
        public string Genre { get; set; } // Жанр гри
        public string Icon { get; set; } // Іконка гри
    }
}
