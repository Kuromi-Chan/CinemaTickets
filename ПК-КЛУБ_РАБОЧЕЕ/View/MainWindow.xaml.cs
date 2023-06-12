
using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Kurs.Infrastructure.JSON;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool loged = false;
        static string _path = "films.json";
        List<Film> _films;
        JsonService JsonServiceFilms = new JsonService(_path);
        public static User user { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AccountLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (!loged)
            {
                Login LoginForm = new Login();
                LoginForm.Show();
            }
            else
            {
                filmsGrid.Visibility = Visibility.Hidden;
                AccountPage.Visibility = System.Windows.Visibility.Visible;
                filmDescription.Visibility = System.Windows.Visibility.Hidden;
                SearchPage.Visibility = Visibility.Hidden;
                buyPage.Visibility = System.Windows.Visibility.Hidden;
                registrationPage.Visibility = System.Windows.Visibility.Hidden;
            }
        }
      

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            filmsGrid.ItemsSource = JsonServiceFilms.ReadAllFilms();


        }
        public static string switchMonth(DateTime data)
        {
            string cuttedData = data.Day.ToString();
            string month = "";
            switch (data.Month)
            {
                case 1:
                    month = "января";
                    break;
                case 2:
                    month = "ферваля";
                    break;
                case 3:
                    month = "марта";
                    break;
                case 4:
                    month = "апреля";
                    break;
                case 5:
                    month = "мая";
                    break;
                case 6:
                    month = "июня";
                    break;
                case 7:
                    month = "июля";
                    break;
                case 8:
                    month = "августа";
                    break;
                case 9:
                    month = "сентября";
                    break;
                case 10:
                    month = "октября";
                    break;
                case 11:
                    month = "ноября";
                    break;
                case 12:
                    month = "декабря";
                    break;
            }
            return cuttedData + " " + month;
        }
        
        private void goToMainPage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccountPage.Visibility = Visibility.Hidden;
            registrationPage.Visibility = Visibility.Hidden;
            filmDescription.Visibility = Visibility.Hidden;
            buyPage.Visibility = Visibility.Hidden;
            SearchPage.Visibility = Visibility.Hidden;
            filmsGrid.Visibility = Visibility.Visible;
            
        }

        private void searchLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            AccountPage.Visibility = Visibility.Hidden;
            registrationPage.Visibility = Visibility.Hidden;
            filmDescription.Visibility = Visibility.Hidden;
            buyPage.Visibility = Visibility.Hidden;
            filmsGrid.Visibility = Visibility.Hidden;
            SearchPage.Visibility = Visibility.Visible;
        }

        private void GoToDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            Film selectedFilm = (sender as FrameworkElement).DataContext as Film;
            filmDescription.film = selectedFilm;
            filmsGrid.Visibility = Visibility.Hidden;
            filmDescription.PreviousControl = "";
            filmDescription.Visibility = Visibility.Visible;
            
        }

        private void MenuImage_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            if (image != null)
            {
                Uri uri = new Uri(image.Source.ToString());
                string imagePath = uri.LocalPath;
                string newImagePath = imagePath.Substring(0, imagePath.Length - 4) + "-1.png";
                image.Source = new BitmapImage(new Uri(newImagePath, UriKind.Relative));
            }


        }

        private void MenuImage_MouseLeave(object sender, MouseEventArgs e)
        {

            Image image = sender as Image;
            if (image != null)
            {
                Uri uri = new Uri(image.Source.ToString());
                string imagePath = uri.LocalPath;
                string newImagePath = imagePath.Substring(0, imagePath.Length - 6) + ".png";
                image.Source = new BitmapImage(new Uri(newImagePath, UriKind.Relative));
            }

        }

        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Отключение всех привязок данных в главном окне
            DataContext = null;

        }
    }
}
