using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Kurs.Domain.Application
{
    internal class UserApplication
    {



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
            Regex telephonePattern = new Regex(@"\+375 ?\((25|33|44|29|24)\) ?\d{3}\-\d{2}\-\d{2}");
            var matchTelephone = telephonePattern.Match(Convert.ToString(telephone));
            if (matchTelephone.Success)
                return true;
            else
                return false;
        }
        private bool isPasswordValid(string pass1, string pass2)
        {
            if (!pass1.Equals(pass2) || pass1 == "")
                return false;
            else
                return true;
        }

        private bool checkEmpty(TextBox textBox)
        {

            if (textBox.Text == "")
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
    }
}
