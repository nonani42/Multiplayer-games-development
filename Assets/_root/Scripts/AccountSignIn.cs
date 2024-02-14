using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public sealed class AccountSignIn : Account
{
    private const string AuthGuidKey = "authorization-guid";

    [SerializeField] private GameObject _signInPlayerHolder;
    [SerializeField] private Button _signInBtn;
    [SerializeField] private TextMeshProUGUI _signInBtnText;
    [SerializeField] private Button _forgetBtn;


    [SerializeField] private Toggle _newPlayerToggle;
    [SerializeField] private GameObject _newPlayerHolder;

    private string id;

    public event Action OnSuccess;

    public override void Show()
    {
        base.Show();
        Subscribe();
        _signInPlayerHolder.SetActive(true);
        _newPlayerHolder.SetActive(false);
        _newPlayerToggle.isOn = false;

        Debug.Log(PlayerPrefs.GetString(AuthGuidKey));

        _signInBtnText.text = !PlayerPrefs.HasKey(AuthGuidKey)? "New" : "Sign in";
        _forgetBtn.interactable = PlayerPrefs.HasKey(AuthGuidKey) || PlayerPrefs.HasKey(AuthUsernameKey);
        _inputUsername.text = PlayerPrefs.GetString(AuthUsernameKey);
    }

    public override void Hide()
    {
        base.Hide();
        Unsubscribe();
    }

    protected override void Subscribe()
    {
        base.Subscribe();
        _newPlayerToggle.onValueChanged.AddListener(AuthNewPlayer);
        _signInBtn.onClick.AddListener(LoginWithGuid);
        _forgetBtn.onClick.AddListener(ForgetUser);
    }

    protected override void Unsubscribe()
    {
        base.Unsubscribe();
        _newPlayerToggle.onValueChanged.RemoveListener(AuthNewPlayer);
        _signInBtn.onClick.RemoveListener(LoginWithGuid);
        _forgetBtn.onClick.RemoveListener(ForgetUser);
    }

    protected override void ProceedForward() => LoginWithUsername();

    private void LoginWithUsername()
    {
        var request = new LoginWithPlayFabRequest
        {
            Username = _username,
            Password = _pass,
        };
        PlayerPrefs.SetString(AuthUsernameKey, _username);
        if(_loadingPanel != null)
            _loadingPanel.StartLoading();
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        if (_loadingPanel != null)
            _loadingPanel.FinishLoading();
        string textResult = "Congratulations, you made successful API call!";
        ShowResult(textResult, Color.green);
        OnSuccess?.Invoke();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (_loadingPanel != null)
            _loadingPanel.FinishLoading();
        var errorMessage = error.GenerateErrorReport();
        string textResult = $"Something went wrong: {errorMessage}";
        ShowResult(textResult, Color.red);
    }

    private void LoginWithGuid()
    {
        bool needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        if (_loadingPanel != null)
            _loadingPanel.StartLoading();
        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = !needCreation
        }, OnGuidLoginSuccsess, OnLoginFailure);
    }

    private void OnGuidLoginSuccsess(LoginResult result)
    {
        PlayerPrefs.SetString(AuthGuidKey, id);
        if (_loadingPanel != null)
            _loadingPanel.FinishLoading();
        OnSuccess?.Invoke();
    }

    private void ForgetUser()
    {
        PlayerPrefs.DeleteKey(AuthUsernameKey);
        PlayerPrefs.DeleteKey(AuthGuidKey);
        _signInBtnText.text = "New";
        _inputUsername.text = string.Empty;
        _forgetBtn.interactable = false;
    }

    private void AuthNewPlayer(bool toggle)
    {
        _signInPlayerHolder.SetActive(!toggle);
        _newPlayerHolder.gameObject.SetActive(toggle);
    }
}
