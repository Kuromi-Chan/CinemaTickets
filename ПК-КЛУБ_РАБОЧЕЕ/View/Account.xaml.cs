using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Kurs.View;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для Account.xaml
    /// </summary>
    public partial class Account : UserControl
    {
        static List<Ticket> tickets { get;set; }
        static List<Occupancy> occupancies = new List<Occupancy>();
        static List<Film> films = new List<Film>();

        static string _path = "Users.json";
        static string o_path = "occupancy.json";
        static string f_path = "films.json";
        JsonService JsonServiceUsers = new JsonService(_path);
        JsonService JsonServiceOpccupancy = new JsonService(o_path);
        JsonService JsonServiceFilms = new JsonService(f_path);
        public Account()
        {
            InitializeComponent();
        }

        private void logOutLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.loged = false;
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.AccountPage.Visibility = Visibility.Hidden;
            mainWindow.filmsGrid.Visibility = Visibility.Visible;
        }

        private void accountPage_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {

                tickets = JsonServiceUsers.GetUserTickets(MainWindow.user.Id);
                acritveTicketsGrid.Items.Clear();


                foreach (var ticket in tickets)
                {

                    string filmName = JsonServiceFilms.GetFilm(JsonServiceOpccupancy.GetOccupancy(ticket.OccupancyId).FilmId).filmName;
                    string OccupancyDateFrom = JsonServiceOpccupancy.GetOccupancy(ticket.OccupancyId).OccupancyDateFrom;
                    string OccupancyTimeFrom = JsonServiceOpccupancy.GetOccupancy(ticket.OccupancyId).OccupancyTimeFrom;
                    int OccupancyId = JsonServiceOpccupancy.GetOccupancy(ticket.OccupancyId).Id;
                    int PlaceNumber = ticket.PlaceNumber;
                    int HallId = JsonServiceOpccupancy.GetOccupancy(ticket.OccupancyId).HallId;


                    var fullDate = OccupancyDateFrom + " " + OccupancyTimeFrom;
                    DateTime date = DateTime.ParseExact(fullDate, "dd.MM.yyyy HH:mm", null);
                    if (date.CompareTo(DateTime.Now) < 0) //если билет неактивный
                    {
                        continue;
                    }

                    acritveTicketsGrid.Items.Add(new { 
                        Id = OccupancyId,
                        filmName = filmName, 
                        OccupancyDateFrom = OccupancyDateFrom, 
                        OccupancyTimeFrom = OccupancyTimeFrom,
                        PlaceNumber = ticket.PlaceNumber, 
                        HallId = HallId});
                }
                
                SortDescription sortDescriptionDate = new SortDescription("OccupancyDateFrom", ListSortDirection.Ascending);
                SortDescription sortDescriptionTime = new SortDescription("OccupancyTimeFrom", ListSortDirection.Ascending);
                acritveTicketsGrid.Items.SortDescriptions.Add(sortDescriptionDate);
                acritveTicketsGrid.Items.SortDescriptions.Add(sortDescriptionTime);
                acritveTicketsGrid.Items.Refresh();
            }
        }

        private void Podrobnee_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = (sender as FrameworkElement).DataContext;

            if (dataContext != null && dataContext.GetType().GetProperty("Id") != null)
            {
                var idValue = dataContext.GetType().GetProperty("Id").GetValue(dataContext, null);
                var Occupancyid = (int)idValue;
                Occupancy occupancy = JsonServiceOpccupancy.GetOccupancy(Occupancyid);
                Film film = JsonServiceFilms.GetFilm(occupancy.FilmId);
                int PlaceNumber = Convert.ToInt32(dataContext.GetType().GetProperty("PlaceNumber").GetValue(dataContext, null));
                var ticketDescription = new TicketDescription();
                ticketDescription.film = film;
                ticketDescription.occuapncy = occupancy;
                ticketDescription.placeNumber = PlaceNumber;
                ticketDescription.Show();
            }
        }
    }
    
}
