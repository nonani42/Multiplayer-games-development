using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private GameObject _charPanel;
    [SerializeField] private CharButtonView _firstCharBtn;
    [SerializeField] private CharButtonView _secondCharBtn;

    [SerializeField] private GameObject _newCharPanel;
    [SerializeField] private TMP_InputField _newCharName;
    [SerializeField] private Button _newCharSubmit;

    private string _charNameText;


    public event Action<string> OnNewCharAdded;
    public event Action OnFilledCharButtonClicked;

    private void Awake() => gameObject.SetActive(false);

    public void ShowSelectionPanel(int charsNum, string[] names)
    {
        gameObject.SetActive(true);
        _charPanel.SetActive(true);

        _newCharPanel.SetActive(false);

        if (charsNum == 2)
        {
            _firstCharBtn.ShowFilledCharButton(true, SendSelectedChar, names[0]);
            _secondCharBtn.ShowFilledCharButton(true, SendSelectedChar, names[1]);
        }
        else if (charsNum == 1)
        {
            _firstCharBtn.ShowFilledCharButton(true, SendSelectedChar, names[0]);
            _secondCharBtn.ShowEmptyCharButton(false, CreateChar);
        }
        else
        {
            _firstCharBtn.ShowEmptyCharButton(false, CreateChar);
            _secondCharBtn.ShowEmptyCharButton(false, CreateChar);
        }
    }

    public void SendSelectedChar() => OnFilledCharButtonClicked?.Invoke();

    public void CreateChar()
    {
        _newCharPanel.SetActive(true);
        _newCharSubmit.onClick.AddListener(AddNewChar);
    }

    private void AddNewChar()
    {
        if (_newCharName.text == null)
            _charNameText = "default";
        _charNameText = _newCharName.text;
        _newCharPanel.SetActive(false);
        _newCharSubmit.onClick.RemoveListener(AddNewChar);
        OnNewCharAdded?.Invoke(_charNameText);
    }

    public void OnDestroy()
    {
        _firstCharBtn.OnDestroy();
        _secondCharBtn.OnDestroy();
        _newCharSubmit.onClick.RemoveListener(AddNewChar);
    }
}
