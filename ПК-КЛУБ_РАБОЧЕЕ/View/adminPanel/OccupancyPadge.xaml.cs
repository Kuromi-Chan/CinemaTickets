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
using Kurs.Infrastructure.JSON;
using Kurs.Models;

namespace Kurs.View.adminPanel
{
    /// <summary>
    /// Логика взаимодействия для OccupancyPadge.xaml
    /// </summary>
    
    public partial class OccupancyPadge : Window
    {
        public Film film { get; set; }
        static string _Opath = "occupancy.json";
        JsonService JsonServiceOccupancy = new JsonService(_Opath);
        public OccupancyPadge()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            setOccuancyGrid.Visibility = Visibility.Visible;
            nameTB.Content = film.filmName;
            janrTB.Content = film.janr;
            ratingTB.Content = film.rating;
            durationTB.Content = film.duration;
            filmDoTB.Content = film.filmDo;
            ticketPriceTB.Content = film.TicketPrice;


            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int hallId = Convert.ToInt32(hallNumber.SelectedItem.ToString().Substring(38, 1));
            string occuapDate = OccupDate.Text;
            string occupTimeFrom = OccupTimeFrom.Text;
            string occupTimeTo = OccupTimeTo.Text;
            Dictionary<string, bool> placesOccup = new Dictionary<string, bool>();
            for (int i = 1; i <= 30; i++)
            {
                placesOccup.Add(i.ToString(), false);
            }
            if (!string.IsNullOrEmpty(occuapDate) && !string.IsNullOrEmpty(occupTimeFrom)
                && !string.IsNullOrEmpty(occupTimeTo) && hallId != 0 && hallId != null)
            {
                Occupancy occupancy = new Occupancy(hallId, film.Id, occuapDate, occuapDate, occupTimeFrom, occupTimeTo, placesOccup);
                if (JsonServiceOccupancy.SetOccupancy(occupancy))
                    MessageBox.Show("Занятость сформирована!", "Успех!");
               

                else
                {
                    MessageBox.Show("Конфикт вермени!", "Провал!");
                }
            }

            
        }
    }
}
