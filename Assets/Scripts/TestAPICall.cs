using UnityEngine;

public class TestAPICall : MonoBehaviour
{
    public TMDbAPIManager apiManager;

    public void SearchForMovie(string query)
    {
        StartCoroutine(apiManager.SearchMovies(query, OnSuccess, OnError));
    }

    private void OnSuccess(string response)
    {
        Debug.Log("API Call Success: " + response);

        /*MovieSearchResult searchResult = JsonUtility.FromJson<MovieSearchResult>(response);
        foreach (var movie in searchResult.results)
        {
            Debug.Log($"Title: {movie.title}, Overview: {movie.overview}");
        }*/
    }

    private void OnError(string error)
    {
        Debug.LogError("API Call Failed: " + error);
    }
}
