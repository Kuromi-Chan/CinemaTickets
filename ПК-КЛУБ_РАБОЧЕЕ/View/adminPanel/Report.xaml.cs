﻿using System;
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
using Kurs.Infrastructure;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        public Report()
        {
            InitializeComponent();
            
        }
      

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {

                DbContext dbContext = new DbContext();
                dbContext.InsertFilms();

                this.ReportViewer.ReportPath = System.IO.Path.Combine(Environment.CurrentDirectory, "C:\\Users\\bizyu\\Desktop\\Курсовой проект\\Отчет\\Report1.rdl");
                this.ReportViewer.RefreshReport();

            }
        }
    }
}
