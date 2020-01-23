using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace memoryGame
{
    //Metoda rozszerzająca która poprawia optymalizacje interfejsu graficznego wykorzystując zdarzenia i delegaty
    public static class ExtensionMethods
    {
        private static Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }

    //Algorytm pod nazwą Fisher mieszający karty 
    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

    public partial class GamePage : Page
    {
        //zawiera informacje odnośnie klikniętych kart by następnie sprawdzić czy są takie same
        Button buttonPreviously = null;
        Button buttonNext = null;
        //flaga zmieniająca napis w głównym menu po wyjściu sekcji z rozgrywką
        public static bool replay = false;
        int countDown = 0;  //maintain count of visible cards

        //zawiera liste kolekcji przyciskow i obrazkow 
        List<Image> images = new List<Image>();
        List<Button> buttons = new List<Button>();

        //tablica z numerami obrazkow
        int[] board = new int[12] { 1, 2, 3, 4, 5, 6, 6, 5, 4, 3, 2, 1 }; 

        SoundPlayer player = new SoundPlayer("Assets/Sounds/background_music001.wav");

public GamePage()
        {    
            InitializeComponent();
            Stoper.TimerReset();
            Time.Content = Stoper.zeroMinutes + Stoper.minutes.ToString() + ":" + Stoper.zeroSeconds + Stoper.seconds.ToString();
            Initialize_list();
            RandomizeList();
            player.PlayLooping();
        }

        private void Initialize_list()
        {
            images.Add(A1);
            images.Add(A2);
            images.Add(A3);
            images.Add(A4);
            images.Add(A5);
            images.Add(A6);
            images.Add(A7);
            images.Add(A8);
            images.Add(A9);
            images.Add(A10);
            images.Add(A11);
            images.Add(A12);

            buttons.Add(B1);
            buttons.Add(B2);
            buttons.Add(B3);
            buttons.Add(B4);
            buttons.Add(B5);
            buttons.Add(B6);
            buttons.Add(B7);
            buttons.Add(B8);
            buttons.Add(B9);
            buttons.Add(B10);
            buttons.Add(B11);
            buttons.Add(B12);
        }

        //algorytm losuje obrazki by znajdowaly sie w roznych miejsca
        private void RandomizeList()
        {
            new Random().Shuffle(board);
            int randomNumber = 0;
            int i = 0;
            foreach (Image img in images)
            {
                randomNumber = board[i];
                img.Source = new BitmapImage(new Uri("img2/P" + randomNumber + ".png", UriKind.Relative));
                i++;
            }
        }

        //zdarzenie wystepujace podczas klikniecia na karte
        private void ButtonClik(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;

            if (countDown == 0)
            {
                buttonPreviously = b;
                buttonPreviously.Visibility = Visibility.Hidden;
            }
            else if (countDown == 1)
            {
                buttonNext = b;
                buttonNext.Visibility = Visibility.Hidden;
                buttonNext.Refresh();

                System.Threading.Thread.Sleep(500);

                if (Check_image())
                {
                    buttonPreviously.IsEnabled = false;
                    buttonNext.IsEnabled = false;
                }
                else
                {
                    buttonPreviously.Visibility = Visibility.Visible;
                    buttonNext.Visibility = Visibility.Visible;
                }

                if (GameWin())
                {
                    System.Threading.Thread.Sleep(1000);
                    ButtonBack(sender, e);
                }
                countDown = -1;
            }
            countDown++;
        }

        //sprawdza czy widoczne obrazki są takie same

        private bool Check_image()
        {
            string image1 = ImageDictionary(buttonPreviously.Name);
            string image2 = ImageDictionary(buttonNext.Name);
            string src1 = "";
            string src2 = "";

            foreach (Image img in images)
            {
                if (img.Name == image1 || img.Name == image2)
                {
                    if (src1 == "")
                        src1 = img.Source.ToString();
                    else
                        src2 = img.Source.ToString();
                }
            }

            if (src1 == src2)
                return true;

            return false;
        }

        private string ImageDictionary(string btnName)
        {
            //Słownik zawierające nazwy obrazków ktore sa kluczowe w dzialaniu algorytmu

            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "B1", "A1" },
                { "B2", "A2" },
                { "B3", "A3" },
                { "B4", "A4" },
                { "B5", "A5" },
                { "B6", "A6" },
                { "B7", "A7" },
                { "B8", "A8" },
                { "B9", "A9" },
                { "B10", "A10" },
                { "B11", "A11" },
                { "B12", "A12" }
            };

            if (dictionary.ContainsKey(btnName))
            {
                string value = dictionary[btnName];
                return value;
            }

            return "";
        }

        //Sprawdza czy odkryliśmy wszystkie karty
        private bool GameWin()
        {
            foreach (Button button in buttons)
            {
                if (button.IsEnabled == true)
                    return false;
            }

            return true;
        } 

        //Przycisk powrotu do głównego menu
        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            replay = true;
            Stoper.TimerReset();
            player.Stop();
            this.NavigationService.Navigate(new Uri("MainMenuPage.xaml", UriKind.Relative));

        }

        public void GameLoaed(object sender, RoutedEventArgs e)
        {
            //Podczas załadowania sekcji gry następuję inicjalizacja timeru
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += DispatcherTimerTicker;
            dispatcherTimer.Start();
        }
        public void DispatcherTimerTicker(object sender, EventArgs e)
        {
            Stoper.Timer();
            Time.Content = Stoper.zeroMinutes + Stoper.minutes.ToString() + ":" + Stoper.zeroSeconds + Stoper.seconds.ToString();

        }
    }
}

