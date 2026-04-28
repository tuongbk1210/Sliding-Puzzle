using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileUI: MonoBehaviour
{
    public Image bg;
    public Image img;

    public Color normalColor = new Color32(249, 248, 234, 255);
    public Color emptyColor;

    private void Awake()
    {
        ColorUtility.TryParseHtmlString("#871400", out emptyColor);
    }

    public void SetTile(Sprite sprite)
    {
        if(sprite == null)
        {
            img.gameObject.SetActive(false);
            bg.color = emptyColor;
        }else
        {
            img.gameObject.SetActive(true);
            img.sprite = sprite;
            bg.color = normalColor;
        }
    }
}
