using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GenreManager : MonoBehaviour
{
    private const string genreListEndpoint = "https://api.themoviedb.org/3/genre/movie/list";
    public Dictionary<int, string> genreMapping = new Dictionary<int, string>();

    public IEnumerator FetchGenres(string apiKey)
    {
        string url = $"{genreListEndpoint}?api_key={apiKey}&language=en-US";

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            GenreList genreList = JsonUtility.FromJson<GenreList>(request.downloadHandler.text);
            foreach (var genre in genreList.genres)
            {
                genreMapping[genre.id] = genre.name;
            }
            Debug.Log("Genres successfully loaded.");
        }
        else
        {
            Debug.LogError("Failed to fetch genres: " + request.error);
        }
    }

    public string GetGenreName(int id)
    {
        return genreMapping.ContainsKey(id) ? genreMapping[id] : "Unknown";
    }
}
