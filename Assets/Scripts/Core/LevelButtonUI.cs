using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public GameObject tickIcon;

    public Color selectedColor;
    public Color normalColor;

    public int level;

    public Image background;

    private void Awake()
    {
        //background = GetComponent<Image>();
    }

    public void Refresh()
    {

        bool isSelected =
            LevelManager.Instance.IsCurrentLevel(level);

        tickIcon.SetActive(isSelected);

        background.color =
        isSelected ? selectedColor : normalColor;
    }

    public void SelectLevel()
    {
        LevelManager.Instance.SetLevel(level);

        RefreshAllButtons();
    }

    void RefreshAllButtons()
    {
        LevelButtonUI[] buttons =
            FindObjectsOfType<LevelButtonUI>();

        foreach (var btn in buttons)
        {
            btn.Refresh();
        }
    }
}