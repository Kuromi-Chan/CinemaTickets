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
using Kurs.Infrastructure.JSON;
using Kurs.Models;
using Kurs.View.adminPanel;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для OccupancyAplication.xaml
    /// </summary>
    public partial class OccupancyAplication : UserControl
    {
        static string _path = "films.json";
        JsonService JsonServiceFilms = new JsonService(_path);
        static string _Opath = "occupancy.json";
        JsonService JsonServiceOccupancy = new JsonService(_Opath);

        public Film film { get; set; }
        public OccupancyAplication()
        {
            InitializeComponent();
            this.IsVisibleChanged += UserControl_IsVisibleChanged;
        }

    

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                OccupGrid.ItemsSource = null;
                filmsGrid.ItemsSource = null; 

                List<Film> filmsInProg = JsonServiceFilms.ReadAllFilms();
                filmsGrid.ItemsSource = filmsInProg;
                
                var occupancies = JsonServiceOccupancy.GetOccupancies();
                OccupGrid.ItemsSource = occupancies;
                //foreach (var occupancy in occupancies)
                //{
                //    Film film = JsonServiceFilms.GetFilm(occupancy.FilmId);
                //    OccupGrid.Items.Add(new
                //    {
                //        Id = occupancy.Id,
                //        filmName = film.filmName,
                //        HallId = occupancy.HallId,
                //        OccupancyDateFrom = occupancy.OccupancyDateFrom,
                //        OccupancyTimeFrom = occupancy.OccupancyTimeFrom,
                //        OccupancyTimeTo = occupancy.OccupancyTimeTo

                //    });
                //}


            }
        }

        private void UpdateFilm_Click(object sender, RoutedEventArgs e)
        {
            List<Film> filmsInProg = JsonServiceFilms.ReadAllFilms();
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("filmName") != null)
            {
                var _filmName = dataContext.GetType().GetProperty("filmName").GetValue(dataContext, null);
                film = filmsInProg.FirstOrDefault(f => f.filmName == _filmName.ToString());
            }

            OccupancyPadge a = new OccupancyPadge();
            a.film = film;
            a.Show();


        }


        private void DeleteOccupancy_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("Id") != null)
            {
                var occupId = dataContext.GetType().GetProperty("Id").GetValue(dataContext, null);
                var occupancies = JsonServiceOccupancy.GetOccupancies();
                Occupancy occupancy = occupancies.Find(f => f.Id == Convert.ToInt32(occupId));
                JsonServiceOccupancy.DeleteOccupancy(occupancy);

               
            }
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            OccupGrid.ItemsSource = null;
            var _occupancies = JsonServiceOccupancy.GetOccupancies();
            OccupGrid.ItemsSource = _occupancies;


            //foreach (var occupancy in _occupancies)
            //{
            //    Film film = JsonServiceFilms.GetFilm(occupancy.FilmId);
            //    OccupGrid.Items.Add(new
            //    {
            //        Id = occupancy.Id,
            //        filmName = film.filmName,
            //        HallId = occupancy.HallId,
            //        OccupancyDateFrom = occupancy.OccupancyDateFrom,
            //        OccupancyTimeFrom = occupancy.OccupancyTimeFrom,
            //        OccupancyTimeTo = occupancy.OccupancyTimeTo

            //    });


            //}
        }

        private void UpdateOccupacny_Click(object sender, RoutedEventArgs e)
        {

            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("Id") != null)
            {
                var occupId = dataContext.GetType().GetProperty("Id").GetValue(dataContext, null);
                var occupancies = JsonServiceOccupancy.GetOccupancies();
                Occupancy occupancy = occupancies.Find(f => f.Id == Convert.ToInt32(occupId));
                Film film = JsonServiceFilms.GetFilm(occupancy.FilmId);
                OccupancyUpadte a = new OccupancyUpadte();
                a.film = film;
                a.occupancy = occupancy;
                a.Show();
            }
        }

        private void DeleteFilm_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("Id") != null)
            {
                var occupId = dataContext.GetType().GetProperty("Id").GetValue(dataContext, null);
                var occupancies = JsonServiceOccupancy.GetOccupancies();
                Occupancy occupancy = occupancies.Find(f => f.Id == Convert.ToInt32(occupId));
                JsonServiceOccupancy.DeleteOccupancy(occupancy);

                OccupGrid.ItemsSource = null;
                OccupGrid.ItemsSource = occupancies;
            }
        }
    }
}
