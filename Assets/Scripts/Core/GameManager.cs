using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform board;
    public Tile tilePrefab;
    public Sprite[] sprites;

    private List<Tile> tiles = new List<Tile>();
    private int size = 4;
    private int emptyIndex;

    private bool isShuffling = false;

    void Start()
    {
        CreateTiles();

        isShuffling = true;
        Shuffle();
        isShuffling = false;
    }

    void CreateTiles()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            Tile tile = Instantiate(tilePrefab, board);

            tile.index = i;
            tile.correctIndex = i;
            tile.manager = this;
            tile.SetImage(sprites[i]);

            if (i == sprites.Length - 1)
            {
                tile.SetEmpty(true);
                emptyIndex = i;
            }

            tiles.Add(tile);
        }
    }

    public void TryMove(Tile tile)
    {
        if (IsAdjacent(tile.index, emptyIndex))
        {
            Swap(tile.index, emptyIndex);
        }
    }

    public void TryMoveBySwipe(Tile tile, Vector2 delta)
    {
        if (delta.magnitude < 50f) return;

        int targetIndex = -1;

        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            // ngang
            targetIndex = delta.x > 0 ? tile.index + 1 : tile.index - 1;
        }
        else
        {
            // dọc
            targetIndex = delta.y > 0 ? tile.index - size : tile.index + size;
        }

        if (IsValidMove(tile.index, targetIndex) && targetIndex == emptyIndex)
        {
            Swap(tile.index, emptyIndex);
        }
    }

    void Swap(int a, int b)
    {
        Tile tileA = tiles.Find(t => t.index == a);
        Tile tileB = tiles.Find(t => t.index == b);

        if (tileA == null || tileB == null)
        {
            Debug.LogError("Swap lỗi: tile null");
            return;
        }

        int temp = tileA.index;
        tileA.index = tileB.index;
        tileB.index = temp;

        tileA.transform.SetSiblingIndex(tileA.index);
        tileB.transform.SetSiblingIndex(tileB.index);

        emptyIndex = a;
        if (!isShuffling && CheckWin())
        {
            Debug.Log("YOU WIN!");
        }
    }

    bool IsAdjacent(int a, int b)
    {
        int ax = a % size;
        int ay = a / size;

        int bx = b % size;
        int by = b / size;

        return (Mathf.Abs(ax - bx) + Mathf.Abs(ay - by)) == 1;
    }

    bool IsValidMove(int from, int to)
    {
        if (to < 0 || to >= size * size) return false;

        int fx = from % size;
        int tx = to % size;

        if (Mathf.Abs(fx - tx) > 1) return false;

        return true;
    }

    void Shuffle()
    {
        for (int i = 0; i < 100; i++)
        {
            List<int> neighbors = GetNeighbors(emptyIndex);
            int rand = neighbors[Random.Range(0, neighbors.Count)];
            Swap(rand, emptyIndex);
        }
    }

    List<int> GetNeighbors(int index)
    {
        List<int> result = new List<int>();

        int x = index % size;
        int y = index / size;

        if (x > 0) result.Add(index - 1);
        if (x < size - 1) result.Add(index + 1);
        if (y > 0) result.Add(index - size);
        if (y < size - 1) result.Add(index + size);

        return result;
    }

    bool CheckWin()
    {
        foreach (var tile in tiles)
        {
            if (!tile.image.enabled) continue;

            if (tile.index != tile.correctIndex)
                return false;
        }
        return true;
    }
}