using Fantasy;
using Fantasy.Async;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : MonoBehaviour
{
    public Button joinButton;

    private void Awake()
    {
        joinButton.onClick.AddListener( () =>
        {
            StartJoin().Coroutine();
        });
    }

    public async FTask StartJoin()
    {
        G2C_JoinResponse response = (G2C_JoinResponse)await NetManager.Instance.session.Call(new C2G_JoinRequest());
        NetManager.Instance.Id = response.id;
        Debug.Log("成功加入对局");
        GameManager.Instance.isStart = true;
    }
}
