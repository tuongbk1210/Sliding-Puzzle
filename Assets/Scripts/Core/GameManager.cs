using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public Transform board;
    public Tile tilePrefab;
    //public Sprite[] sprites;

    private List<Tile> tiles = new List<Tile>();
    private int size = 3;
    private int emptyIndex;

    private bool isShuffling = false;

    public AudioClip clickSound;

    public GameTimer timer;
    private bool hasStarted = false;

    public Image originalImage;

    public GridController gridController;

    [Header("Level UI")]
    public PopupSlide popupLevel;

    public LevelButtonUI button3x3;
    public LevelButtonUI button4x4;
    public LevelButtonUI button5x5;

    private bool isChangingLevel = false;

    [Header("Result")]
    public ResultPopup resultPopup;
    public AudioClip winSound;
    private int moveCount = 0;


    void Start()
    {
        originalImage.sprite = SelectedImageData.selectedSprite;
        //CreateTiles();
        gridController.SetupGrid(size);
        GenerateFromImage(size);

        //isShuffling = true;
        //Shuffle();
        //isShuffling = false;

    }


    public void SelectLevel(int level)
    {

        StartCoroutine(ChangeLevel(level));
     
    }

    IEnumerator ChangeLevel(int level)
    {
        isChangingLevel = true;
        LevelManager.Instance.SetLevel(level);

        popupLevel.TogglePopup();

        yield return new WaitForSeconds(popupLevel.duration);

        gridController.SetupGrid(level);

        GenerateFromImage(level);
        isChangingLevel = false;
    }


    public void GenerateFromImage(int gridSize)
    {
        size = gridSize;
        hasStarted = false;
        moveCount = 0;

        if (timer != null)
        {
            timer.ResetTimer();
        }

        Texture2D texture = originalImage.sprite.texture;

        int width = texture.width / size;
        int height = texture.height / size;

       
        tiles.Clear();
        emptyIndex = -1;
        for (int i = board.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(board.GetChild(i).gameObject);
        }
        Canvas.ForceUpdateCanvases();

        List<Sprite> sprites = new List<Sprite>();

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int index = y * size + x;

                if (index == size * size - 1)
                    continue;

                Rect rect = new Rect(
                            x * width,
                            texture.height - (y + 1) * height,
                            width,
                            height
                            );

                Sprite tile = Sprite.Create(
                    texture,
                    rect,
                    new Vector2(0.5f, 0.5f)
                );

                sprites.Add(tile);
            }
        }

        for (int i = 0; i < sprites.Count; i++)
        {
            Tile tile = Instantiate(tilePrefab, board);

            tile.index = i;
            tile.correctIndex = i;
            tile.manager = this;

            tile.SetImage(sprites[i]);

            tiles.Add(tile);
        }


        Tile emptyTile = Instantiate(tilePrefab, board);

        emptyTile.index = size * size - 1;
        emptyTile.correctIndex = size * size - 1;
        emptyTile.manager = this;

        emptyTile.SetEmpty(true);

        tiles.Add(emptyTile);

        emptyIndex = size * size - 1;

        isShuffling = true;
        Shuffle();
        isShuffling = false;

        RefreshTileIndexes();
    }

    //public void Set3x3()
    //{
    //    gridController.SetupGrid(3);
    //    GenerateFromImage(3);
    //}

    //public void Set4x4()
    //{
    //    gridController.SetupGrid(4);
    //    GenerateFromImage(4);
    //}

    //public void Set5x5()
    //{
    //    gridController.SetupGrid(5);
    //    GenerateFromImage(5);
    //}

    //void CreateTiles()
    //{
    //    for (int i = 0; i < sprites.Length; i++)
    //    {
    //        Tile tile = Instantiate(tilePrefab, board);

    //        tile.index = i;
    //        tile.correctIndex = i;
    //        tile.manager = this;
    //        tile.SetImage(sprites[i]);

    //        if (i == sprites.Length - 1)
    //        {
    //            tile.SetEmpty(true);
    //            emptyIndex = i;
    //        }

    //        tiles.Add(tile);
    //    }
    //}

    //public void TryMove(Tile tile)
    //{
    //    if (IsAdjacent(tile.index, emptyIndex))
    //    {
    //        Swap(tile.index, emptyIndex);
    //    }
    //}

    void RefreshTileIndexes()
    {
        for (int i = 0; i < board.childCount; i++)
        {
            Tile tile = board.GetChild(i).GetComponent<Tile>();

            if (tile != null)
            {
                tile.index = i;

                if (!tile.image.enabled)
                {
                    emptyIndex = i;
                }
            }
        }
    }

    public void TryMoveBySwipe(Tile tile, Vector2 delta)
    {
        if (isShuffling) return;
        if (isChangingLevel)
            return;
        if (delta.magnitude < 50f)
            return;

        // tile phải nằm cạnh empty
        if (!IsAdjacent(tile.index, emptyIndex))
            return;

        int tileX = tile.index % size;
        int tileY = tile.index / size;

        int emptyX = emptyIndex % size;
        int emptyY = emptyIndex / size;

        Vector2 dir = delta.normalized;

        bool valid = false;

        // empty ở bên phải
        if (emptyX > tileX && dir.x > 0.5f)
            valid = true;

        // empty ở bên trái
        if (emptyX < tileX && dir.x < -0.5f)
            valid = true;

        // empty ở phía dưới
        if (emptyY > tileY && dir.y < -0.5f)
            valid = true;

        // empty ở phía trên
        if (emptyY < tileY && dir.y > 0.5f)
            valid = true;

        if (valid)
        {
            SoundManager.Instance.PlaySound(clickSound);
            moveCount++;
            Swap(tile.index, emptyIndex);
        }
    }

    void Swap(int a, int b)
    {
        Tile tileA = null;
        Tile tileB = null;

        foreach (Tile t in tiles)
        {
            if (t.index == a)
                tileA = t;

            if (t.index == b)
                tileB = t;
        }

        if (tileA == null || tileB == null)
        {
            Debug.LogError("Swap lỗi");
            return;
        }

        int oldA = tileA.index;
        int oldB = tileB.index;

        tileA.index = oldB;
        tileB.index = oldA;

        tileA.transform.SetSiblingIndex(tileA.index);
        tileB.transform.SetSiblingIndex(tileB.index);
        RefreshTileIndexes();
        Canvas.ForceUpdateCanvases();

        LayoutRebuilder.ForceRebuildLayoutImmediate(
            board.GetComponent<RectTransform>()
        );

        emptyIndex = oldA;

        if (!hasStarted && !isShuffling)
        {
            hasStarted = true;
            timer.StartTimer();
        }

        if (!isShuffling && CheckWin())
        {
            StartCoroutine(ShowWinPopup());
        }
    }

    IEnumerator ShowWinPopup()
    {
        string finalTime = timer.GetTimeString();

        timer.StopTimer();

        SoundManager.Instance.PlaySound(winSound);

        yield return new WaitForSeconds(1f);

        resultPopup.Show(size, finalTime, moveCount);

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
            // bỏ qua ô trống
            if (tile.isEmpty)
                continue;

            // tile chưa đúng vị trí
            if (tile.index != tile.correctIndex)
                return false;
        }

        return true;
    }

    }