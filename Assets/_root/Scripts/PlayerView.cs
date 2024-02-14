using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviourPunCallbacks, IPunObservable, IInRoomCallbacks, IPunInstantiateMagicCallback
{
    [SerializeField] private GameObject _healthBar;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private TextMeshProUGUI _idText;

    private Transform _uiParent;
    private Material material;
    private Color color;

    private const string KEY_COLOR_R = "colorR";
    private const string KEY_COLOR_G = "colorG";
    private const string KEY_COLOR_B = "colorB";
    private const string KEY_COLOR_A = "colorA";

    private void Start()
    {
        PhotonNetwork.AddCallbackTarget(this);
        PhotonNetwork.AutomaticallySyncScene = true;

        color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        Hashtable customProps = new Hashtable()
        {
            { KEY_COLOR_R, color.r },
            { KEY_COLOR_G, color.g },
            { KEY_COLOR_B, color.b },
            { KEY_COLOR_A, color.a },
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);

        SetColor(color);
    }

    public void ChangeHealth(float health)
    {
        _healthSlider.value = health;
    }

    private void SetMaxHealth(float health)
    {
        _healthSlider.maxValue = health;
        ChangeHealth(health);
    }

    public void SetHealth(float maxHealth, float currentHealth, Transform healthHolder)
    {
        _uiParent = healthHolder;
        _healthBar.transform.SetParent(_uiParent);
        SetMaxHealth(maxHealth);
        ChangeHealth(currentHealth);
    }

    public void SetId()
    {
        _idText.text = photonView.ViewID.ToString();
    }

    private void SetColor(Color color)
    {
        if(material == null)
            material = GetComponentInChildren<Renderer>().material;
        Debug.Log($"material {material == null}, color {color == null}");
        if(color != null)
            material.color = color;
    }

    private void UpdateColor(float colorR, float colorG, float colorB, float colorA)
    {
        if (material == null)
            material = GetComponentInChildren<Renderer>().material;

        float r;
        float g;
        float b;
        float a;

        r = (colorR != default && colorR != material.color.r) ? colorR : material.color.r;
        g = (colorG != default && colorR != material.color.g) ? colorR : material.color.g;
        b = (colorB != default && colorR != material.color.b) ? colorR : material.color.b;
        a = (colorA != default && colorR != material.color.a) ? colorR : material.color.a;

        Color newColor = new Color(r, g, b, a);
        SetColor(newColor);
    }

    public bool CheckMine()
    {
        return photonView.IsMine;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(_healthSlider.value);
            stream.SendNext(_healthSlider.maxValue);
            stream.SendNext(_idText.text);
        }
        else
        {
            // Network player, receive data
            _healthSlider.value = (float)stream.ReceiveNext();
            _healthSlider.maxValue = (float)stream.ReceiveNext();
            _idText.text = (string)stream.ReceiveNext();
        }
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("OnPhotonPlayerPropertiesChanged");

        Player player = targetPlayer;
        Debug.Log($"player {player.UserId}");

        Debug.Log($"{changedProps.TryGetValue(KEY_COLOR_R, out object valueR)}");
        if (!CheckMine())
        {
            Debug.Log("OnPhotonPlayerPropertiesChanged. NotMine");
            changedProps.TryGetValue(KEY_COLOR_R, out valueR);
            changedProps.TryGetValue(KEY_COLOR_G, out object valueG);
            changedProps.TryGetValue(KEY_COLOR_B, out object valueB);
            changedProps.TryGetValue(KEY_COLOR_A, out object valueA);

            Debug.Log($"valueR {valueR == null}, valueG {valueG == null}, valueB {valueB == null}, valueA {valueA == null}");
            UpdateColor((float)valueR, (float)valueG, (float)valueB, (float)valueA);
        }
    }

    //void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
    //{
    //    Debug.Log("OnPhotonPlayerPropertiesChanged");

    //    Player player = targetPlayer;
    //    Debug.Log($"player {player.UserId}");

    //    Debug.Log($"{changedProps.TryGetValue(KEY_COLOR_R, out object valueR)}");
    //    if (!CheckMine())
    //    {
    //        Debug.Log("OnPhotonPlayerPropertiesChanged. NotMine");
    //        changedProps.TryGetValue(KEY_COLOR_R, out valueR);
    //        changedProps.TryGetValue(KEY_COLOR_G, out object valueG);
    //        changedProps.TryGetValue(KEY_COLOR_B, out object valueB);
    //        changedProps.TryGetValue(KEY_COLOR_A, out object valueA);

    //        Debug.Log($"valueR {valueR == null}, valueG {valueG == null}, valueB {valueB == null}, valueA {valueA == null}");
    //        UpdateColor((float)valueR, (float)valueG, (float)valueB, (float)valueA);
    //    }
    //}

    void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!CheckMine())
        {
            Debug.Log("I'm here!");
            if (color != null)
            {
                Hashtable customProps = new Hashtable()
            {
                { KEY_COLOR_R, color.r },
                { KEY_COLOR_G, color.g },
                { KEY_COLOR_B, color.b },
                { KEY_COLOR_A, color.a },
            };
                PhotonNetwork.LocalPlayer.SetCustomProperties(customProps);
            }
        }
    }
}
