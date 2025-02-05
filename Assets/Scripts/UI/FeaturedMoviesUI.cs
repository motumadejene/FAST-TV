using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class MovieList
{
    public List<Movie> results;
}

public class FeaturedMoviesUI : MonoBehaviour
{
    private string apiKey;
    private string baseUrl = "https://api.themoviedb.org/3";
    private string imageBaseUrl = "https://image.tmdb.org/t/p/w500";

    public GameObject moviePrefab;
    public Transform trendingPanel, nowPlayingPanel, topRatedPanel, upcomingPanel;

    private int maxMoviesPerCategory = 9;

    public void LoadAPIKey()
    {
        if (PlayerPrefs.HasKey("TMDbAPIKey"))
        {
            apiKey = PlayerPrefs.GetString("TMDbAPIKey");
            FetchMovies();
        }
    }

    void FetchMovies()
    {
        StartCoroutine(GetMovies("/trending/movie/week", trendingPanel));
        StartCoroutine(GetMovies("/movie/now_playing", nowPlayingPanel));
        StartCoroutine(GetMovies("/movie/top_rated", topRatedPanel));
        StartCoroutine(GetMovies("/movie/upcoming", upcomingPanel));
    }

    IEnumerator GetMovies(string endpoint, Transform panel)
    {
        string url = $"{baseUrl}{endpoint}?api_key={apiKey}&language=en-US&page=1";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            MovieList movieList = JsonUtility.FromJson<MovieList>(request.downloadHandler.text);
            int count = Mathf.Min(movieList.results.Count, maxMoviesPerCategory);

            for (int i = 0; i < count; i++)
            {
                Movie movie = movieList.results[i];
                if (!string.IsNullOrEmpty(movieList.results[i].poster_path))
                {
                    StartCoroutine(LoadMoviePoster(imageBaseUrl + movieList.results[i].poster_path, panel, movie));
                }
            }
        }
        else
        {
            Debug.LogError("Failed to fetch movies: " + request.error);
        }
    }

    IEnumerator LoadMoviePoster(string url, Transform parent, Movie movie)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

            GameObject movieItem = Instantiate(moviePrefab, parent);
            Image movieImage = movieItem.GetComponent<Image>();
          
            FeaturedMovie featuredMovie = movieItem.GetComponent<FeaturedMovie>();
            if (featuredMovie != null)
            {
                featuredMovie.SetMovie(movie);
            }

            if (movieImage != null)
            {
                movieImage.sprite = sprite;
            }
            else
            {
                Debug.LogError("Movie prefab is missing an Image component!");
            }
        }
        else
        {
            Debug.LogError("Failed to load image: " + request.error);
        }
    }
}