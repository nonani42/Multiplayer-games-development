using Photon.Pun;
using UnityEngine;

public class GameEntryPoint : MonoBehaviour
{
    [SerializeField] private Transform _healthHolder;
    private PlayerController _player;

    private PlayFabSaveLoad _saveLoad;

    private void Awake()
    {
        _saveLoad = new PlayFabSaveLoad();
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        _player = new PlayerController(_saveLoad, _healthHolder);
    }

    private void Update()
    {
        _player.Update();
    }

    private void OnDestroy()
    {
        _player.OnDestroy();
    }
}
