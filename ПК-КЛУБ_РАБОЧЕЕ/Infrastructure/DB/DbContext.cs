using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Kurs.Infrastructure.JSON;

namespace Kurs.Infrastructure
{
    public  class DbContext
    {
        public string connectionString = @"Data Source=DESKTOP-VA8A5B5\SQLEXPRESS;Initial Catalog=Курсовая;Integrated Security=True";
        

        public DbContext() { }

        public void InsertFilms()
        {
            string _path = "films.json";
            JsonService JsonServiceFilms = new JsonService(_path);
            string occupPath = "occupancy.json";
            JsonService JsonServiceOccup = new JsonService(occupPath);
            var occupancies = JsonServiceOccup.GetOccupancies();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sqlDeleteQuery = "DELETE FROM Продажи";

                    using (SqlCommand deleteCommand = new SqlCommand(sqlDeleteQuery, connection))
                    {
                        deleteCommand.ExecuteNonQuery();
                    }



                    string sqlQuery = "INSERT INTO Продажи (Название_фильма, День_проката,Время_сеанса, Продано_билетов) VALUES (@Название_фильма, @День_проката,@Время_сеанса, @Продано_билетов)";

                    foreach (var occupancy in occupancies)
                    {
                        string filmName = JsonServiceFilms.GetFilm(occupancy.FilmId).filmName;
                        string OccupancyDate = occupancy.OccupancyDateFrom;
                        string OccpancyTime = occupancy.OccupancyTimeFrom;
                        int ticketsCount = occupancy.PlacesOccupancy.Count(x => x.Value);

                        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Название_фильма", filmName);
                            command.Parameters.AddWithValue("@День_проката", OccupancyDate);
                            command.Parameters.AddWithValue("@Время_сеанса", OccpancyTime);
                            command.Parameters.AddWithValue("@Продано_билетов", ticketsCount);

                            command.ExecuteNonQuery();
                        }
                    }

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            }
        }



    }
}
