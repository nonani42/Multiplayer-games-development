using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class LobbyView : PanelView
{
    [SerializeField] private Button _newRoomButton;
    [SerializeField] private TextMeshProUGUI _newRoomText;
    [SerializeField] private GameObject _roomListHolder;
    [SerializeField] private TextMeshProUGUI _roomNameText;
    [SerializeField] private TMP_InputField _roomNameInput;
    [SerializeField] private Dropdown _scrollView;

    private List<TextMeshProUGUI> roomsList = new List<TextMeshProUGUI>();
    private List<TextMeshProUGUI> pool = new List<TextMeshProUGUI>();


    private void Awake() => gameObject.transform.parent.gameObject.SetActive(false);

    public void OnEnteredLobby(Action randomRoomCallback, Action<string> newRoomCallback)
    {
        gameObject.transform.parent.gameObject.SetActive(true);
        gameObject.SetActive(true);
        actionBtn.interactable = true;
        _newRoomButton.interactable = true;
        _newRoomText.text = "New room";
        _newRoomText.color = Color.cyan;
        actionBtnText.text = "Get a random room";
        actionBtnText.color = Color.green;
        actionBtn.onClick.RemoveAllListeners();
        actionBtn.onClick.AddListener(() => randomRoomCallback());
        actionBtn.onClick.AddListener(OnSearchingRoom);
        _newRoomButton.onClick.AddListener(() => OnCreatingRoom(newRoomCallback));
    }

    public void ShowRooms(List<RoomInfo> updatedRoomList)
    {
        for (int i = 0; i < updatedRoomList.Count; i++)
        {
            if (updatedRoomList[i] == null)
                continue;

            bool contains = false;

            for (int j = roomsList.Count - 1; j >= 0; j--)
            {
                if (roomsList[j].text == updatedRoomList[i].Name)
                {
                    contains = true;
                    if (updatedRoomList[i].RemovedFromList)
                    {
                        roomsList[j].gameObject.SetActive(false);
                        pool.Add(roomsList[j]);
                        roomsList.RemoveAt(j);
                    }
                }
            }
            if (contains)
                continue;

            if(pool.Count == 0)
            {
                TextMeshProUGUI tempRoom = Instantiate(_roomNameText, _roomListHolder.transform);
                tempRoom.gameObject.SetActive(true);
                tempRoom.text = updatedRoomList[i].Name;
                roomsList.Add(tempRoom);

            }
            else
            {
                roomsList.Add(pool[pool.Count - 1]);
                pool.RemoveAt(pool.Count - 1);
                roomsList.Last().gameObject.SetActive(true);
                roomsList.Last().text = updatedRoomList[i].Name;
            }
        }
    }

    public void OnSearchingRoom()
    {
        actionBtn.interactable = false;
        actionBtnText.text = "Searching for a room";
        actionBtnText.color = Color.gray;
    }

    public void OnCreatingRoom(Action<string> newRoomCallback)
    {
        string str = string.Empty;

        if(_roomNameInput.text != null)
            str = _roomNameInput.text;
        newRoomCallback(str);
    }

    public void OnLeftLobby()
    {
        actionBtn.onClick.RemoveAllListeners();
        _newRoomButton.onClick.RemoveAllListeners();
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
