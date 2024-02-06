using PlayFab.ClientModels;
using PlayFab;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayFabAccountManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleLabel;

    private void Start()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnFailure);
    }

    private void OnGetAccountSuccess(GetAccountInfoResult result)
    {
        _titleLabel.text = $"Welcome back, Player ID {result.AccountInfo.Username}";
    }

    private void OnFailure(PlayFabError error)
    {
        var errorMessage = error.GenerateErrorReport();
        Debug.LogError($"Something went wrong: {errorMessage}");
    }
}
