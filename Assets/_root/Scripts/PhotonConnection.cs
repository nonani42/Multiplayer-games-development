using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotonConnection : IConnectionCallbacks
{
    private ConnectionView _connectionUi;
    private LobbyView _lobbyUi;
    private RoomView _roomUi;
    private PhotonLobby _lobby;

    public PhotonConnection(ConnectionView ui, LobbyView lobbyUi, RoomView roomUi)
    {
        _connectionUi = ui;
        _lobbyUi = lobbyUi;
        _roomUi = roomUi;
        PhotonNetwork.AutomaticallySyncScene = true;
        SetBtn();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public void OnDestroy()
    {
        Disconnect();
        PhotonNetwork.RemoveCallbackTarget(this);
        _connectionUi.OnDestroy();

        if(_lobby != null )
            _lobby.OnDestroy();
        Debug.Log("OnDestroyConnection");
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    private void SetBtn()
    {
        if (PhotonNetwork.IsConnected)
            _connectionUi.OnConnected(Disconnect);
        else
            _connectionUi.OnDisconnected(Connect);
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
        SetBtn();
        _connectionUi.Close();
        _lobby = new PhotonLobby(_lobbyUi, _roomUi);
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log($"OnDisconnected: {cause}");
        SetBtn();
        Debug.Log($"PhotonNetwork.IsConnected {PhotonNetwork.IsConnected}");
    }

    public void OnConnected() => Debug.Log("OnConnected");

    public void OnRegionListReceived(RegionHandler regionHandler) => Debug.Log($"OnRegionListReceived: {regionHandler}");

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data) => Debug.Log($"OnCustomAuthenticationResponse: {data}");

    public void OnCustomAuthenticationFailed(string debugMessage) => Debug.Log($"OnCustomAuthenticationFailed: {debugMessage}");
}
