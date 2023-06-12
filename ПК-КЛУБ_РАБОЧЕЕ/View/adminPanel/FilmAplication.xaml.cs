using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using HtmlAgilityPack;
using Kurs.Infrastructure.JSON;
using static System.Net.WebRequestMethods;
using System.Collections.ObjectModel;
using MaterialDesignThemes.Wpf;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для FilmAplication.xaml
    /// </summary>
   


    public partial class FilmAplication : System.Windows.Controls.UserControl
    {

        public List<Film> films = new List<Film>();
        static string _path = "films.json";
        JsonService JsonServiceFilms = new JsonService(_path);
        public FilmAplication()
        {
            InitializeComponent();
            this.IsVisibleChanged += UserControl_IsVisibleChanged;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                // Получение списка фильмов с сайта
                List<Film> filmsFromSite = downloadFilmsFromSite(); // Ваш код получения списка фильмов с сайта
                List<Film> filmsInProg = JsonServiceFilms.ReadAllFilms();


                // Фильтрация списка films1 и удаление фильмов, которые уже есть в existingFilms
                filmsFromSite = filmsFromSite.Where(film => !filmsInProg.Any(filmInProg => filmInProg.filmName == film.filmName)).ToList();




                ObservableCollection<Film> _films = new ObservableCollection<Film>(filmsFromSite);
                filmsGrid1.ItemsSource = _films;
                filmsGrid.ItemsSource = filmsInProg;
            }
        }
        
        public List<Film> downloadFilmsFromSite()
        {
            // Создаем объект HtmlWeb для загрузки HTML-страницы
            HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = null;
            try
            {
                // Загружаем HTML-страницу
                doc = web.Load("https://afisha.me/film/");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Не удается выгрузить фильмы с сайта :(");
                return null;
            }

            // Используем XPath для выбора элементов <li> с классом "lists__li "
            HtmlNodeCollection liElements = doc.DocumentNode.SelectNodes("//div[@class='events-block js-cut_wrapper' and @id='events-block']//li[contains(@class, 'lists__li')]");



            // Выводим содержимое каждого элемента <li>
            if (liElements != null)
            {
                foreach (HtmlNode liElement in liElements)
                {
                    HtmlNode linkNode = liElement.SelectSingleNode(".//a[@class='media']");
                    //сохраняем превью
                    string fileName;
                    string previewPath = linkNode?.SelectSingleNode("img")?.GetAttributeValue("src", "");
                    using (WebClient client = new WebClient())
                    {
                        fileName = System.IO.Path.GetFileName(previewPath);
                        string savePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "previews", fileName);
                        try
                        {
                            client.DownloadFile(previewPath, savePath);
                        }
                        catch (Exception)
                        {
                            System.Windows.MessageBox.Show("Ошибка загрузки фильмов, перезапустите программу");
                        }


                    }




                    string filmName = liElement.SelectSingleNode(".//a[@class='name']")?.InnerText;

                    //вырываем жанр из мешанины
                    string[] janr = liElement.SelectSingleNode(".//div[@class='txt']/p")?.InnerText.Split(',');

                    //достаем дату проката 
                    string element = Array.Find(janr, x => x.Contains("до"));
                    string[] parts;
                    string formattedDate = null;
                    if (element != null)
                        parts = element.Split(new[] { "&nbsp;" }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        continue;
                    if (parts.Length >= 3)
                    {
                        string dayStr = parts[1].TrimStart('0');
                        string monthStr = parts[2];

                        if (int.TryParse(dayStr, out int day) && DateTime.TryParseExact(monthStr, "MMMM", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime month))
                        {
                            DateTime result = new DateTime(DateTime.Now.Year, month.Month, day);
                            formattedDate = result.ToString("dd.MM.yyyy"); // дата есть


                        }
                    }


                    int index = Array.IndexOf(janr, janr.FirstOrDefault(x => x.StartsWith("  до")));
                    string _janr = "";
                    if (index != -1)
                    {
                        string[] elementsBefore = janr.Take(index).ToArray();
                        _janr = string.Join(",", elementsBefore);
                    }

                    double rating = 7.1;
                    double ticketPrice = 12; // Ваше значение
                    string duration = "120 минут"; // Ваше значение

                    Film film = new Film(fileName,filmName, _janr, rating, formattedDate, ticketPrice, duration);
                    films.Add(film);
                }
                
            }
            return films;
        }

        private void GoToDescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("filmName") != null)
            {
                var _filmName = dataContext.GetType().GetProperty("filmName").GetValue(dataContext, null);
                Film film = films.FirstOrDefault(f => f.filmName == _filmName);
                JsonServiceFilms.SetFilm(film);

                Film filmToRemove = films.Find(f => f.Id == film.Id); // Или другое условие сравнения
                films.Remove(filmToRemove);
                ObservableCollection<Film> _films = new ObservableCollection<Film>(films);



                filmsGrid1.ItemsSource = null;
                filmsGrid1.ItemsSource = _films;

                filmsGrid.ItemsSource = null;
                filmsGrid.ItemsSource = JsonServiceFilms.ReadAllFilms();
            }
        }

        private void GoToDescriptionButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("filmName") != null)
            {
                var _filmName = dataContext.GetType().GetProperty("filmName").GetValue(dataContext, null);
                List<Film> filmss = JsonServiceFilms.ReadAllFilms();
                Film film = filmss.Find(f => f.filmName == _filmName.ToString());
                JsonServiceFilms.DeleteFilm(film);


                filmsGrid.ItemsSource = null;
                filmsGrid.ItemsSource = JsonServiceFilms.ReadAllFilms();
            }
        }

        private void UpdateFilm_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("filmName") != null)
            {
                var _filmName = dataContext.GetType().GetProperty("filmName").GetValue(dataContext, null);
                List<Film> filmss = JsonServiceFilms.ReadAllFilms();
                Film film = filmss.Find(f => f.filmName == _filmName.ToString());
                var filmDescription = new FilmPadge();
                filmDescription.film = film;
                filmDescription.Show();
            }
            
        }
    }
}
