using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIButtonSound : MonoBehaviour
{
    public AudioClip clickSound;
    public AudioSource audioSource;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        SoundManager.Instance.PlaySound(clickSound);
    }
    public void BackToGallery()
    {
        StartCoroutine(BackCoroutine());
    }
    IEnumerator BackCoroutine()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("GameImage");
    }
}