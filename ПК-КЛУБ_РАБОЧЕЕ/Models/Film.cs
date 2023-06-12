using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs
{
    public class Film
    {

        
        public int Id { get; set; }
        public string previewPath { get; set; }
       
        public string filmName { get; set; }
        public string janr { get; set; }
        public double rating { get; set; }
        public string filmDo { get; set; }
        public string duration { get; set; }
        public double TicketPrice { get; set; }
        
        public Film(string previewPath, string filmName, string janr,
            double rating, string filmDo, double TicketPrice, string duration)
        {
            this.previewPath = Path.Combine(Directory.GetCurrentDirectory(), "previews", previewPath);
            this.filmName = filmName;
            this.janr = janr;
            this.rating = rating;
            this.filmDo = filmDo;
            this.TicketPrice = TicketPrice;
            this.duration=duration;

        }

        [JsonConstructor]
        public Film(int id, string previewPath, string filmName, string janr, double rating, string filmDo, string duration, double ticketPrice)
        {
            Id = id;
            this.previewPath = Path.Combine(Directory.GetCurrentDirectory(), "previews", previewPath);
            this.filmName = filmName;
            this.janr = janr;
            this.rating = rating;
            this.filmDo = filmDo;
            this.duration = duration;
            TicketPrice = ticketPrice;
        }

        public Film(string filmName, string janr, double rating, string filmDo, double ticketPrice, string duration)
        {
            this.filmName = filmName;
            this.janr = janr;
            this.rating = rating;
            this.filmDo = filmDo;
            this.duration = duration;
            TicketPrice = ticketPrice;
        }
    }
}
