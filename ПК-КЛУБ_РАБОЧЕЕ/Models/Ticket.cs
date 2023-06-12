using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int OccupancyId { get; set; }
        public int PlaceNumber { get; set; }
        public double TicketPrice { get; set; }

        public Ticket(int id, int occupancyId, int placeNumber, double ticketPrice)
        {
            Id = id;
            OccupancyId = occupancyId;
            PlaceNumber = placeNumber;
            TicketPrice = ticketPrice;
        }


    }
}
