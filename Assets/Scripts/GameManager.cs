using Fantasy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance{ get; set; }
    [SerializeField]
    private GameObject startUI;
    public EndPanel endPanel;
    public bool isStart = false;

    public Texture2D texture;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InputManager.Instance.enabled = false;
    }

    public void StartGame()
    {
        startUI.SetActive(false);
        InputManager.Instance.enabled = true;
        CursorTool.SetCursor(texture);
    }

    public void EndGame(long loserId)
    {
        FSManager.Instance.StopSync();
        startUI.SetActive(true);
        InputManager.Instance.enabled = false;
        endPanel.SetText(loserId == NetManager.Instance.Id ? "你输了" : "你赢了");
        endPanel.gameObject.SetActive(true);
        isStart = false;
        NetManager.Instance.session.Send(new C2G_EndSync());
    }
}
