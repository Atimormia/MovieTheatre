using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class SeatButton : MonoBehaviour
{
    [SerializeField] private Color _selected;

    public event Action OnSelected;
    public event Action OnDeselected;

    private Button _button;
    private Button Button{
        get
        {
            if (_button == null)
                _button = GetComponent<Button>();
            return _button;
        }
    }
    
    private Image _image;
    private Image Image{
        get
        {
            if (_image == null)
                _image = GetComponent<Image>();
            return _image;
        }
    }
    private Text _text;
    private Text Text{
        get
        {
            if (_text == null)
                _text = GetComponentInChildren<Text>();
            return _text;
        }
    }
    public void SetAsReserved()
    {
        Button.interactable = false;
    }

    public void SetAsAvailable()
    {
        Button.interactable = true;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(SetAsSelected);
        Image.color = Color.white;
        OnDeselected?.Invoke();
    }

    private void SetAsSelected()
    {
        Button.interactable = true;
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(SetAsAvailable);
        Image.color = _selected;
        
        OnSelected?.Invoke();
    }

    public void SetCost(float cost)
    {
        Text.text = cost.ToString(CultureInfo.InvariantCulture);
    }

}
