using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Buttons
{
    public class MovieButton : MonoBehaviour
    {
        private Text _text;
        private Button _button;

        public void SetText(string movieName, DateTime dateTime)
        {
            if (_text == null)
                _text = GetComponentInChildren<Text>();
            _text.text = $"{movieName}, {dateTime}";
        }

        public void SetListener(UnityAction action)
        {
            if (_button == null)
                _button = GetComponent<Button>();

            _button.onClick.RemoveAllListeners();
            _button.onClick.AddListener(action);
        }
    }
}
