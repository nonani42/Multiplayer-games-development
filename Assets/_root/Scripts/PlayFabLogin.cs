using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    [SerializeField] private Button loginBtn;
    [SerializeField] private TextMeshProUGUI loginResult;

    public void Start()
    {
        loginBtn.onClick.AddListener(Login);
    }

    private void Login()
    {
        // Here we need to check whether TitleId property is configured in settings or not
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            //If not we need to assign it to the appropriate variable manually. Otherwise we can just remove this if statement at all
            PlayFabSettings.staticSettings.TitleId = "A823B";
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = "GeekBrainsLesson3",
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string textResult = "Congratulations, you made successful API call!";
        Debug.Log(textResult);
        loginResult.color = Color.green;
        loginResult.text = textResult;
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        string textResult = $"Something went wrong: {errorMessage}";
        Debug.LogError(textResult);
        loginResult.color = Color.red;
        loginResult.text = textResult;
    }
}