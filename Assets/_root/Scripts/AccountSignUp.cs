using PlayFab;
using UnityEngine;
using TMPro;
using PlayFab.ClientModels;

public sealed class AccountSignUp : Account
{
    [SerializeField] private TMP_InputField _inputMail;

    private string _mail;

    public override void Start()
    {
        base.Start();
        _inputMail.onValueChanged.AddListener(UpdateEmail);
    }

    private void UpdateEmail(string mail) => _mail = mail;

    public override void Show()
    {
        base.Show();
        _inputMail.text = string.Empty;
    }

    protected override void ProceedForward() => CreateAccount();

    private void CreateAccount()
    {
        _loadingPanel.StartLoading();
        var request = new RegisterPlayFabUserRequest
        {
            Username = _username,
            Email = _mail,
            Password = _pass,
            RequireBothUsernameAndEmail = true
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegistrationSuccess, OnRegistrationFailure);
    }

    private void OnRegistrationSuccess(RegisterPlayFabUserResult result)
    {
        _loadingPanel.FinishLoading();
        string textResult = $"Success: {_username}";
        ShowResult(textResult, Color.green);
        PlayerPrefs.SetString(AuthUsernameKey, _username);
    }

    private void OnRegistrationFailure(PlayFabError error)
    {
        _loadingPanel.FinishLoading();
        var errorMessage = error.GenerateErrorReport();
        string textResult = $"Fail: {errorMessage}";
        ShowResult(textResult, Color.red);
    }
}
