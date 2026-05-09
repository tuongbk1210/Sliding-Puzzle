using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class SelectedImageData
{
    public static Sprite selectedSprite;
}


public class ImageListPawner : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform content;
    public List<Sprite> images;

    void Start()
    {
        foreach (var sprite in images)
        {
            GameObject item = Instantiate(itemPrefab, content);
            var ui = item.GetComponent<ImageItemUI>();
            ui.SetSprite(sprite);
            //item.GetComponentInChildren<Image>().sprite = sprite;
            item.GetComponent<Button>().onClick.AddListener(() =>
            {
                StartCoroutine(LoadGame(sprite));
            });
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
    }

    IEnumerator LoadGame(Sprite sprite)
    {
        SoundManager.Instance.PlayButtonSound();

        SelectedImageData.selectedSprite = sprite;

        yield return new WaitForSeconds(0.15f);

        SceneManager.LoadScene("GamePlay");
    }

}

