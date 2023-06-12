using System;
using System.Collections.Generic;
using System.IO;
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

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для FilmPadge.xaml
    /// </summary>
    public partial class FilmPadge : Window
    {
        public Film film { get; set; }
        static string _path = "films.json";
        JsonService JsonServiceFilms = new JsonService(_path);
        public FilmPadge()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string preview = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "previews", film.previewPath);
            igage.Source = new BitmapImage(new Uri(preview));
            DataContext = film;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string filmName = nameTB.Text;
            string rating = ratingTB.Text.Replace(".", ",");
            string janr = janrTB.Text;
            string filmDo = filmDoTB.Text;
            string ticketPrice = ticketPriceTB.Text.Replace(".", ",");
            string duration = durationTB.Text;
            if(string.IsNullOrEmpty(filmName) || string.IsNullOrEmpty(rating) 
                || string.IsNullOrEmpty(janr) || string.IsNullOrEmpty(filmDo)
                || string.IsNullOrEmpty(ticketPrice) || string.IsNullOrEmpty(duration))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
            else
            {
                try
                {
                    film.filmName = filmName;
                    film.rating = Convert.ToDouble(rating);
                    film.filmDo = filmDo;
                    film.duration = duration;
                    film.janr = janr;
                    film.TicketPrice = Convert.ToDouble(ticketPrice);
                    JsonServiceFilms.UpadteFilm(film);
                    MessageBox.Show("Обновлено!", "Успех!");
                    DataContext = film;
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка! Проверьте правильность заполнения полей!");
                    return;
                }
            }

        }
    }
}
