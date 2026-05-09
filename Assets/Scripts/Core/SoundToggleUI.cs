using UnityEngine;
using UnityEngine.UI;

public class SoundToggleUI : MonoBehaviour
{
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private Image buttonImage;

    private bool isMuted;

    void Start()
    {
        buttonImage = GetComponent<Image>();

        isMuted = PlayerPrefs.GetInt("SOUND_OFF", 0) == 1;

        ApplyState();

        GetComponent<Button>().onClick.AddListener(ToggleSound);
    }

    void ToggleSound()
    {
        isMuted = !isMuted;

        PlayerPrefs.SetInt("SOUND_OFF", isMuted ? 1 : 0);

        ApplyState();
    }

    void ApplyState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;

        buttonImage.sprite = isMuted
            ? soundOffSprite
            : soundOnSprite;
    }
}