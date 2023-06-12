using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Kurs.Models;
using File = System.IO.File;

namespace Kurs.Infrastructure.JSON
{
    // CRUD
    internal class JsonService
    {
        string _path;
        public JsonService() { }

        public JsonService(string path)
        {
            _path = path;
        }


        public List<Film> ReadAllFilms()
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Film>>(fileText);
            }
        }

        public List<User> ReadAllUsers()
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<User>>(fileText);
            }
        }
        public void SetNewUser(User user)
        {

            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<User> users =  JsonConvert.DeserializeObject<List<User>>(fileText);
                int newId = users.Count + 1;
                user.Id = newId;
                users.Add(user);
                fileText = JsonConvert.SerializeObject(users, Formatting.Indented);
            }
            File.WriteAllText(_path, fileText);
        }

        public Film GetFilm(string filmName)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Film>>(fileText)
            .FirstOrDefault(x => x.filmName.IndexOf(filmName, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        public List<Film> GetFilmsBySorting(string searchFilmName, string searchJanr, string searchDate, string searchRaiting, string searchTicketPrice)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                var films = JsonConvert.DeserializeObject<List<Film>>(fileText);

                // получаем список занятости зала на заданную дату
                var occupancyOnDate = GetOccupanciesByDate(searchDate,"occupancy.json");

                // присоединяем информацию о занятости к каждому фильму
                var filmsWithOccupancy = from film in films
                                         join occ in occupancyOnDate
                                         on film.Id equals occ.FilmId into gj
                                         from subOcc in gj.DefaultIfEmpty()
                                         select new { Film = film, Occupancy = subOcc };

                // применяем фильтры к списку фильмов
                if (!string.IsNullOrEmpty(searchFilmName))
                {
                    filmsWithOccupancy = filmsWithOccupancy.Where(x => x.Film.filmName.IndexOf(searchFilmName, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                if (!string.IsNullOrEmpty(searchJanr))
                {
                    filmsWithOccupancy = filmsWithOccupancy.Where(x => x.Film.janr.IndexOf(searchJanr, StringComparison.OrdinalIgnoreCase) >= 0);
                }
                if (!string.IsNullOrEmpty(searchDate))
                {
                    filmsWithOccupancy = filmsWithOccupancy.Where(x => x.Occupancy != null);
                }
                if (!string.IsNullOrEmpty(searchRaiting))
                {
                    
                    filmsWithOccupancy = filmsWithOccupancy.Where(x => x.Film.rating >= Convert.ToDouble(searchRaiting) && x.Film.rating < Math.Round(Convert.ToDouble(searchRaiting))+1);
                }
                if (!string.IsNullOrEmpty(searchTicketPrice))
                {
                    double price;
                    if (double.TryParse(searchTicketPrice, out price))
                    {
                        filmsWithOccupancy = filmsWithOccupancy.Where(x => x.Film.TicketPrice <= price);
                    }
                }

                // извлекаем список фильмов из результата объединения
                var sortedFilms = filmsWithOccupancy.Select(x => x.Film).ToList();

                return sortedFilms;
            }
        }

        public void DeleteUser(User user)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(fileText);
                User userToRemove = users.Find(f => f.Id == user.Id); // Или другое условие сравнения
                users.Remove(userToRemove);
                for (int i = 0; i < users.Count; i++)
                {
                    users[i].Id = i + 1;
                }
                fileText = JsonConvert.SerializeObject(users, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }
        public void SetFilm(Film film)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Film> films = JsonConvert.DeserializeObject<List<Film>>(fileText);
                int newId = films.Count + 1;
                film.Id = newId;
                films.Add(film);

                fileText = JsonConvert.SerializeObject(films, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }


        public bool SetOccupancy(Occupancy occupancy)
        {
            string fileText = "";

            // Чтение существующих данных из файла
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
            }

            // Десериализация данных из файла
            List<Occupancy> occupancies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);

            // Проверка на конфликты существующих занятостей
            bool isConflict = occupancies.Any(existingOccupancy =>
                existingOccupancy.HallId == occupancy.HallId &&
                existingOccupancy.OccupancyDateFrom == occupancy.OccupancyDateFrom &&
                (
                    (DateTime.Parse(existingOccupancy.OccupancyTimeFrom) >= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeFrom) <= DateTime.Parse(occupancy.OccupancyTimeTo)) ||
                    (DateTime.Parse(existingOccupancy.OccupancyTimeTo) >= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeTo) <= DateTime.Parse(occupancy.OccupancyTimeTo)) ||
                    (DateTime.Parse(existingOccupancy.OccupancyTimeFrom) <= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeTo) >= DateTime.Parse(occupancy.OccupancyTimeTo))
                )
            );

            if (isConflict)
            {
                return false;
            }
            else
            {
                // Генерация нового ID
                int newId = occupancies.Count + 1;
                occupancy.Id = newId;

                // Добавление новой занятости
                occupancies.Add(occupancy);

                // Сериализация данных обратно в JSON
                fileText = JsonConvert.SerializeObject(occupancies, Formatting.Indented);

                // Запись данных в файл
                File.WriteAllText(_path, fileText);
            }
            return true;
        }



        public void UpadteFilm(Film film)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Film> films = JsonConvert.DeserializeObject<List<Film>>(fileText);
                Film filmToUpdate = films.Find(f => f.Id == film.Id); // Или другое условие сравнения
                // Найти индекс фильма для обновления
                int index = films.FindIndex(f => f.Id == filmToUpdate.Id);




                // Заменить фильм на новый
                films[index] = film;

                fileText = JsonConvert.SerializeObject(films, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }


        public bool UpadteOccupancy(Occupancy occupancy)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Occupancy> occupancies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);
                Occupancy OccupancyToUpdate = occupancies.Find(f => f.Id == occupancy.Id); // Или другое условие сравнения
                // Найти индекс фильма для обновления
                int index = occupancies.FindIndex(f => f.Id == OccupancyToUpdate.Id);


                // Проверка на конфликты существующих занятостей
                bool isConflict = occupancies.Any(existingOccupancy =>
                    existingOccupancy.HallId == occupancy.HallId &&
                    existingOccupancy.OccupancyDateFrom == occupancy.OccupancyDateFrom &&
                    (
                        (DateTime.Parse(existingOccupancy.OccupancyTimeFrom) >= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeFrom) <= DateTime.Parse(occupancy.OccupancyTimeTo)) ||
                        (DateTime.Parse(existingOccupancy.OccupancyTimeTo) >= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeTo) <= DateTime.Parse(occupancy.OccupancyTimeTo)) ||
                        (DateTime.Parse(existingOccupancy.OccupancyTimeFrom) <= DateTime.Parse(occupancy.OccupancyTimeFrom) && DateTime.Parse(existingOccupancy.OccupancyTimeTo) >= DateTime.Parse(occupancy.OccupancyTimeTo))
                    )
                );

                if (isConflict)
                {
                    return false;
                }
                else
                {
                    // Заменить фильм на новый
                    occupancies[index] = occupancy;
                }

                fileText = JsonConvert.SerializeObject(occupancies, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
            return true;
        }
        public void DeleteFilm(Film film)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Film> films = JsonConvert.DeserializeObject<List<Film>>(fileText);
                Film filmToRemove = films.Find(f => f.Id == film.Id); // Или другое условие сравнения
                films.Remove(filmToRemove);
                for (int i = 0; i < films.Count; i++)
                {
                    films[i].Id = i + 1;
                }
                fileText = JsonConvert.SerializeObject(films, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }


        public void DeleteOccupancy(Occupancy occupancy)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Occupancy> occupancies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);
                Occupancy OccupancyToRemove = occupancies.Find(f => f.Id == occupancy.Id); // Или другое условие сравнения
                occupancies.Remove(OccupancyToRemove);
                for (int i = 0; i < occupancies.Count; i++)
                {
                    occupancies[i].Id = i + 1;
                }
                fileText = JsonConvert.SerializeObject(occupancies, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }

        

        public Film GetFilm(int id)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Film>>(fileText)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
        public Occupancy GetOccupancy(int id)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                Occupancy a =JsonConvert.DeserializeObject<List<Occupancy>>(fileText)
                    .FirstOrDefault(x => x.Id == id);
                return a;
            }
        }
        public List<Occupancy> GetOccupanciesByDate(string date, string occup_path)
        {
            using (var reader = File.OpenText(occup_path))
            {
                var fileText = reader.ReadToEnd();
                var occupancies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);
                return occupancies.Where(x => x.OccupancyDateFrom == date).ToList();
            }
        }
        public List<Occupancy> GetOccupancies()
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<Occupancy>>(fileText);
                
            }
        }


        public List<Occupancy> GetOccupancyByFilmId(int film_id)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                var occupancies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);
                return occupancies.Where(x => x.FilmId == film_id).ToList();
            }
        }

        public List<Ticket> GetUserTickets(int id)
        {
            using (var reader = File.OpenText(_path))
            {
                var json = reader.ReadToEnd();
                var jArray = JArray.Parse(json);
                var userToken = jArray.FirstOrDefault(u => (int)u["Id"] == id);
                if (userToken == null)
                {
                    return null;
                }

                var ticketsToken = userToken.SelectToken("Tickets");
                var tickets = ticketsToken.ToObject<List<Ticket>>();
                return tickets;
            }
        }

        public void SetUserTicket(List<Ticket> tickets, int userId)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(fileText);

                User user = users.FirstOrDefault(u => u.Id == userId);
                user.Tickets.AddRange(tickets);
                fileText = JsonConvert.SerializeObject(users, Formatting.Indented);
                
            }
            File.WriteAllText(_path, fileText);
        }
        public void SetOccupancy(List<int> selectedSeats, int occupancyId)
        {
            string fileText = "";
            using (var reader = File.OpenText(_path))
            {
                fileText = reader.ReadToEnd();
                List<Occupancy> occupansies = JsonConvert.DeserializeObject<List<Occupancy>>(fileText);

                Occupancy occupancy = occupansies.FirstOrDefault(u => u.Id == occupancyId);

                foreach (var seat in selectedSeats)
                {
                        occupancy.PlacesOccupancy[Convert.ToString(seat)] = true;
               
                }

                fileText = JsonConvert.SerializeObject(occupansies, Formatting.Indented);

            }
            File.WriteAllText(_path, fileText);
        }

        public User GetUser(int id)
        {
            using (var reader = File.OpenText(_path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<List<User>>(fileText)
                    .FirstOrDefault(x => x.Id == id);
            }
        }
    }
}
