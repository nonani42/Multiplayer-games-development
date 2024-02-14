using Photon.Pun;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomView : PanelView
{
    [SerializeField] private TextMeshProUGUI _successText;
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private Button _closeRoomButton;
    [SerializeField] private TextMeshProUGUI _closeRoomText;

    private bool _isMaster = false;

    private void Awake() => gameObject.transform.parent.gameObject.SetActive(false);

    public void OnEnteredRoom(bool isMaster, Action CloseRoom, string roomName)
    {
        _isMaster = isMaster;
        gameObject.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);

        _successText.enabled = false;
        _roomNameInput.enabled = true;

        _roomNameInput.text = roomName;
        _roomNameInput.readOnly = true;

        actionBtn.interactable = false;
        _closeRoomButton.interactable = _isMaster;

        actionBtnText.text = "Play";
        actionBtnText.color = Color.blue;

        _closeRoomText.text = "Close room";
        _closeRoomText.color = Color.red;

        actionBtn.onClick.RemoveAllListeners();
        _closeRoomButton.onClick.RemoveAllListeners();

        actionBtn.onClick.AddListener(OnPlay);
        _closeRoomButton.onClick.AddListener(() => CloseRoom());
        _closeRoomButton.onClick.AddListener(OnClose);
    }

    private void Update()
    {
        if(_roomNameInput.isFocused)
            GUIUtility.systemCopyBuffer = _roomNameInput.text;
    }

    public void OnPlay()
    {
        actionBtn.interactable = false;
        actionBtnText.color = Color.gray;
        _successText.enabled = true;
        _successText.color = Color.green;
        _successText.text = "Now you are playing"; 
        PhotonNetwork.LoadLevel(1);
        //SceneManager.LoadScene(1);
    }

    public void OnClose()
    {
        _closeRoomButton.interactable = false;
        _closeRoomText.color = Color.gray;
        actionBtn.interactable = _isMaster;
    }

    public void OnLeftRoom()
    {
        actionBtn.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
