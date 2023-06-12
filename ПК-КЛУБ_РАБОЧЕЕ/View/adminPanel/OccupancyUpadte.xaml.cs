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

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для OccupancyUpadte.xaml
    /// </summary>
    public partial class OccupancyUpadte : Window
    {
        public Film film { get; set; }
        public Occupancy occupancy { get; set; }
        static string _Opath = "occupancy.json";
        JsonService JsonServiceOccupancy = new JsonService(_Opath);
        public OccupancyUpadte()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            nameTB.Content = film.filmName;
            janrTB.Content = film.janr;
            ratingTB.Content = film.rating;
            durationTB.Content = film.duration;
            filmDoTB.Content = film.filmDo;
            ticketPriceTB.Content = film.TicketPrice;


            HallIdTB.Text = occupancy.HallId.ToString();
            OccupDateTB.Text = occupancy.OccupancyDateFrom;
            OccupTimeFromTB.Text = occupancy.OccupancyTimeFrom;
            OccupTimeToTB.Text = occupancy.OccupancyTimeTo;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int hallId = 0;
            string occupancyDateFrom = null;
            string occupancyTimeFrom = null;
            string occupancyTimeTo = null;
            try
            {
                hallId = Convert.ToInt32(HallIdTB.Text);
                occupancyDateFrom = OccupDateTB.Text;
                occupancyTimeFrom = OccupTimeFromTB.Text;
                occupancyTimeTo = OccupTimeToTB.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Заполните поля валидными данными!");
                return;
            }
            if(hallId == 0 || string.IsNullOrEmpty(occupancyDateFrom)
                || string.IsNullOrEmpty(occupancyTimeFrom) || string.IsNullOrEmpty(occupancyTimeTo))
            {
                MessageBox.Show("Заполните все поля данными!");
                return;
            }
            else
            {
                occupancy.HallId = hallId;
                occupancy.OccupancyDateFrom = occupancyDateFrom;
                occupancy.OccupancyTimeFrom = occupancyTimeFrom;
                occupancy.OccupancyTimeTo = occupancyTimeTo;
                if(
                JsonServiceOccupancy.UpadteOccupancy(occupancy))
                {
                    MessageBox.Show("Обновлено!", "Успех!!!");
                }
                else
                {
                    MessageBox.Show("Конфликт времени!", "Провал!!!");
                }
            }
            
        }
    }
}
