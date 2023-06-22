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
using System.Windows.Threading;

namespace KYRSOVA
{
    /// <summary>
    /// Логика взаимодействия для TimerWindows.xaml
    /// </summary>
    public partial class TimerWindows : Window
    {
        private DispatcherTimer timer;
        private TimeSpan timeElapsed;

        public TimerWindows()
        {
            InitializeComponent();

            // Ініціалізуємо таймер
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            // Початкове значення часу
            timeElapsed = TimeSpan.Zero;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Збільшуємо пройдений час на 1 секунду
            timeElapsed = timeElapsed.Add(TimeSpan.FromSeconds(1));

            // Оновлюємо значення таймера на екрані
            timerLabel.Content = timeElapsed.ToString(@"hh\:mm\:ss");
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            // Починаємо таймер
            timer.Start();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            // Зупиняємо таймер
            timer.Stop();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви точно хочете очистити таймер?", "Підтвердження", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                // Скидаємо таймер до початкового значення
                timeElapsed = TimeSpan.Zero;
                timerLabel.Content = timeElapsed.ToString(@"hh\:mm\:ss");
            }
        }
    }
}