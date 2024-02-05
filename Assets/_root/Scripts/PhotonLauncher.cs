using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button connectionBtn;
    [SerializeField] private TextMeshProUGUI btnText;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion(which allows you to make breaking changes).
    /// </summary>
    string gameVersion = "1";

    public void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Start()
    {
        SetBtn();
    }

    public void OnDestroy()
    {
        Disconnect();
        connectionBtn.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Start the connection process.
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // we check if we are connected or not, we join if we are, else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            // #Critical 
            // we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical
            // we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    private void Disconnect()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"Disconected from Photon: {cause}");
        SetBtn();
        Debug.Log($"PhotonNetwork.IsConnected {PhotonNetwork.IsConnected}");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN");
        SetBtn();
    }

    private void SetBtn()
    {
        if (PhotonNetwork.IsConnected)
        {
            btnText.text = "Disconnect";
            btnText.color = Color.red;
            connectionBtn.onClick.RemoveListener(Connect);
            connectionBtn.onClick.AddListener(Disconnect);
        }
        else
        {
            btnText.text = "Connect";
            btnText.color = Color.green;
            connectionBtn.onClick.RemoveListener(Disconnect);
            connectionBtn.onClick.AddListener(Connect);
        }
    }
}