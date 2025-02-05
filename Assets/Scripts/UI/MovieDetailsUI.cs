using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class MovieDetailsUI : MonoBehaviour
{
    public static MovieDetailsUI Instance; // Singleton

    [Header("UI Panels")]
    public GameObject detailedPanel;
    public GameObject featuredPanel;
    public GameObject[] panelsToHide;
    public GenreManager genreManager;

    [Header("Movie Details UI Elements")]
    public TMP_Text titleText;
    public TMP_Text releaseYearText;
    public TMP_Text overviewText;
    public TMP_Text ratingText;
    public TMP_Text genresText;
    public Image posterImage;
    public Image backdropImage;
    public Sprite noImage;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowMovieDetails(Movie movie)
    {
        if (movie == null)
        {
            Debug.LogError("No movie data provided!");
            return;
        }

        backdropImage.sprite = noImage;
        HideAllPanels();
        detailedPanel.SetActive(true);

        titleText.text = movie.title;
        releaseYearText.text = movie.release_date.Length >= 4 ? movie.release_date.Substring(0, 4) : "Unknown Year";
        overviewText.text = !string.IsNullOrEmpty(movie.overview) ? movie.overview : "No description available.";
        ratingText.text = $"Rating: {movie.vote_average}/10";

        if (!string.IsNullOrEmpty(movie.poster_path))
            StartCoroutine(LoadImage("https://image.tmdb.org/t/p/w500" + movie.poster_path, posterImage));
        else
            posterImage.sprite = noImage;

        if (!string.IsNullOrEmpty(movie.backdrop_path))
        {
            StartCoroutine(LoadImage("https://image.tmdb.org/t/p/w780" + movie.backdrop_path, backdropImage));
        }
        else
        {
            // Assign a default placeholder sprite
            backdropImage.sprite = noImage;
        }

        // Fetch genre names
        string genreNames = "";
        if (genreManager != null)
        {
            foreach (int genreId in movie.genre_ids)
            {
                genreNames += genreManager.GetGenreName(genreId) + ", ";
            }
            genreNames = genreNames.TrimEnd(',', ' ');
        }
        else
        {
            genreNames = "Unknown";
        }

        genresText.text = $"Genres: {genreNames}";
    }

    private IEnumerator LoadImage(string url, Image targetImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
        }
        else
        {
            // Assign a default placeholder sprite
            targetImage.sprite = Resources.Load<Sprite>("Images/Placeholder");
        }
    }

    private void HideAllPanels()
    {
        featuredPanel.SetActive(false);
        foreach (GameObject panel in panelsToHide)
            if (panel != null)
                panel.SetActive(false);
    }

    public void CloseMovieDetails()
    {
        foreach (GameObject panel in panelsToHide)
            if (panel != null)
                panel.SetActive(false);

        detailedPanel.SetActive(false);
        featuredPanel.SetActive(true);
    }
}
