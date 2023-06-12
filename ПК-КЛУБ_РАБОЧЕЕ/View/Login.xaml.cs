
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using Kurs.View;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void loginFieldGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (sender as TextBox);
            if (tb == null)
                return;

            tb.Text = "";
        }

        private void PasswordBoxGotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox tb = (sender as PasswordBox);
            if (tb == null)
                return;

            tb.Password = "";
        }
       
        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            



            string LoginInput = Convert.ToString(loginField);
            string PasswordInput = Convert.ToString(passwordField);
            if(loginField.Text== "1" && passwordField.Password == "1")
            {
                var mainWindow = App.Current.MainWindow as MainWindow;
               
                mainWindow.Close();
                AdminPanel admin = new AdminPanel();
                admin.Show();

                Visibility = System.Windows.Visibility.Hidden;
                return;
            }
            if (RightLoginFormat(LoginInput) == true && RightPasswordFormat(PasswordInput) == true)
            {
                string json = File.ReadAllText("Users.json");
                JArray usersArray = JArray.Parse(json);

                foreach (JObject userObj in usersArray)
                {
                    string userEmail = userObj["Email"].ToString();
                    string userPassword = userObj["Password"].ToString();
                    if (userEmail.Equals(loginField.Text) && userPassword.Equals(passwordField.Password))
                    {
                        string userName = userObj["Name"].ToString();
                        string userSurname = userObj["Surname"].ToString();
                        string userId = userObj["Id"].ToString();
                        User m_user = new User(Convert.ToInt32(userId), userEmail, userName, userSurname);

                        var mainWindow = App.Current.MainWindow as MainWindow;
                        MainWindow.loged = true;
                        MainWindow.user = m_user;
                        Visibility = System.Windows.Visibility.Hidden;
                        return;
                    }
                }

                MessageBox.Show("Мы не нашли тебя в базе данных. Возможно, ты где-то ошибся");
            }
        }
        static bool RightLoginFormat(string LoginInput)
        {
            Regex Loginformat = new Regex("(.)@(gmail.com|mail.ru)");

            var matchLogin = Loginformat.Match(LoginInput);

            if (!matchLogin.Success)
            {
                MessageBox.Show("Неверный формат логина!");
                return false;
            }
            else
                return true;
        }
        static bool RightPasswordFormat(string PasswordInput)
        {
            Regex Passformat = new Regex("(.){16}");
            var mathPassword = Passformat.Match(PasswordInput);
            if (!mathPassword.Success)
            {
                MessageBox.Show("Неверный формат пароля!");
                return false;
            }
            else
                return true;
        }

      

        private void RegistrationLable_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            Visibility = System.Windows.Visibility.Hidden;
            mainWindow.registrationPage.Visibility = System.Windows.Visibility.Visible;
            mainWindow.filmsGrid.Visibility = Visibility.Hidden;
            mainWindow.SearchPage.Visibility = Visibility.Hidden;
            mainWindow.AccountPage.Visibility = Visibility.Hidden;
            mainWindow.buyPage.Visibility = Visibility.Hidden;
        }
    }
}
