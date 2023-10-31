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
        public bool _gamesParsed;
        public List<Game> _games;
        public List<Game> _selectedGames;
        public List<string> _genres;
        public GameViewModel _gameViewModel;
        private GameParser _gameParser;
        public GameEntity _gameEntity;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            GameViewModel = new GameViewModel();
            _games = new List<Game>();
            _selectedGames = new List<Game>();
            _genres = new List<string>();
            _gamesParsed = false;
        }

        public List<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged("Games");
            }
        }

        public List<string> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged("Genres");
            }
        }


        public GameViewModel GameViewModel
        {
            get { return _gameViewModel; }
            set
            {
                _gameViewModel = value;
                OnPropertyChanged("GameViewModel");
            }
        }

        



        public bool GamesParsed
        {
            get { return _gamesParsed; }
            set
            {
                _gamesParsed = value;
                OnPropertyChanged(nameof(GamesParsed));
            }
        }




        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_selectedGames == null)
            {
                _selectedGames = new List<Game>();
            }
            else
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
        }


        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in _selectedGames)
            {
                //if (!SelectedGames.Contains(game))
                //{
                //    Games.Remove(game);
                //    _gameEntity.SelectedGames.Add(game);
                //}
            }
            //  OnPropertyChanged(nameof(Games));
            // OnPropertyChanged(nameof(SelectedGames));
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
        

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            //// Перехід на наступну сторінку, якщо існують ще елементи
            //int totalItems = Games.Count;
            //int totalPages = (int)Math.Ceiling((double)totalItems / _itemsPerPage);
            //if (_currentPage < totalPages)
            //{
            //    _currentPage++;
            //    UpdateGamesListView();
            //}
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            //// Перехід на попередню сторінку, якщо не перший рядок
            //if (_currentPage > 1)
            //{
            //    _currentPage--;
            //    UpdateGamesListView();
            //}
        }

        public event PropertyChangedEventHandler PropertyChanged; // Подія, яка виникає при зміні властивості

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }


}
