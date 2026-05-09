using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageItemUI : MonoBehaviour
{
    public Image mainImage;

    public void SetSprite(Sprite sprite)
    {
        mainImage.sprite = sprite;
    }
}
