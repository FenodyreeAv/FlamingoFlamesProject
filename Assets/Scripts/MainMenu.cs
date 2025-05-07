using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Ryan & Ross
public class MainMenu : MonoBehaviour
{
    [SerializeField] private float delayBeforeLoading = 6f; //Animation duration
    [SerializeField] private float delaySquish = 1; //Delay before squish sound
    [SerializeField] private float delayCrunch = 2; //Delay before crunch sound

    [Header("Assign audio clips to these")]
    [SerializeField] private AudioClip squishSound;
    [SerializeField] private AudioClip crunchSound; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayGame()
    {
        StartCoroutine(PlayVideoThenLoadScene());
        StartCoroutine(playSquish());
        StartCoroutine(playCrunch());
    }

    private IEnumerator PlayVideoThenLoadScene()
    {
        yield return new WaitForSeconds(delayBeforeLoading);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private IEnumerator playSquish()
    {
        yield return new WaitForSeconds(delaySquish);
        audioSource.PlayOneShot(squishSound);
    }
    private IEnumerator playCrunch()
    {
        yield return new WaitForSeconds(delayCrunch);
        audioSource.PlayOneShot(crunchSound);
    }

    public void QuitGame()
    {
        //if (Application.isEditor)
        //{
        //    UnityEditor.EditorApplication.isPlaying = false;
        //}
        //else
        //{
            Application.Quit();
        //}
    }
}
