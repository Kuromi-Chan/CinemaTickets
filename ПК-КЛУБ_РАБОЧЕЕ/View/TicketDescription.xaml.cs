using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Xps;
using Kurs.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Kurs.View
{
    /// <summary>
    /// Логика взаимодействия для TicketDescription.xaml
    /// </summary>
    public partial class TicketDescription : System.Windows.Window
    {
        public Film film { get; set; }
        public Occupancy occuapncy { get; set; }
        public int placeNumber { get; set; }
        public TicketDescription()
        {
            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filmNameLable.Content = film.filmName;
            hallLabel.Content = occuapncy.HallId;
            PlaceLabel.Content = placeNumber;
            OccupTimeFromLable.Content = occuapncy.OccupancyTimeFrom;
            OccupTimeToLable.Content = occuapncy.OccupancyTimeTo;
            OccupTimeFromLable2.Content = occuapncy.OccupancyTimeFrom;
            OccupDateFromLable.Content = occuapncy.OccupancyDateFrom;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap(
    500, 500, 96, 96, PixelFormats.Default);
            renderTargetBitmap.Render(ticketToPrint);
            PngBitmapEncoder bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintQueue printQueue = printDialog.PrintQueue;
                XpsDocumentWriter xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);

                FixedDocument fixedDocument = new FixedDocument();
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage();

                // Установка размера A4
                fixedPage.Width = 793.7; // 210 мм в пикселях при разрешении 96 dpi
                fixedPage.Height = 1122.5; // 297 мм в пикселях при разрешении 96 dpi

                fixedPage.Children.Add(new Image { Source = renderTargetBitmap });
                ((IAddChild)pageContent).AddChild(fixedPage);
                fixedDocument.Pages.Add(pageContent);

                
                // Напечатайте документ
                xpsDocumentWriter.Write(fixedDocument);
            }


        }
    }
}
