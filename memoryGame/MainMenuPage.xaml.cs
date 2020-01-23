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

namespace memoryGame
{
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        //Przeniesienie się do sekcji z rozgrywką
        private void PlayButton(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("GamePage.xaml", UriKind.Relative));
        }

        //Zamknięcie Aplikacji
        private void QuitButton(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
