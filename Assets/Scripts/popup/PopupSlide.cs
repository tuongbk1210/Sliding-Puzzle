using System.Collections;
using UnityEngine;

public class PopupSlide : MonoBehaviour
{
    public RectTransform content;
    public float duration = 0.3f;

    private Vector2 shownPos;
    private Vector2 hiddenPos;

    public LevelButtonUI[] buttons;

    private bool isOpen = false;

    void Start()
    {
        shownPos = content.anchoredPosition;
        hiddenPos = shownPos + Vector2.down * Screen.height;
        content.anchoredPosition = hiddenPos;
    }

    public void TogglePopup()
    {
        StopAllCoroutines();

        if (isOpen)
        {
            StartCoroutine(Slide(shownPos, hiddenPos));
        }
        else
        {
            foreach (var btn in buttons)
            {
                btn.Refresh();
            }
            StartCoroutine(Slide(hiddenPos, shownPos));
        }

        isOpen = !isOpen;
    }

    IEnumerator Slide(Vector2 from, Vector2 to)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            content.anchoredPosition =
                Vector2.Lerp(from, to, time / duration);

            yield return null;
        }

        content.anchoredPosition = to;
    }
}