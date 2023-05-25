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
using System.Windows.Shapes;
using HtmlAgilityPack;

namespace KYRSOVA
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Game> _games;
        private List<Game> _selectedGames;
        private List<string> _genres;
        private string _selectedGenre;

        public List<Game> Games
        {
            get { return _games; }
            set
            {
                _games = value;
                OnPropertyChanged();
            }
        }

        public List<string> Genres
        {
            get { return _genres; }
            set
            {
                _genres = value;
                OnPropertyChanged();
            }
        }

        public string SelectedGenre
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
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            ParseGames();
        }

        private void GamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = GamesListBox.SelectedItems;
            _selectedGames.Clear();

            foreach (var item in selectedItems)
            {
                if (item is Game game)
                {
                    _selectedGames.Add(game);
                }
            }
        }

        private void MoveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var game in _selectedGames)
            {
                Games.Remove(game);
                SelectedGamesListBox.Items.Add(game);
            }

            _selectedGames.Clear();
        }

        private void ParseGames()
        {
            var games = new List<Game>();
            var genres = new List<string>();
            var url = "https://store.steampowered.com/search/?sort_by=_ASC&category1=998&page=1";

            var web = new HtmlWeb();
            var doc = web.Load(url);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='responsive_search_name_combined']");

            foreach (var node in nodes)
            {
                var nameNode = node.SelectSingleNode(".//span[@class='title']");
                var priceNode = node.SelectSingleNode(".//div[@class='col search_price_discount_combined responsive_secondrow']");
                var genreNode = node.SelectSingleNode(".//span[@class='search_tag']/a");

                if (nameNode != null && priceNode != null)
                {
                    var game = new Game
                    {
                        Name = nameNode.InnerText.Trim(),
                        Price = priceNode.InnerText.Trim()
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

        private void RefreshGames()
        {
            if (string.IsNullOrEmpty(SelectedGenre))
            {
                Games = _games;
            }
            else
            {
                Games = _games.Where(g => g.Genre == SelectedGenre).ToList();
            }
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
        public string Genre { get; set; }
    }
}
