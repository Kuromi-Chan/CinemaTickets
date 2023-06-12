using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Models
{
    public class Occupancy
    {
        public int Id { get; set; }
        public int HallId { get; set; }
        public int FilmId { get; set; }
        public string OccupancyDateFrom { get; set; }
        public string OccupancyDateTo { get; set; }

        public string OccupancyTimeFrom { get; set; }
        public string OccupancyTimeTo { get; set; }

        public Dictionary<string,bool> PlacesOccupancy { get; set; }


        [JsonConstructor]
        public Occupancy(int id, int hallId, int filmId, string occupancyDateFrom, string occupancyDateTo, string occupancyTimeFrom, string occupancyTimeTo, Dictionary<string, bool> placesOccupancy)
        {
            Id = id;
            HallId = hallId;
            FilmId = filmId;
            OccupancyDateFrom = occupancyDateFrom;
            OccupancyDateTo = occupancyDateTo;
            OccupancyTimeFrom = occupancyTimeFrom;
            OccupancyTimeTo = occupancyTimeTo;
            PlacesOccupancy = placesOccupancy;
        }

        public Occupancy(int hallId, int filmId, string occupancyDateFrom, string occupancyDateTo, string occupancyTimeFrom, string occupancyTimeTo, Dictionary<string, bool> placesOccupancy)
        {
            HallId = hallId;
            FilmId = filmId;
            OccupancyDateFrom = occupancyDateFrom;
            OccupancyDateTo = occupancyDateTo;
            OccupancyTimeFrom = occupancyTimeFrom;
            OccupancyTimeTo = occupancyTimeTo;
            PlacesOccupancy = placesOccupancy;
        }
    }
}
