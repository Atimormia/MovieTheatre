using Data;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Utils;

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
        _clientUpdate.onClick.AddListener(ClientUpdateHandle);
        
        FillReservations();
    }

    private void ClientUpdateHandle()
    {
        StateController.Instance.CurrentClientName = _clientInput.text;
        FillReservations();
    }

    private void FillReservations()
    {
        var reservations = DataAccessor.Instance.GetReservationsForClient(StateController.Instance.CurrentClientName);
        int i = 0;
        foreach (var reservation in reservations)
        {
            var text = _textsPool.GetFromPool(i);
            i++;

            text.text =
                $"Movie: {reservation.EventData.MovieName}, DateTime: {reservation.EventData.DateTime}, Seat: {reservation.SeatNumber}";
        }
    }
}
