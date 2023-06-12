using BoldReports.Processing.ObjectModel;
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

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для UserAplication.xaml
    /// </summary>
    public partial class UserAplication : UserControl
    {
        
        public UserAplication()
        {
            InitializeComponent();
            this.IsVisibleChanged += UserControl_IsVisibleChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = "";
            string surname = "";
            string email = "";
            string pass = "";

            if(!string.IsNullOrEmpty(nameTB.Text) && !string.IsNullOrEmpty(surnameTB.Text)
                && !string.IsNullOrEmpty(emailTB.Text) && !string.IsNullOrEmpty(passwordTB.Text))
            {
                JsonService jsonServiceUser = new JsonService("Users.json");
                User newUser = new User(emailTB.Text, passwordTB.Text, nameTB.Text, surnameTB.Text, new List<Ticket>());
                jsonServiceUser.SetNewUser(newUser);
                MessageBox.Show("Добавлено!");
                UsersGrid.ItemsSource = null;
                UsersGrid.ItemsSource = jsonServiceUser.ReadAllUsers();
            }
            else
            {
                MessageBox.Show("Заполните все поля!!!");
            }
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true && (bool)e.OldValue == false)
            {
                UsersGrid.ItemsSource = null;
                JsonService jsonServiceUser = new JsonService("Users.json");
                UsersGrid.ItemsSource = jsonServiceUser.ReadAllUsers();


            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {

            JsonService jsonServiceUser = new JsonService("Users.json");
            var dataContext = (sender as FrameworkElement).DataContext;
            if (dataContext != null && dataContext.GetType().GetProperty("Id") != null)
            {
                var _Id = dataContext.GetType().GetProperty("Id").GetValue(dataContext, null);
                User user = jsonServiceUser.ReadAllUsers().FirstOrDefault(f => f.Id == Convert.ToInt32(_Id.ToString()));
                jsonServiceUser.DeleteUser(user);
                MessageBox.Show("Пользователь удален!");
                UsersGrid.ItemsSource = null;
                UsersGrid.ItemsSource = jsonServiceUser.ReadAllUsers();
            }
            
        }
    }
}
