using System.Collections.Generic;
using Data;
using Global;
using UnityEngine;
using Utils;

public class MoviesMenu : MonoBehaviour
{
    [SerializeField]
    private MovieButton _movieButtonPrefab;
    [SerializeField]
    private Transform _buttonsParent;
    [SerializeField] 
    private GameObject _seatsMenu;

    private ObjectsPool<MovieButton> _buttonsPool = null;

    public void OnEnable()
    {
        if (_buttonsPool == null)
            _buttonsPool = new ObjectsPool<MovieButton>(_buttonsParent, _movieButtonPrefab);
        FillButtons();
    }

    private void FillButtons()
    {
        var movies = DataAccessor.Instance.GetAllMovies();

        var i = 0;
        foreach (var movie in movies)
        {
            var button = _buttonsPool.GetFromPool(i);
            i++;

            button.SetText(movie.MovieName, movie.DateTime);
            button.SetListener(() =>
            {
                gameObject.SetActive(false);
                _seatsMenu.SetActive(true);
                StateController.Instance.CurrentEvent = movie;
            });
        }
    }
}
