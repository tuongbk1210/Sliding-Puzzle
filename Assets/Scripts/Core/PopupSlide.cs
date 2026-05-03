using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSlide : MonoBehaviour
{
    public RectTransform content;
    public float duration = 0.3f;

    private Vector2 hiddenPos;
    private Vector2 shownPos;

    private void Start()
    {
        hiddenPos = new Vector2(0, -content.rect.height);
        shownPos = new Vector2(0, 0);

        content.anchoredPosition = hiddenPos;
    }

    public void Show()
    {
        StopAllCoroutines();
        StartCoroutine(Slide(shownPos, hiddenPos));


    }

    IEnumerator Slide(Vector2 from , Vector2 to)
    {
        float time = 0;
        while(time < duration)
        {
            time += Time.deltaTime;
            content.anchoredPosition = Vector2.Lerp(from, to, time / duration);
            yield return null;
        }
        content.anchoredPosition = to;
    }
}
