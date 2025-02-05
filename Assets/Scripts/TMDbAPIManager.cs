using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TMDbAPIManager : MonoBehaviour
{
    private const string BaseURL = "https://api.themoviedb.org/3/";
    private string apiKey;

    public IEnumerator SearchMovies(string query, System.Action<string> onSuccess, System.Action<string> onError)
    {
        apiKey = APIKeyManager.GetAPIKey();
        if (string.IsNullOrEmpty(apiKey))
        {
            Debug.LogError("API Key is missing!");
        }

        string url = $"{BaseURL}search/movie?api_key={apiKey}&query={UnityWebRequest.EscapeURL(query)}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                onSuccess?.Invoke(request.downloadHandler.text);
            }
            else
            {
                onError?.Invoke(request.error);
            }
        }
    }
}

[Serializable]
public class Movie
{
    public string title;
    public string poster_path;
    public string release_date;
    public string overview;   //  Add this field
    public string backdrop_path;  //  Optional: for a larger background image
    public float vote_average;   //  Optional: for rating display
    public List<int> genre_ids;
}

[System.Serializable]
public class Genre
{
    public int id;
    public string name;
}

[System.Serializable]
public class GenreList
{
    public List<Genre> genres;
}


[Serializable]
public class MovieSearchResult
{
    public int page;
    public Movie[] results;
}
