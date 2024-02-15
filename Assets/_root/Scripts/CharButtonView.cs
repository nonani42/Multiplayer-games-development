using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharButtonView : MonoBehaviour
{
    [SerializeField] private Button _charBtn;
    [SerializeField] private GameObject _emptyPanel;
    [SerializeField] private GameObject _filledPanel;
    [SerializeField] private TextMeshProUGUI _nameText;

    private void Awake() => gameObject.transform.parent.gameObject.SetActive(false);

    public void ShowFilledCharButton(bool isFilled, Action callback, string name)
    {
        _charBtn.gameObject.SetActive(true);
        _filledPanel.SetActive(isFilled);
        _emptyPanel.SetActive(!isFilled);
        _charBtn.onClick.AddListener(() => callback());
        _nameText.text = name;
    }

    public void ShowEmptyCharButton(bool isFilled, Action callback)
    {
        _charBtn.gameObject.SetActive(true);
        _filledPanel.SetActive(isFilled);
        _emptyPanel.SetActive(!isFilled);
        _charBtn.onClick.AddListener(() => callback());
    }

    internal void OnDestroy()
    {
        _charBtn.onClick.RemoveAllListeners();
    }
}
