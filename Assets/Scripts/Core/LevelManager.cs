using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentLevel = 3;

    private void Awake()
    {
        Instance = this;
    }

    public void SetLevel(int level)
    {
        currentLevel = level;

    }

    public bool IsCurrentLevel(int level)
    {
        return currentLevel == level;
    }
}
