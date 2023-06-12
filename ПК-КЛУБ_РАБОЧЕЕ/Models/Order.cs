
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using Kurs.Models;

namespace Kurs
{
    public class Order
    {
        public int Id { get; set; }    
        public int UserId { get; set; }
        public List<int> ticketsId { get; set; }
        public string TotalCost { get; set; }

        public Order(int id, int userId, List<int> ticketsId, string totalCost)
        {
            Id = id;
            UserId = userId;
            this.ticketsId = ticketsId;
            TotalCost = totalCost;
        }


    }
}
