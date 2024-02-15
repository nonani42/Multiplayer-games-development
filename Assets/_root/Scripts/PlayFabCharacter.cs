using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayFabCharacter
{
    private CharacterView _characterView;

    public event Action OnCharSelected;

    public PlayFabCharacter(CharacterView characterView)
    {
        _characterView = characterView;
        _characterView.OnFilledCharButtonClicked += () => OnCharSelected?.Invoke();
        _characterView.OnNewCharAdded += AddCharacter;
    }

    private void OnAccountFailure(PlayFabError error) => Debug.Log($"OnAccountFailure {error.ErrorMessage}");

    private void OnAccountSuccess(GetAccountInfoResult result) => GetCharacterList();

    public void SelectCharacter() => PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnAccountSuccess, OnAccountFailure);

    private void AddCharacter(string name)
    {
        Debug.Log($"AddCharacter in PlayFabClientAPI {name}");

        PlayFabClientAPI.GrantCharacterToUser(
            new GrantCharacterToUserRequest { CharacterName = name, ItemId = "creationToken" },
            OnGrantCharacterToUserSuccess, 
            OnGrantCharacterToUserFailure);
    }

    private void GetCharacterList() => PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(), OnCharacterListSuccess, OnCharacterListFailure);

    private void OnCharacterListFailure(PlayFabError error) => Debug.Log($"OnGetCharacterListFailure {error.ErrorMessage}");

    private void OnCharacterListSuccess(ListUsersCharactersResult result)
    {
        IEnumerable<string> names = from chars
                         in result.Characters
                         select chars.CharacterName;
        _characterView.ShowSelectionPanel(result.Characters.Count, names.ToArray());
    }

    private void OnGrantCharacterToUserFailure(PlayFabError error) => Debug.Log($"OnGrantCharacterToUserFailure {error.ErrorMessage}");

    private void OnGrantCharacterToUserSuccess(GrantCharacterToUserResult result)
    {
        PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest
        {
            CharacterId = result.CharacterId,
            CharacterStatistics = new Dictionary<string, int>
            {
                {"Level", 1},
                {"XP", 0},
                {"Gold", 0}
            }
        }, 
        OnUpdateCharacterStatisticsSuccess,
        OnUpdateCharacterStatisticsFailure);
    }

    private void OnUpdateCharacterStatisticsFailure(PlayFabError error) => Debug.Log($"OnUpdateCharacterStatisticsFailure {error.ErrorMessage}");

    private void OnUpdateCharacterStatisticsSuccess(UpdateCharacterStatisticsResult result) => GetCharacterList();

    public void OnDestroy()
    {
        _characterView.OnFilledCharButtonClicked -= () => OnCharSelected?.Invoke();
        if(_characterView != null)
            _characterView.OnDestroy();
    }
}
