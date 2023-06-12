using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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
using Kurs.Infrastructure.JSON;
using Kurs.Models;
using HtmlAgilityPack;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для FilmDescription.xaml
    /// </summary>
    public partial class FilmDescription : UserControl
    {
        
        static string _path = "occupancy.json";
        JsonService JsonServiceOccupancy = new JsonService(_path);
        public Film film { get; set; }
        static List<Occupancy> currentFilmOcuppancies { get; set; }
        public string PreviousControl { get; set; }


        public FilmDescription()
        {
            InitializeComponent();

            this.IsVisibleChanged += filmDescriptionUC_IsVisibleChanged;
        }
        

        private void filmDescriptionUC_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private void filmDescriptionUC_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                

                // Далее выполните необходимые действия для отображения usercontrol FilmDescription

                price.Content = film.TicketPrice.ToString() + " рублей";
                currentFilmOcuppancies = JsonServiceOccupancy.GetOccupancyByFilmId(film.Id);
                FuckedGrid.ItemsSource = currentFilmOcuppancies;
                DataContext = film;
            }
        }

        private void backLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.filmDescription.Visibility = Visibility.Hidden;
            if (PreviousControl == mainWindow.SearchPage.Name)
            {
                mainWindow.SearchPage.Visibility = Visibility.Visible;
            }
            else
            {

                mainWindow.filmsGrid.Visibility = Visibility.Visible;
            }
            
        }
 
        

        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            var brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x67, 0x3A, 0xB7));
            label.Foreground = brush;
        }

        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = (Label)sender;
            var brush = new SolidColorBrush(Color.FromArgb(0xDD, 0xDD, 0xDD, 0xDD));
            label.Foreground = brush;
        }

        private void GoTicketsButton_Click(object sender, RoutedEventArgs e)
        {

            Occupancy selectedOccupancy = (sender as FrameworkElement).DataContext as Occupancy;
           
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.buyPage.CurrentFilmOcuppancy = selectedOccupancy;
            mainWindow.filmDescription.Visibility = Visibility.Hidden;
            mainWindow.buyPage.film = film;
            mainWindow.buyPage.Visibility = Visibility.Visible;
        }
    }
}
