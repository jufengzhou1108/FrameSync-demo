using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏界面
/// </summary>
public class GamePanel : MonoBehaviour
{
    public Image bloodImage;     //血条图片
    public Image backImage;      //背景图片
    public Text hpText;

    public void UpdateHp(int hp)
    {
        bloodImage.rectTransform.sizeDelta = new Vector2(((float)hp / 100) * backImage.rectTransform.sizeDelta.x, backImage.rectTransform.sizeDelta.y);
        hpText.text = hp.ToString();
    }
}
