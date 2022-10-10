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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KeyboardTrainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand = new Random();
        private List<String> symbols = new List<String>() {"`", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=",
            "q", "w", "e", "r", "t", "y", "u", "i", "o", "p", "[", "]", "\\",
            "a", "s", "d", "f", "g", "h", "j", "k", "l", ";", "'",
            "z", "x", "c", "v", "b", "n", "m", ",", ".", "/" };

        private System.Windows.Threading.DispatcherTimer timer;
        private DateTime startTime;

        private bool isStarted;
        private String gameStr;
        private int failsCounter;
        private int strSymbolsCount;
        private int symCount;
        private double speed;

        private Label? prevKeyPressed = null;
        public MainWindow()
        {
            InitializeComponent();
            gameStr = "";
            failsCounter = 0;
            isStarted = false;
            strSymbolsCount = 40;
            speed = 0.0;
            symCount = 0;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (isStarted)
            {
                var keyboardKey = this.FindName(e.Key.ToString().ToLower()) as Label;
                if (keyboardKey != null)
                {
                    if (prevKeyPressed != null)
                    {
                        prevKeyPressed.BorderThickness = new Thickness(1.5);
                        prevKeyPressed.BorderBrush = Brushes.Black;
                    }
                    if (keyboardKey.Content.ToString() == gameStr[0].ToString())
                    {

                        keyboardKey.BorderThickness = new Thickness(3.5);
                        keyboardKey.BorderBrush = Brushes.Green;
                        prevKeyPressed = keyboardKey;

                        gameStr = gameStr.Substring(1);
                        strLabel.Content = gameStr;
                        symCount++;
                        if (String.IsNullOrEmpty(gameStr))
                        {
                            Win();
                        }
                    }
                    else
                    {
                        keyboardKey.BorderThickness = new Thickness(3.5);
                        keyboardKey.BorderBrush = Brushes.Red;
                        prevKeyPressed = keyboardKey;
                        failsCounter++;
                        failsLabel.Content = $"Fails: {failsCounter}";
                    }
                }
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            gameStr = GenerateStr();
            strLabel.Content = gameStr;
            isStarted = true;

            startTime = DateTime.Now;
            timer = new();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
            startButton.IsEnabled = false;
            stopButton.IsEnabled = true;
            if (prevKeyPressed != null)
            {
                prevKeyPressed.BorderThickness = new Thickness(1.5);
                prevKeyPressed.BorderBrush = Brushes.Black;
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            startButton.IsEnabled = true;
            stopButton.IsEnabled = false;
            isStarted = false;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            speed = (symCount / (DateTime.Now - startTime).TotalSeconds) * 60;
            speedLabel.Content = $"Speed: {Math.Round(speed, 2)} char/min";
        }

        private String GenerateStr()
        {
            String str = "";
            for (int i = 0; i < strSymbolsCount; i++)
            {
                str += symbols[rand.Next(symbols.Count - 1)];
            }
            return str;
        }

        private void Win()
        {
            timer.Stop();
            MessageBox.Show($"Time Elapsed: {(DateTime.Now - startTime)}\nFails: {failsCounter}");
        }

        private void Reset()
        {
            gameStr = "";
            failsCounter = 0;
            isStarted = false;
            speed = 0.0;

            strLabel.Content = gameStr;
            failsLabel.Content = $"Fails: {failsCounter}";
            speedLabel.Content = $"Speed: {speed} char/min";
        }

  
    }
}
