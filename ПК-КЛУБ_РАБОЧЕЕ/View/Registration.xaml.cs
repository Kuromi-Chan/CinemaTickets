using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Kurs.Infrastructure.JSON;
using Kurs.Models;

namespace Kurs
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : UserControl
    {
        public Registration()
        {
            InitializeComponent();
        }

        private bool isEmailValid(TextBox email)
        {
            Regex EmailPattern = new Regex("(.)@(gmail.com|mail.ru){1}");

            var matchEmail = EmailPattern.Match(Convert.ToString(email));

            if (matchEmail.Success) 
                return true;
            else
                return false;
        }
        private bool isTelephoneValid(TextBox telephone)
        {
            Regex telephonePattern = new Regex(@"\+375\d{9}");
            var matchTelephone = telephonePattern.Match(Convert.ToString(telephone));
            if (matchTelephone.Success)
                return true;
            else
                return false;
        }
        private bool isPasswordValid(string pass1 , string pass2)
        {
            if (!pass1.Equals(pass2) || pass1 == "")
                return false;
            else
                return true;
        }
        
        private bool checkEmpty(TextBox textBox)
        {
           
            if (textBox.Text == "" )
            {
                textBox.BorderBrush = Brushes.Red;
                return false;
            }
            else
            {
                textBox.BorderBrush = Brushes.DarkOliveGreen;
            }
            return true;
        }
        
        private void RegistrationButtonClick(object sender, RoutedEventArgs e)
        {




            bool allFieldsAreValid = true;
            if (!isEmailValid(emailField) || !checkEmpty(emailField))
            {
                emailField.BorderBrush = Brushes.Red;
                
                allFieldsAreValid = false;
            }
            else
            {
                emailField.BorderBrush = Brushes.DarkOliveGreen;
            }
            if (!isPasswordValid(passwordField.Password, repeatPassportField.Password))
            {
                allFieldsAreValid = false;
                passwordField.BorderBrush = Brushes.Red;
                repeatPassportField.BorderBrush = Brushes.Red;
            }
            else
            {
                passwordField.BorderBrush = Brushes.DarkOliveGreen;
                repeatPassportField.BorderBrush = Brushes.DarkOliveGreen;
            }
            if (!isTelephoneValid(phoneField))
            {
                phoneField.BorderBrush = Brushes.Red;
                allFieldsAreValid = false;
            }
            else
            {
                phoneField.BorderBrush = Brushes.DarkOliveGreen;
            }
            if (!checkEmpty(nameField))
            {
                allFieldsAreValid = false;
                nameField.BorderBrush = Brushes.Red;
            }
            else
            {
                nameField.BorderBrush = Brushes.DarkOliveGreen;
            }
            if (!checkEmpty(surnameField))
            {
                allFieldsAreValid = false;
                surnameField.BorderBrush = Brushes.Red;
            }
            else
            {
                surnameField.BorderBrush = Brushes.DarkOliveGreen;
            }
         
            if (allFieldsAreValid)
            {
                User newUser = new User(emailField.Text, passwordField.Password, nameField.Text, surnameField.Text, new List<Ticket>());
                JsonService JsonServiceUsers = new JsonService("Users.json");
                JsonServiceUsers.SetNewUser(newUser);
                var mainWindow = App.Current.MainWindow as MainWindow;
                mainWindow.registrationPage.Visibility = Visibility.Hidden;
                mainWindow.filmsGrid.Visibility = Visibility.Visible;
                mainWindow.SearchPage.Visibility = Visibility.Hidden;
                Login LoginForm = new Login();
                LoginForm.Show();
            }
        }
        

    }
}
