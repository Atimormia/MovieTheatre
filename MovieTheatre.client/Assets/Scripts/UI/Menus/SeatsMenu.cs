using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Data;
using Global;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class SeatsMenu : MonoBehaviour
{
    [SerializeField]
    private SeatButton _seatButtonPrefab;
    [SerializeField]
    private Transform _buttonsGridParent;
    [SerializeField]
    private Transform _buttonsRawParent;

    [SerializeField] private Text _seatsText;
    [SerializeField] private Text _costText;

    [SerializeField] private Button _reserveButton;
    [SerializeField] private GameObject _reservationsMenu;

    private float _currentCost = 0;
    private readonly HashSet<SeatData> _selectedSeats = new HashSet<SeatData>();

    private ObjectsPool<SeatButton> _buttonsPool = null;
    private ObjectsPool<Transform> _rawParentsPool = null;

    private EventData _prevEvent;
    public void OnEnable()
    {
        if (_buttonsPool == null)
            _buttonsPool = new ObjectsPool<SeatButton>(null, _seatButtonPrefab);
        if (_rawParentsPool == null)
            _rawParentsPool = new ObjectsPool<Transform>(_buttonsGridParent, _buttonsRawParent);

        if (_prevEvent != null && !_prevEvent.Equals(StateController.Instance.CurrentEvent))
        {
            _buttonsPool.Clear();
            _rawParentsPool.Clear();
        }

        FillSeats();
        _reserveButton.onClick.AddListener(ReserveHandle);
        _selectedSeats.Clear();
        UpdateSeats();
        _currentCost = 0;
        UpdateCost();
    }

    private void ReserveHandle()
    {
        DataAccessor.Instance.SaveReservation(_selectedSeats.ToList());
        gameObject.SetActive(false);
        _reservationsMenu.SetActive(true);
    }

    public void OnDisable()
    {
        _reserveButton.onClick.RemoveListener(ReserveHandle);
    }

    private void FillSeats()
    {
        var seats = DataAccessor.Instance.GetAllSeatsForMovie(StateController.Instance.CurrentEvent)
            .OrderBy(x => x.SeatNumber);

        var i = 0;
        var prevRaw = 0;
        foreach (var seat in seats)
        {
            if (seat.SeatNumber / 100 != prevRaw)
            {
                prevRaw = seat.SeatNumber / 100;
                _buttonsPool.SetParent(_rawParentsPool.GetFromPool(prevRaw));
            }

            var button = _buttonsPool.GetFromPool(i, x =>
            {
                x.OnSelected += () => HandleSelection(seat);
                x.OnDeselected += () => HandleDeselection(seat);
            });

            i++;
            button.SetAsAvailable();
            if (seat.Reserved)
            {
                button.SetAsReserved();
                button.SetCost(string.Empty);
            }
            else
            {
                button.SetCost(seat.Cost.ToString(CultureInfo.InvariantCulture));
            }
        }
    }

    private void HandleSelection(SeatData seat)
    {
        _selectedSeats.Add(seat);
        UpdateSeats();
        
        _currentCost += seat.Cost;
        UpdateCost();
    }

    private void HandleDeselection(SeatData seat)
    {
        _selectedSeats.Remove(seat);
        UpdateSeats();
        
        _currentCost -= seat.Cost;
        UpdateCost();
    }

    private void UpdateCost()
    {
        _costText.text = $"Cost: {_currentCost.ToString(CultureInfo.InvariantCulture)}";
    }

    private void UpdateSeats()
    {
        _seatsText.text = $"Selected: {string.Join(", ", _selectedSeats.Select(x => x.SeatNumber))}";
    }
}
