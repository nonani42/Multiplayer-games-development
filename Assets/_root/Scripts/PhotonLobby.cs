using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : IMatchmakingCallbacks, ILobbyCallbacks
{
    private LobbyView _lobbyUi;
    private RoomView _roomUi;
    private PhotonRoom _room;

    public PhotonLobby(LobbyView lobbyUi, RoomView roomUi)
    {
        _lobbyUi = lobbyUi;
        _roomUi = roomUi;
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinLobby();
    }

    private void GetRandomRoom() => PhotonNetwork.JoinRandomOrCreateRoom();
    private void GetRoom(string str)
    {
        string temp;
        if (str == string.Empty)
            temp = Guid.NewGuid().ToString()[..8];
        else 
            temp = str;
        PhotonNetwork.CreateRoom(temp);
    }

    public void OnCreatedRoom() => Debug.Log("OnCreatedRoom");

    public void OnCreateRoomFailed(short returnCode, string message) => Debug.Log($"OnCreateRoomFailed: {returnCode}, {message}");

    public void OnFriendListUpdate(List<FriendInfo> friendList) => Debug.Log($"OnFriendListUpdate: {friendList}");

    public void OnJoinedLobby() 
    {
        if (_lobbyUi != null)
            _lobbyUi.OnEnteredLobby(GetRandomRoom, GetRoom);
        Debug.Log("OnJoinedLobby");
    }

    public void OnJoinedRoom()
    {
        _room = new PhotonRoom(_roomUi);
        if (_lobbyUi != null)
            _lobbyUi.OnLeftLobby();
        Debug.Log("OnJoinedRoom"); 
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        if (_lobbyUi != null)
            _lobbyUi.OnEnteredLobby(GetRandomRoom, GetRoom);
        Debug.Log($"OnJoinRandomFailed: {returnCode}, {message}");
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        if (_lobbyUi != null)
            _lobbyUi.OnEnteredLobby(GetRandomRoom, GetRoom);
        Debug.Log($"OnJoinRoomFailed: {returnCode}, {message}");
    }

    public void OnLeftLobby() => Debug.Log("OnLeftLobby");

    public void OnLeftRoom()
    {
        if (_lobbyUi != null)
            _lobbyUi.OnEnteredLobby(GetRandomRoom, GetRoom);
        Debug.Log("OnLeftRoom");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) => Debug.Log($"OnLobbyStatisticsUpdate: {lobbyStatistics}");

    public void OnRoomListUpdate(List<RoomInfo> roomList) => _lobbyUi.ShowRooms(roomList);

    internal void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        _lobbyUi.OnLeftLobby();

        if ( _room != null )
            _room.OnDestroy();

        if(PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();
        Debug.Log("OnDestroyLobby");
    }
}
