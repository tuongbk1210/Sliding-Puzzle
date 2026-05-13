using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ResultPopup : MonoBehaviour
{
    public GameObject root;

    public TMP_Text boardSize;
    public TMP_Text timeTxt;
    public TMP_Text moveText;
    public AudioClip onClickButton;
 
    public void Show(int level, string time, int moves)
    {
        root.SetActive(true);

        boardSize.text = level + "x" + level;
        timeTxt.text = time;
        moveText.text = moves.ToString();
    }

    public void Hide()
    {
        root.SetActive(false);
        SoundManager.Instance.PlaySound(onClickButton);
    }
    public void OnPlayAgain()
    {
        SoundManager.Instance.PlaySound(onClickButton);
        SceneManager.LoadScene("GameImage");
    }
   
}
