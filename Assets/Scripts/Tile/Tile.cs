using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int index;
    public int correctIndex;

    public Image image;
    public Image bg;
    public GameManager manager;

    private Vector2 startPos;

    public Color normalColor = new Color32(249, 248, 234, 255);
    public Color emptyColor;

    public bool isEmpty;

    void Awake()
    {
        ColorUtility.TryParseHtmlString("#871400", out emptyColor);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEmpty) return;

        startPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEmpty) return;

        Vector2 endPos = eventData.position;
        Vector2 delta = endPos - startPos;

        if (delta.magnitude < 50f) return;

        if (manager != null)
        {
            manager.TryMoveBySwipe(this, delta);
        }
    }

    public void SetImage(Sprite sprite)
    {
        isEmpty = false;

        image.sprite = sprite;
        image.enabled = true;

        if (bg != null)
            bg.color = normalColor;
    }

    public void SetEmpty(bool empty)
    {
        isEmpty = empty;

        if (empty)
        {
            image.sprite = null;
            image.enabled = false;

            if (bg != null)
                bg.color = emptyColor;
        }
        else
        {
            image.enabled = true;

            if (bg != null)
                bg.color = normalColor;
        }
    }
}