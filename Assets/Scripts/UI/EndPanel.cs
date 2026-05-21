
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndPanel : MonoBehaviour
{
    public Text text;
    public Button button;

    private void OnEnable()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        this.gameObject.SetActive(false);
    }

    public void SetText(string content)
    {
        text.text = content;
    }
}
