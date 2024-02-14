using Photon.Pun;
using System;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerParams
{
    public Color color;
    public Vector3 position;
}

public class PlayerController
{
    private PlayFabSaveLoad _saveLoad;
    private readonly Transform _healthHolder;
    private GameObject _player;
    private PlayerView _playerView;
    private float _speed;
    private float _maxHealth;
    private float _currentHealth;

    private float rate;

    System.Random rand = new System.Random();

    public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

    public PlayerController(PlayFabSaveLoad saveLoad, Transform healthHolder)
    {
        _saveLoad = saveLoad;
        _healthHolder = healthHolder;
        _speed = rand.Next(1, 10);
        Vector3 position = new Vector3(rand.Next(0, 10), rand.Next(0, 5), rand.Next(0, 10));

        _player = PhotonNetwork.Instantiate("PlayerPrefab", position, Quaternion.identity);

        _playerView = _player.GetComponent<PlayerView>();

        LoadParams();
        _playerView.SetId();
    }

    private async void LoadParams()
    {
        Tuple<float, float> load = await _saveLoad.LoadGame();
        if (load != null && load.Item1 > 0)
        {
            _maxHealth = load.Item1;
            _currentHealth = load.Item2;
        }
        else
        {
            _maxHealth = _currentHealth = 100;
        }

        _playerView.SetHealth(_maxHealth, _currentHealth, _healthHolder);
    }

    public void Update()
    {
        Move();
        ReceiveDamage();
    }

    private void ReceiveDamage()
    {
        float dif = 2;
        if (rate >= dif)
        {
            _currentHealth -= rand.Next(1, 10);
            _playerView.ChangeHealth(_currentHealth);
            rate = 0;
            _saveLoad.SaveGame(_maxHealth, _currentHealth);
        }
        else
        {
            rate += Time.deltaTime;
        }
    }

    public void Move()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            _player.transform.position += new Vector3(_speed * Time.deltaTime, 0f, 0f);
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            _player.transform.position -= new Vector3(_speed * Time.deltaTime, 0f, 0f);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            _player.transform.position += new Vector3(0f, _speed * Time.deltaTime, 0f);
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            _player.transform.position -= new Vector3(0f, _speed * Time.deltaTime, 0f);
    }

    public void OnDestroy()
    {
        //_saveLoad.SaveGame(_maxHealth, _currentHealth);
    }
}
