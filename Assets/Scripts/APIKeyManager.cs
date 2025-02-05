using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Collections;

public class APIKeyManager : MonoBehaviour
{
    private const string APIKeyPref = "TMDbAPIKey";
    public TMP_InputField apiKeyInput;
    public GameObject apiKeyPanel;
    public GameObject searchPanel;
    public Button saveApiKeyButton;
    public FeaturedMoviesUI featuredMoviesUI;
    public GenreManager genreManager;

    public GameObject errorMessageUI; // Reference to the error message panel
    public TMP_Text errorMessageText; // Reference to the error message text

    private void Start()
    {
        ShowAPIKeyPrompt();
        saveApiKeyButton.onClick.AddListener(ValidateAndSaveAPIKey);
    }

    private void ShowAPIKeyPrompt()
    {
        apiKeyPanel.SetActive(true);
        searchPanel.SetActive(false);
    }

    private void ValidateAndSaveAPIKey()
    {
        string userApiKey = apiKeyInput.text.Trim();

        // Check if there's no internet connection
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowErrorMessage("No internet connection. Please check your network and try again.");
            return;
        }

        if (!string.IsNullOrEmpty(userApiKey))
        {
            StartCoroutine(ValidateAPIKey(userApiKey));
        }
        else
        {
            ShowErrorMessage("API Key cannot be empty!");
        }
    }

    private IEnumerator ValidateAPIKey(string apiKey)
    {
        string testUrl = $"https://api.themoviedb.org/3/configuration?api_key={apiKey}";

        using (UnityWebRequest request = UnityWebRequest.Get(testUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                PlayerPrefs.SetString(APIKeyPref, apiKey);
                PlayerPrefs.Save();
                ShowSearchPanel();
                featuredMoviesUI.LoadAPIKey();
                StartCoroutine(genreManager.FetchGenres(apiKey));
            }
            else
            {
                string error = "Invalid API Key";
                ShowErrorMessage(error);
            }
        }
    }

    private void ShowSearchPanel()
    {
        apiKeyPanel.SetActive(false);
        searchPanel.SetActive(true);
    }

    private void ShowErrorMessage(string message)
    {
        errorMessageUI.SetActive(true);
        errorMessageText.text = message;

        // Hide the error message after 3 seconds
        StartCoroutine(HideErrorMessageAfterTime(2f));
    }

    private IEnumerator HideErrorMessageAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        errorMessageUI.SetActive(false);
    }

    public static string GetAPIKey()
    {
        return PlayerPrefs.GetString(APIKeyPref, "");
    }
}
