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

namespace Kurs.View
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

     
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            FilmAplicationUC.Visibility = Visibility.Visible;
            OccupancyAplicationUC.Visibility = Visibility.Hidden;
            ReportUC.Visibility = Visibility.Hidden;
            UserPage.Visibility = Visibility.Hidden;
        }


        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            FilmAplicationUC.Visibility = Visibility.Hidden;
            OccupancyAplicationUC.Visibility = Visibility.Visible;
            ReportUC.Visibility = Visibility.Hidden;
            UserPage.Visibility = Visibility.Hidden;

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            
            FilmAplicationUC.Visibility = Visibility.Hidden;
            OccupancyAplicationUC.Visibility = Visibility.Hidden;
            UserPage.Visibility = Visibility.Hidden;
            ReportUC.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            FilmAplicationUC.Visibility = Visibility.Hidden;
            OccupancyAplicationUC.Visibility = Visibility.Hidden;
            ReportUC.Visibility = Visibility.Hidden;
            UserPage.Visibility = Visibility.Visible;
        }
    }
}
