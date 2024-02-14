using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayFabSaveLoad
{
    private const char SEPARATOR = '|';
    private const string MAX_HEALTH = "maxHealth";
    private const string CURRENT_HEALTH = "currentHealth";

    private string _playerId;
    private TaskCompletionSource<GetUserDataResult> _completedTask;

    private GetUserDataResult _result;

    public PlayFabSaveLoad()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountInfoSuccess, OnFailure);
        _completedTask = new TaskCompletionSource<GetUserDataResult>();
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        _playerId = result.AccountInfo.PlayFabId;
        Debug.LogWarning($"_playerId {_playerId}");
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = _playerId }, OnSuccessfulGetUserData, OnFailure);
    }

    public void SaveGame(float maxHealth, float currentHealth)
    {
        UpdateUserDataRequest userDataRequest = new UpdateUserDataRequest {
            Data = new Dictionary<string, string>
            {
                { MAX_HEALTH, maxHealth.ToString() },
                { CURRENT_HEALTH, currentHealth.ToString() },
            } 
        };
        PlayFabClientAPI.UpdateUserData(userDataRequest, OnSuccessfulSave, OnFailure);
    }

    public async Task<Tuple<float, float>> LoadGame()
    {
        GetUserDataResult temp = await _completedTask.Task;
        float maxHealth = default;
        if (_result != null && _result.Data.ContainsKey(MAX_HEALTH))
        {
            _result.Data.TryGetValue(MAX_HEALTH, out UserDataRecord value);
            maxHealth = float.Parse(value.Value.ToString());
            Debug.LogWarning($"maxHealth {value.Value}");
        }

        float currentHealth = default;
        if (_result != null && _result.Data.ContainsKey(CURRENT_HEALTH))
        {
            _result.Data.TryGetValue(CURRENT_HEALTH, out UserDataRecord value);
            currentHealth = float.Parse(value.Value.ToString());
            Debug.LogWarning($"currentHealth {value.Value}");
        }
        return new Tuple<float, float>(maxHealth, currentHealth);
    }

    private void OnSuccessfulGetUserData(GetUserDataResult result)
    {
        Debug.LogWarning($"result {result == null}");

        _result = result;
        _completedTask.SetResult(_result);
    }

    private void OnSuccessfulSave(UpdateUserDataResult result) => Debug.Log($"OnSuccessfulSave");

    private void OnFailure(PlayFabError error)
    {
        Debug.Log($"OnFailure {error.ErrorMessage}");
        _result = null;
    }
}
