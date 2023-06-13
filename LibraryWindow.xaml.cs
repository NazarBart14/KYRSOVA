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
    /// <summary>
    /// Логика взаимодействия для LibraryWindow.xaml
    /// </summary>
    public partial class LibraryWindow : Window
    {
        public LibraryWindow()
        {
            InitializeComponent();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedGamesListBox.Items.Remove(SelectedGamesListBox.SelectedItem);
        }
    }
}
