﻿using System;
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



        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            _selectedGames = new List<Game>();
            _gamesParsed = false;
            ParseGamesAsync(); // Викликаємо метод парсингу автоматично

        }




        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) // Вибір ігор
        {
            var selectedItems = GamesListView.SelectedItems;
            _selectedGames.Clear();

            foreach (var item in selectedItems) // Додавання вибраних ігор в список вибраних ігор
            {
                if (item is Game game)
                {
                    _selectedGames.Add(game);
                }
            }
        }

        private void PlaySelectedButton_Click(object sender, RoutedEventArgs e)
        {
            // Отримуємо обрані елементи зі списку вибраних ігор
            var selectedGames = SelectedGamesListView.SelectedItems.Cast<Game>().ToList();

            if (selectedGames.Any())
            {
                // Виконуємо дії для запуску обраних ігор зі збереженими властивостями ігор.
                // В даному випадку просто відобразимо повідомлення з назвами ігор, які обрали.
                var gamesNames = string.Join(", ", selectedGames.Select(game => game.Name));
                MessageBox.Show($"Starting selected games: {gamesNames}", "Games Launch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                // Ви можете виконати інші дії у випадку, якщо не обрано жодної гри
                MessageBox.Show("Please select games to play.", "Games Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void MoveButton_Click(object sender, RoutedEventArgs e) // Переміщення вибраних ігор в список вибраних ігор
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

        private async Task ParseGamesAsync()
        {
            var games = new List<Game>();
            var urlBase = "https://store.steampowered.com/search/?sort_by=_ASC&category1=998&page=";

            for (int page = 1; page <= 5; page++) // Парсити перші 
            {
                var url = urlBase + page;
                var web = new HtmlWeb();
                var doc = await web.LoadFromWebAsync(url);
                var nodes = doc.DocumentNode.SelectNodes("//div[@class='responsive_search_name_combined']");

                foreach (var node in nodes) // Парсинг ігор
                {
                    var nameNode = node.SelectSingleNode(".//span[@class='title']");
                    var priceNode = node.SelectSingleNode(".//div[@class='col search_price_discount_combined responsive_secondrow']");

                    if (nameNode != null && priceNode != null)
                    {
                        var game = new Game
                        {
                            Name = nameNode.InnerText.Trim(),
                            Price = priceNode.InnerText.Trim(),
                        };

                        games.Add(game);
                    }
                }
            }

            Games = games;
        }



        public event PropertyChangedEventHandler PropertyChanged; // Подія зміни властивостей

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) // Оновлення властивостей
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void TimesButton_Click(object sender, RoutedEventArgs e)
        {
            TimerWindows timerWindows = new TimerWindows();
            timerWindows.Show();
        }


    }


    public class Game // Клас ігор
    {
        public string Name { get; set; } // Назва гри
        public string Price { get; set; } // Ціна гри


    }

}