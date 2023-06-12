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
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kurs.Infrastructure.JSON;
using Kurs.Models;
using Brushes = System.Windows.Media.Brushes;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для BuyPage.xaml
    /// </summary>
    public partial class BuyPage : UserControl
    {
        static string _path = "occupancy.json";
        JsonService JsonServiceOccupancy = new JsonService(_path);
        static string u_path = "Users.json";
        JsonService JsonServiceUsers = new JsonService(u_path);
        static List <int> selectedSeats { get;set; }
        public Film film { get; set; }
        public Occupancy CurrentFilmOcuppancy { get; set; }
        static List<Ticket> tickets { get; set; }
        static double TotalPrice { get; set; }
        static Ticket ticket { get; set; }

        public int Counter { get; set; }
        public int TicketCounter { get; set; }
        private Dictionary<string, bool> seatSelection { get; set; } // словарь, хранящий информацию о выбранных местах
        public BuyPage()
        {
            selectedSeats = new List<int>();
            InitializeComponent();
            this.IsVisibleChanged += buyGridUC_IsVisibleChanged;
        }


        private void buyGridUC_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                loadHall();
            }
        }

        public void loadHall()
        {
            seatSelection = new Dictionary<string, bool>();
            showDateAndTime.Content = CurrentFilmOcuppancy.OccupancyDateFrom + " " +
                CurrentFilmOcuppancy.OccupancyTimeFrom + " * Кинотеатр «Беларусь»";
            tickets = new List<Ticket>();
            filmNameLable.Content = film.filmName;


            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 7; column++)
                {
                    var seatNumber = row * 7 + column + 1; // вычисляем номер места
                    if (CurrentFilmOcuppancy.PlacesOccupancy[Convert.ToString(seatNumber)] == true)
                    {
                        var seat = new Ellipse
                        {
                            Width = 20,
                            Height = 20,
                            Fill = Brushes.DarkGray,
                            Stroke = Brushes.DarkGray,
                            StrokeThickness = 1,
                            ToolTip = "Место " + seatNumber // добавляем подсказку с номером места
                        };
                        Canvas.SetLeft(seat, 30 + column * 30); // устанавливаем позицию места по горизонтали
                        Canvas.SetTop(seat, 30 + row * 30); // устанавливаем позицию места по вертикали
                        cinemaHall.Children.Add(seat); // добавляем место на Canvas
                    }
                    else
                    {

                        var seat = new Ellipse
                        {
                            Width = 20,
                            Height = 20,
                            Fill = Brushes.MediumPurple,
                            Stroke = Brushes.MediumPurple,
                            StrokeThickness = 1,
                            ToolTip = "Место " + seatNumber // добавляем подсказку с номером места

                        };
                        Canvas.SetLeft(seat, 30 + column * 30); // устанавливаем позицию места по горизонтали
                        Canvas.SetTop(seat, 30 + row * 30); // устанавливаем позицию места по вертикали
                        seat.MouseLeftButtonDown += Seat_MouseLeftButtonDown;
                        seat.MouseEnter += Seat_MouseEnter; // добавляем обработчик события
                        seat.MouseLeave += Seat_MouseLeave; // добавляем обработчик события
                        seat.Name = "Место" + seatNumber;
                        seatSelection.Add(seat.Name, false);
                        cinemaHall.Children.Add(seat); // добавляем место на Canvas

                    }

                }
            }
        }
        private void Seat_MouseEnter(object sender, MouseEventArgs e)
        {
                var seat = (Ellipse)sender;
                seat.Width = 20;
                seat.Height = 20;
                seat.Fill = Brushes.White; // изменяем цвет места при наведении на него курсора мыши
                seat.Stroke = Brushes.MediumPurple;
                seat.StrokeThickness = 4;
            
        }

        private void Seat_MouseLeave(object sender, MouseEventArgs e)
        {
            var seat = (Ellipse)sender;
            var seatName = seat.Name; // получаем имя места
            try
            {
                if (!seatSelection[seatName]) // если место не выбрано, изменяем его цвет
                {
                    seat.Width = 20;
                    seat.Height = 20;
                    seat.Fill = Brushes.MediumPurple; // возвращаем исходный цвет места при уходе курсора мыши
                    seat.Stroke = Brushes.MediumPurple;
                    seat.StrokeThickness = 1;
                }
            }
            catch (KeyNotFoundException)
            {
                seat.Width = 20;
                seat.Height = 20;
                seat.Fill = Brushes.DarkGray;
                seat.Stroke = Brushes.DarkGray;
                seat.StrokeThickness = 1;
            }
        }
        private void buyGrid_Loaded(object sender, RoutedEventArgs e)
        {
           

        }
        private void Seat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            var seat = (Ellipse)sender;
            var seatNumber = seat.Name.Substring(5);
            var seatName = seat.Name;

            if (seatSelection[seatName] == false)
            {
               
                seatSelection[seatName] = true;
                if (seat.Fill == Brushes.White)
                {
                    seat.Width = 20;
                    seat.Height = 20;
                    seat.Fill = Brushes.White; // изменяем цвет места при наведении на него курсора мыши
                    seat.Stroke = Brushes.MediumPurple;
                    seat.StrokeThickness = 4;
                    selectedSeats.Add(Convert.ToInt32(seatNumber));

                    TicketCounter += 1;
                    ticket = new Ticket(TicketCounter, CurrentFilmOcuppancy.Id,
                        Convert.ToInt32(seat.Name.Substring(5, seat.Name.Length-5)), film.TicketPrice);
                    tickets.Add(ticket);
                    TotalPrice = tickets.Sum(t => Convert.ToDouble(t.TicketPrice));
                    totalPriceLabel.Content = "Итого: " + Convert.ToString(TotalPrice) + " р";
                    ticketssGrid.ItemsSource = null;
                    ticketssGrid.ItemsSource = tickets;
                   
                }
                else
                {
                    tickets.RemoveAll(ticket => ticket.PlaceNumber == Convert.ToInt32(seat.Name.Substring(5, seat.Name.Length - 5)));
                    ticketssGrid.ItemsSource = null;
                    ticketssGrid.ItemsSource = tickets;
                    TotalPrice = tickets.Sum(t => Convert.ToDouble(t.TicketPrice));


                    totalPriceLabel.Content = "Итого: " + Convert.ToString(TotalPrice) + " р";
                    seat.Width = 20;
                    seat.Height = 20;
                    seat.Fill = Brushes.MediumPurple; // возвращаем исходный цвет места при уходе курсора мыши
                    seat.Stroke = Brushes.MediumPurple;
                    seat.StrokeThickness = 1;
                }
            }
            else
            {
                seatSelection[seatName] = false;
                var _seat = Convert.ToInt32(seat.Name.Substring(5, seat.Name.Length - 5));
                tickets.RemoveAll(ticket => ticket.PlaceNumber == _seat);
                TotalPrice = tickets.Sum(t => Convert.ToDouble(t.TicketPrice));
                totalPriceLabel.Content = "Итого: " + Convert.ToString(TotalPrice) + " р";
                ticketssGrid.ItemsSource = null;
                ticketssGrid.ItemsSource = tickets;
            }
        }

        private void backLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ticketssGrid.ItemsSource = null;
            totalPriceLabel.Content = "";
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.buyPage.Visibility = Visibility.Hidden;
            mainWindow.filmDescription.Visibility = Visibility.Visible;
            seatSelection.Clear();
        }

        private void ContinueLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(tickets.Count != 0 || tickets != null)
            {
                if(MainWindow.loged == true)
                {
                    //Counter += 1;
                    //Order order = new Order(Counter, MainWindow.user.Id, tickets.Select(ticket => ticket.Id).ToList(), Convert.ToString(totalPriceLabel.Content));
                    JsonServiceUsers.SetUserTicket(tickets, MainWindow.user.Id);
                    JsonServiceOccupancy.SetOccupancy(selectedSeats, CurrentFilmOcuppancy.Id);
                    MessageBox.Show("Билеты занесены в ваш личный аккаунт!");
                    CurrentFilmOcuppancy = JsonServiceOccupancy.GetOccupancy(CurrentFilmOcuppancy.Id);
                    ticketssGrid.ItemsSource = null;
                    totalPriceLabel.Content = "";
                    loadHall();
                    
                    var mainWindow = App.Current.MainWindow as MainWindow;
                }
                else
                {
                    Login LoginForm = new Login();
                    LoginForm.Show();
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали ни одного билета!");
            }
        }
    }
}
