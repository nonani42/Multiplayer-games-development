using UnityEngine;

public class PhotonEntryPoint : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private ConnectionView connectionView;
    [SerializeField] private LobbyView lobbyView;
    [SerializeField] private RoomView roomView;

    private PhotonConnection _connection;

    private void Start()
    {
        mainPanel.SetActive(true);
        _connection = new PhotonConnection(connectionView, lobbyView, roomView);
    }
}
