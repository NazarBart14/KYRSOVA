using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KYRSOVA
{

    public partial class SelectedGamesWindow  : Window
    {
        public SelectedGamesWindow()
        {
            InitializeComponent();
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

       
    }
}
