using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Menus
{
    public class ReservationsMenu : MonoBehaviour
    {
        [SerializeField] private InputField _clientInput;
        [SerializeField] private Button _clientUpdate;
        [SerializeField] private Transform _reservationsParent;
        [SerializeField] private Text _reservationText;

        private ObjectsPool<Text> _textsPool = null;

        public void OnEnable()
        {
            if (_textsPool == null)
                _textsPool = new ObjectsPool<Text>(_reservationsParent, _reservationText);

            _clientInput.text = StateController.Instance.CurrentClientName;
            _clientUpdate.onClick.AddListener(ClientUpdateHandle);
        
            FillReservations();
        }

        private void ClientUpdateHandle()
        {
            StateController.Instance.CurrentClientName = _clientInput.text;
            _textsPool.Clear();
            FillReservations();
        }

        private void FillReservations()
        {
            var client = StateController.Instance.CurrentClientName;
            if (string.IsNullOrEmpty(client)) 
                return;
        
            var reservations = DataAccessor.Instance.GetReservationsForClient(client).ToArray();
            for (var i = 0; i < reservations.Length; i++)
            {
                var text = _textsPool.GetFromPool(i);

                text.text =
                    $"Movie: {reservations[i].EventData.MovieName}, DateTime: {reservations[i].EventData.DateTime}, Seat: {reservations[i].SeatNumber}";
            }
        }
    }
}
