using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HtmlAgilityPack;

namespace KYRSOVA
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool _gamesParsed;
        private List<Game> _games;
        private List<Game> _selectedGames;
        private List<string> _genres;
        private string _selectedGenre;

        public List<Game> Games
        {
            get => _games;
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public List<string> Genres
        {
            get => _genres;
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _selectedGames = new List<Game>();
            _gamesParsed = false;
            ParseGamesAsync();
        }

        private async Task ParseGamesAsync()
        {
            var games = new List<Game>();
            var urlBase = "https://store.steampowered.com/search/?sort_by=_ASC&supportedlang=ukrainian&category1=1&page=";

            for (int page = 1; page <= 2; page++)
            {
                var url = urlBase + page;
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(url);
                var nodes = doc.DocumentNode.SelectNodes("//div[@id='search_resultsRows']/a");

                foreach (var node in nodes)
                {
                    var nameNode = node.SelectSingleNode(".//span[@class='title']");
                    var priceNode = node.SelectSingleNode(".//div[@class='col search_price_discount_combined responsive_secondrow']");
                    var imageNode = node.SelectSingleNode(".//img");

                    var game = new Game
                    {
                        Name = nameNode?.InnerText?.Trim(),
                        Price = priceNode?.InnerText?.Trim(),
                        ImageUrl = imageNode?.GetAttributeValue("src", "") ?? "Empty"
                    };

                    games.Add(game);
                }
            }
            Games = games;
        }



        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedGames.Clear();
            foreach (var item in GamesListView.SelectedItems)
            {
                if (item is Game game)
                {
                    _selectedGames.Add(game);
                }
            }
        }

        private void PlaySelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGames = SelectedGamesListView.SelectedItems.Cast<Game>().ToList();
            if (selectedGames.Any())
            {
                var gamesNames = string.Join(", ", selectedGames.Select(game => game.Name));
                MessageBox.Show($"Starting selected games: {gamesNames}", "Games Launch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Please select games to play.", "Games Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in _selectedGames)
            {
                if (!SelectedGamesListView.Items.Contains(game))
                {
                    Games.Remove(game);
                    SelectedGamesListView.Items.Add(game);
                }
            }
        }

        private void SelectedGamesListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedGame = SelectedGamesListView.SelectedItem as Game;
            if (selectedGame != null)
            {
                MessageBox.Show($"Name: {selectedGame.Name}\nPrice: {selectedGame.Price}");
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to clear your library?", "Delete confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SelectedGamesListView.Items.Clear();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGamesListView.Items.Remove(SelectedGamesListView.SelectedItem);
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchText = SearchGameTextBox.Text.ToLower();
            var matchingGames = Games.Where(game => game.Name.ToLower().Contains(searchText)).ToList();
            GamesListView.ItemsSource = matchingGames;
        }

        private void TimesButton_Click(object sender, RoutedEventArgs e)
        {
            TimerWindows timerWindows = new TimerWindows();
            timerWindows.Show();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Game
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
