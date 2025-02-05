using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class FeaturedMovie : MonoBehaviour
{
    private Movie currentMovie;
    private Button movieButton;

    private void Awake()
    {
        movieButton = gameObject.GetComponent<Button>();
        if (movieButton != null)
        {
            movieButton.onClick.AddListener(OnMovieClicked);
        }
        else
        {
            Debug.LogError("Movie Button is missing in the Inspector!");
        }
    }
    public Movie GetMovieData()
    {
        return currentMovie;
    }

    public void SetMovie(Movie movie)
    {
        currentMovie = movie;      
    }

    public void OnMovieClicked()
    {
        if (currentMovie != null)
        {
            MovieDetailsUI.Instance.ShowMovieDetails(currentMovie);
        }
    }
}
