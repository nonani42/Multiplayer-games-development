using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhotonRoom : IInRoomCallbacks
{
    private RoomView _roomUi;
    private string _roomName;

    public PhotonRoom(RoomView roomUi)
    {
        _roomUi = roomUi;
        _roomName = PhotonNetwork.CurrentRoom.Name;
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;
        StartPlay();
    }

    public void StartPlay()
    {
        if(PhotonNetwork.IsMasterClient)
        _roomUi.OnEnteredRoom(PhotonNetwork.IsMasterClient, CloseRoom, _roomName);
    }

    public void CloseRoom() => PhotonNetwork.CurrentRoom.IsVisible = false;

    public void OnMasterClientSwitched(Player newMasterClient) => Debug.Log($"OnMasterClientSwitched: {newMasterClient}");

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartPlay();
        Debug.Log($"OnPlayerEnteredRoom: {newPlayer}, PlayerCount: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 0)
            OnDestroy();
        Debug.Log($"OnPlayerLeftRoom: {otherPlayer}, PlayerCount: {PhotonNetwork.CurrentRoom.PlayerCount}");
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps) => Debug.Log($"OnPlayerPropertiesUpdate: {targetPlayer}, {changedProps}");

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) => Debug.Log($"OnRoomPropertiesUpdate: {propertiesThatChanged}");

    internal void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        _roomUi.OnLeftRoom();
        if (PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();
        Debug.Log("OnDestroyRoom");
    }
}
