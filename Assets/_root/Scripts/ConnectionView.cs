using System;
using UnityEngine;

public class ConnectionView : PanelView
{
    public void OnConnected(Action callback)
    {
        actionBtnText.text = "Disconnect";
        actionBtnText.color = Color.red;
        actionBtn.onClick.RemoveAllListeners();
        actionBtn.onClick.AddListener(() => callback());
    }

    public void OnDisconnected(Action callback)
    {
        actionBtnText.text = "Connect";
        actionBtnText.color = Color.green;
        actionBtn.onClick.RemoveAllListeners();
        actionBtn.onClick.AddListener(() => callback());
    }

    public void OnDestroy() => actionBtn.onClick.RemoveAllListeners();
}
