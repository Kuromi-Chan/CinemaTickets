using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurs.Models;

namespace Kurs
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Ticket> Tickets { get; set; }

        [JsonConstructor]
        public User(int id,string email, string password ,string name, string surname, List<Ticket> tickets)
        {
            Id = id;
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Tickets = tickets;
        }
        public User(int id, string email, string name, string surname)
        {
            Id = id;
            Email = email;
            Name = name;
            Surname = surname;
        }

        public User(string email, string password, string name, string surname, List<Ticket> tickets)
        {
            Email = email;
            Password = password;
            Name = name;
            Surname = surname;
            Tickets = tickets;
        }
    }

}
