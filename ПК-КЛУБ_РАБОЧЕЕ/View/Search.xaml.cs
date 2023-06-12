using System;
using System.Collections.Generic;
using System.Drawing;
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
using Kurs.Infrastructure.JSON;
using Kurs.Models;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class Search : UserControl
    {
        bool filtersOn = false;
        Film foundedFilm;
        static string _path = "films.json";
        JsonService JsonServiceFilms = new JsonService(_path);
        public Search()
        {
            InitializeComponent();
        }

        public void DoSearch()
        {
            try
            {
                filmsGrid.Items.Clear();
            }
            catch
            {
                filmsGrid.ItemsSource = null;
            }
                string searchFilmName = filmNameTBox.Text;
                if (searchFilmName == "Введите название фильма")
            {
                searchFilmName = null;
            }

            string searchJanr = "";
                string searchDate = "";
                string searchRaiting = "";
                string searchTicketPrice = "";
                if (janrCBox.SelectedItem != null)
                {
                    int leng = janrCBox.SelectedItem.ToString().Length - 38;
                    searchJanr = janrCBox.SelectedItem.ToString().Substring(38, leng);  
                }
                if (showDatePicker.Text != null)
                {
                    searchDate = showDatePicker.Text;
                }
                if (raitingCBox.SelectedItem != null)
                {
                    searchRaiting = raitingCBox.SelectedItem.ToString().Substring(38, 1);
                }
                if (TicketPriceCBox.SelectedItem != null)
                {
                    searchTicketPrice = TicketPriceCBox.SelectedItem.ToString().Substring(38, 2);
                }

                List<Film> filtredFilms = JsonServiceFilms.GetFilmsBySorting(searchFilmName, searchJanr, searchDate, searchRaiting, searchTicketPrice);
                filtredFilms = filtredFilms.GroupBy(x => x.filmName).Select(g => g.FirstOrDefault()).ToList();
                filmsGrid.ItemsSource = filtredFilms;

            }
    


   

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DoSearch();
        }

        private void aplyFilter_Click(object sender, RoutedEventArgs e)
        {
            filtersOn = true;
            DoSearch();
            //System.Windows.Media.Color darkGrayColor = System.Windows.Media.Color.FromRgb(64, 64, 64); // пример цвета dark gray
            //aplyFilter.Background = new SolidColorBrush(darkGrayColor);
        }

        private void deleteFilter_Click(object sender, RoutedEventArgs e)
        {
            filtersOn = false;
            janrCBox.Text = null;
            showDatePicker.Text = null;
            raitingCBox.Text = null;
            TicketPriceCBox.Text = null;

            System.Windows.Media.Color buttonColor = System.Windows.Media.Color.FromArgb(255, 103, 58, 183);
            aplyFilter.Background = new SolidColorBrush(buttonColor);
            DoSearch();
        }

        private void filmNameTBox_GotFocus(object sender, RoutedEventArgs e)
        {
            filmNameTBox.Text = "";
        }

        private void filmsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // Получаем выбранный объект Film
                Film selectedFilm = e.AddedItems[0] as Film;
                if (selectedFilm != null)
                {
                   

                    var mainWindow = App.Current.MainWindow as MainWindow;
                    mainWindow.filmDescription.PreviousControl = mainWindow.SearchPage.Name;

                    mainWindow.filmDescription.film = selectedFilm;
                    mainWindow.filmsGrid.Visibility = Visibility.Hidden;
                    mainWindow.SearchPage.Visibility = Visibility.Hidden;
                    mainWindow.filmDescription.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
