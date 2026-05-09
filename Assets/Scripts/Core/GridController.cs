using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public GridLayoutGroup grid;
    public RectTransform board;

    public void SetupGrid(int gridSize)
    {
        float boardWidth = board.rect.width;

        float cellSize = boardWidth / gridSize;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = gridSize;

        grid.cellSize = new Vector2(cellSize, cellSize);
        grid.spacing = Vector2.zero;
    }
}
