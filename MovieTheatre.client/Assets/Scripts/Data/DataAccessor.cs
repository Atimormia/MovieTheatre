using System;
using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class DataAccessor
    {
        private static readonly Lazy<DataAccessor> Lazy =
            new Lazy<DataAccessor>(() => new DataAccessor());

        public static DataAccessor Instance => Lazy.Value;

        private DataAccessor()
        {
        }

        public IEnumerable<EventData> GetAllMovies()
        {
            //test
            return new[]
            {
                new EventData {MovieName = "Interstellar", DateTime = DateTime.Today},
                new EventData {MovieName = "Thor", DateTime = DateTime.Today}
            };
        }

        public IEnumerable<SeatData> GetAllSeatsForMovie(EventData movie)
        {
            return new[]
            {
                new SeatData{EventData = movie, SeatNumber = 101, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 102, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 103, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 104, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 105, Reserved = false, Cost = 200f}, 
                //new SeatData{EventData = movie, SeatNumber = 106, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 107, Reserved = true, Cost = 100f, ClientName = "0"}, 
                new SeatData{EventData = movie, SeatNumber = 201, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 202, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 203, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 204, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 205, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 206, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 207, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 301, Reserved = true, Cost = 100f, ClientName = "1"}, 
                new SeatData{EventData = movie, SeatNumber = 302, Reserved = false, Cost = 100f},
                new SeatData{EventData = movie, SeatNumber = 303, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 304, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 305, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 306, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 307, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 401, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 402, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 403, Reserved = true, Cost = 100f, ClientName = "0"}, 
                new SeatData{EventData = movie, SeatNumber = 404, Reserved = false, Cost = 200f}, 
                new SeatData{EventData = movie, SeatNumber = 405, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 406, Reserved = false, Cost = 100f}, 
                new SeatData{EventData = movie, SeatNumber = 407, Reserved = false, Cost = 100f}, 
            };
        }

        public IEnumerable<SeatData> GetReservationsForClient(string client)
        {
            var movies = GetAllMovies().ToArray();
            return new[]
            {
                new SeatData{EventData = movies.First(), SeatNumber = 107, Reserved = true, Cost = 100f, ClientName = "0"}, 
                new SeatData{EventData = movies.First(), SeatNumber = 301, Reserved = true, Cost = 100f, ClientName = "1"}, 
                new SeatData{EventData = movies.Last(), SeatNumber = 403, Reserved = true, Cost = 100f, ClientName = "0"}, 
            };
        }

        public void SaveReservation(IEnumerable<SeatData> seats)
        {
        }
    }
}