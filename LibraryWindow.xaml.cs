using System.Collections.Generic;
using System.Windows;

namespace KYRSOVA
{
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
        }

        public void UpdateGames(List<Game> games)
        {
            SelectedGamesListBox.ItemsSource = games;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що бажаєте очистити свою бібліотеку?", "Підтвердження видалення", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                SelectedGamesListBox.Items.Clear();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGamesListBox.Items.Remove(SelectedGamesListBox.SelectedItem);
        }


    }
}
