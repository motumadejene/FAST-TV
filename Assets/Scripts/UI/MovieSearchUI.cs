using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class MovieSearchUI : MonoBehaviour
{
    public TMDbAPIManager apiManager;
    public TMP_InputField searchInput;
    public Button searchButton;
    public Transform resultContainer;
    public GameObject moviePrefab;

    public GameObject featuredMoviePanel; // Panel for featured movies
    public GameObject searchResultPanel;  // Panel for search results

    private void Start()
    {
        searchButton.onClick.AddListener(() => SearchMovie());
        ShowFeaturedMovies(); // Show featured movies by default
    }

    public void SearchMovie()
    {
        string query = searchInput.text.Trim();
        if (!string.IsNullOrEmpty(query))
        {
            StartCoroutine(apiManager.SearchMovies(query, OnSuccess, OnError));
            ShowSearchResults(); // Switch to the search results panel
        }
    }

    private void OnSuccess(string response)
    {
        ClearResults();
        MovieSearchResult searchResult = JsonUtility.FromJson<MovieSearchResult>(response);

        if (searchResult == null || searchResult.results == null || searchResult.results.Length == 0)
        {
            Debug.LogWarning("No search results found.");
            return;
        }

        int maxResults = Mathf.Min(7, searchResult.results.Length); // Show only up to 7 results

        for (int i = 0; i < maxResults; i++)
        {
            GameObject movieGO = Instantiate(moviePrefab, resultContainer);
            MovieUI movieUI = movieGO.GetComponent<MovieUI>();

            if (movieUI != null)
            {
                movieUI.SetMovie(searchResult.results[i]);
            }
            else
            {
                Debug.LogError("MoviePrefab is missing the MovieUI script!");
            }
        }

    }

    private void OnError(string error)
    {
        Debug.LogError("API Call Failed: " + error);
    }

    private void ClearResults()
    {
        foreach (Transform child in resultContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void ShowFeaturedMovies()
    {
        featuredMoviePanel.SetActive(true);
        searchResultPanel.SetActive(false);
    }

    private void ShowSearchResults()
    {
        featuredMoviePanel.SetActive(false);
        searchResultPanel.SetActive(true);
    }
}
