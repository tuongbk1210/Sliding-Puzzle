using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleGenerator : MonoBehaviour
{
    public Image originalImage;
    public GameObject tilePrefab;
    public Transform parent;

    public int gridSize = 4;

    private List<Sprite> tiles = new List<Sprite>();

    public void Generate()
    {
        ClearOldTiles();
        Texture2D texture = originalImage.sprite.texture;

        int width = texture.width / gridSize;
        int height = texture.height / gridSize;

        tiles.Clear();

        for(int y = 0; y< gridSize; y++)
        {
            for(int x=0;x< gridSize; x++)
            {
                Rect rect = new Rect(x * width, y * height, width, height);
                Sprite tile = Sprite.Create(
                    texture,
                    rect,
                    new Vector2(0.5f, 0.5f));
                tiles.Add(tile);
            }
        }
        CreateTiles();
    }

    void CreateTiles()
    {
        foreach(var sprite in tiles)
        {
            GameObject tileObj = Instantiate(tilePrefab, parent);
            tileObj.GetComponent<Image>().sprite = sprite;
        }
    }

    void ClearOldTiles()
    {
        foreach(Transform child in parent){
            Destroy(child.gameObject);
        }
    }
}
