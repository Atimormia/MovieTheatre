using System.Linq;
using Data;
using UI.Buttons;
using UnityEngine;
using Utils;

namespace UI.Menus
{
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
            var movies = DataAccessor.Instance.GetAllMovies().ToArray();

            for (var i = 0; i < movies.Length; i++)
            {
                var movie = movies[i];
                var button = _buttonsPool.GetFromPool(i);

                button.SetText(movie.MovieName, movie.DateTime);
                button.SetListener(() =>
                {
                    StateController.Instance.CurrentEvent = movie;
                    gameObject.SetActive(false);
                    _seatsMenu.SetActive(true);
                });
            }
        }
    }
}
