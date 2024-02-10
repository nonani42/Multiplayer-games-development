using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class PanelView : MonoBehaviour
{
    [SerializeField] protected Button actionBtn;
    [SerializeField] protected TextMeshProUGUI actionBtnText;

    public void Close() => gameObject.transform.parent.gameObject.SetActive(false);
}
