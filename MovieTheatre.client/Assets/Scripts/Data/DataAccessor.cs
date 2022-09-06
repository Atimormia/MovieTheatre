using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Global;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Util.Store;
using UnityEngine;

namespace Data
{
    public class DataAccessor
    {
        private const string FILE_ID = "1BQDcoWa32zPNk-UhabLPRHtaQif1VKnPoObl6b93CH8";
        private const string APP_NAME = "Movie Theatre";
        private const string EVENTS_RANGE = "Events!A2:C";
        private const string SEATS_RANGE = "Seats!A2:E";
        
        private static readonly Lazy<DataAccessor> Lazy =
            new Lazy<DataAccessor>(() => new DataAccessor());
        private static readonly string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };

        private List<SeatData> _seats = new List<SeatData>();
        private Dictionary<int, EventData> _events = new Dictionary<int, EventData>();
        private SheetsService _service;
        
        public static DataAccessor Instance => Lazy.Value;

        private DataAccessor()
        {
            try
            {
                UserCredential credential;
                using (var stream = new FileStream("Assets/credentials.json", FileMode.Open, FileAccess.Read))
                {
                    const string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.FromStream(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                    Debug.Log("Credential file saved to: " + credPath);
                }

                _service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = APP_NAME
                });

                LoadEvents();
                LoadSeats();
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError(e.Message);
            }
        }

        private void LoadEvents()
        {
            _events.Clear();
            foreach (var raw in GetValues(EVENTS_RANGE))
            {
                _events.Add(int.Parse((string) raw[0]), new EventData
                {
                    Id = int.Parse((string) raw[0]),
                    MovieName = (string) raw[1],
                    DateTime = DateTime.Parse((string) raw[2])
                });
            }
        }
        
        private void LoadSeats()
        {
            _seats.Clear();
            foreach (var raw in GetValues(SEATS_RANGE))
            {
                var eventId = int.Parse((string) raw[1]);
                _seats.Add(new SeatData
                {
                    SeatNumber = int.Parse((string) raw[0]),
                    EventData = _events.ContainsKey(eventId) ? _events[eventId] : null,
                    Reserved = bool.Parse((string) raw[2]),
                    Cost = float.Parse((string) raw[3]),
                    ClientName = raw.Count > 4 ? (string) raw[4] : null,
                });
            }
        }

        private IList<IList<object>> GetValues(string range)
        {
            var request = _service.Spreadsheets.Values.Get(FILE_ID, range);
            var response = request.Execute();
            var values = response.Values;
            if (values != null && values.Count != 0) 
                return values;
            
            Debug.LogWarning("No data found.");
            return null;
        }

        public IEnumerable<EventData> GetAllMovies()
        {
            return _events.Values;
        }

        public IEnumerable<SeatData> GetAllSeatsForMovie(EventData movie)
        {
            return _seats.Where(x => x.EventData.Equals(movie));
        }

        public IEnumerable<SeatData> GetReservationsForClient(string client)
        {
            return _seats.Where(x => x.ClientName == client);
        }

        public void SaveReservation(List<SeatData> seats)
        {
            foreach (var seat in _seats.Where(seats.Contains))
            {
                seat.Reserved = true;
                seat.ClientName = StateController.Instance.CurrentClientName;
            }
            UploadSeats();
        }

        private void UploadSeats()
        {
            try
            {
                var body = new ValueRange {Range = SEATS_RANGE, Values = SeatsToValues()};
                var result = _service.Spreadsheets.Values.Update(body, FILE_ID, SEATS_RANGE).Execute();
                Debug.Log($"Cells updated: {result.UpdatedCells}");
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        
        private IList<IList<object>> SeatsToValues()
        {
            return _seats.Select(seat => new List<object>
                {
                    seat.SeatNumber.ToString(),
                    seat.EventData.Id.ToString(),
                    seat.Reserved.ToString(),
                    seat.Cost.ToString(CultureInfo.InvariantCulture),
                    seat.ClientName
                })
                .Cast<IList<object>>()
                .ToList();
        }
    }
}