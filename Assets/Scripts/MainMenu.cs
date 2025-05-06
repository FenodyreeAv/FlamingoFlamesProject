using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public float delayBeforeLoading = 6f;
   public void PlayGame()
    {
        StartCoroutine(PlayVideoThenLoadScene());
    }

    private IEnumerator PlayVideoThenLoadScene()
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
}
