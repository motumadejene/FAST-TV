using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;
using System.Threading;
using System.Text.RegularExpressions;
using UnityEngine.Analytics;

public class MovieUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text releaseYearText;
    public TMP_Text genresText;
    public Image posterImage;
    private const string ImageBaseURL = "https://image.tmdb.org/t/p/w500";

    private Movie currentMovie;
    public Button movieButton;
    public Button movieImageButton;

    private void Awake()
    {
        if (movieButton != null)
        {
            movieButton.onClick.AddListener(OnMovieClicked);
            movieImageButton.onClick.AddListener(OnMovieClicked);
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
        if (titleText == null || releaseYearText == null || posterImage == null)
        {
            Debug.LogError("MovieUI: UI elements are not assigned in the Inspector!");
            return;
        }

        currentMovie = movie;
        titleText.text = movie.title;

        if (!string.IsNullOrEmpty(movie.release_date) && movie.release_date.Length >= 4)
        {
            releaseYearText.text = movie.release_date.Substring(0, 4);
        }
        else
        {
            releaseYearText.text = "Unknown Year";
        }

        if (!string.IsNullOrEmpty(movie.poster_path))
        {
            StartCoroutine(LoadImage(ImageBaseURL + movie.poster_path));
        }
        else
        {
            Debug.LogWarning($"Movie '{movie.title}' has no poster path. Using default image.");
            //posterImage.sprite = MovieDetailsUI.noImage;
            //posterImage.sprite = Resources.Load<Sprite>("Images/Placeholder");
        }
    }

    private IEnumerator LoadImage(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            posterImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            Debug.LogError("Failed to load image: " + request.error);
        }
    }

    public void OnMovieClicked()
    {
        if (currentMovie != null)
        {
            MovieDetailsUI.Instance.ShowMovieDetails(currentMovie);
        }
    }
}
