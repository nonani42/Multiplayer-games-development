using PlayFab;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Account : MonoBehaviour
{
    //login: gb3 pass: 123456
    protected const string AuthUsernameKey = "authorization-username";

    [SerializeField] protected Loading _loadingPanel;

    [SerializeField] private TextMeshProUGUI _resultText;

    [SerializeField] protected TMP_InputField _inputUsername;
    [SerializeField] protected TMP_InputField _inputPass;
    [SerializeField] private Button _okBtn;

    [SerializeField] private Button _backBtn;

    protected string _username;
    protected string _pass;

    public event Action OnBack;

    public virtual void Start()
    {
        CheckSettings();
    }

    private void UpdateUsername(string username) => _username = username;
    private void UpdatePassword(string pass) => _pass = pass;

    private void CheckSettings()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            PlayFabSettings.staticSettings.TitleId = "EFFF5";
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        _inputUsername.text = string.Empty;
        _inputPass.text = string.Empty;
        _resultText.gameObject.SetActive(false);
        Subscribe();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        Unsubscribe();
    }

    protected abstract void ProceedForward();

    protected virtual void Subscribe()
    {
        _inputUsername.onValueChanged.AddListener(UpdateUsername);
        _inputPass.onValueChanged.AddListener(UpdatePassword);
        _backBtn.onClick.AddListener(() => OnBack?.Invoke());
        _okBtn.onClick.AddListener(ProceedForward);
    }

    protected virtual void Unsubscribe()
    {
        _inputUsername.onValueChanged.RemoveListener(UpdateUsername);
        _inputPass.onValueChanged.RemoveListener(UpdatePassword);
        _backBtn.onClick.RemoveAllListeners();
        _okBtn.onClick.RemoveListener(ProceedForward);
    }

    protected void ShowResult(string msg, Color color = default)
    {
        Debug.Log(msg);
        _resultText.gameObject.SetActive(true);
        _resultText.color = color;
        _resultText.text = msg;
    }
}
