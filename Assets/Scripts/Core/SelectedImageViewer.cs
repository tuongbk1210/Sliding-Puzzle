using UnityEngine;
using UnityEngine.UI;

public class SelectedImageViewer : MonoBehaviour
{
    public Image displayImage;

    void Start()
    {
        displayImage.sprite = SelectedImageData.selectedSprite;
    }
}