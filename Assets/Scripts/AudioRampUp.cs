using System.Collections;
using UnityEngine;

public class AudioRampUp : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private float maxVolume = 1.0f; // Maximum volume set in the inspector
    [SerializeField] private float fadeSpeed = 0.5f; // Speed of fade-in
    [SerializeField] private GameObject gameManagerObject;

    private AudioSource[] audioSources;
    private float bloodValue;
    private int currentTrackIndex = 0;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
        
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0; // Ensure all music starts with volume 0
        }
    }

    void Update()
    {
        if (gameManagerObject != null)
        {
            GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
            if (gameManager != null)
            {
                bloodValue = gameManager.bloodLoss;
            }
        }

        int targetTrackIndex = Mathf.FloorToInt(bloodValue / 20.0f);
        if (currentTrackIndex <= targetTrackIndex && currentTrackIndex < audioSources.Length)
        {
            AudioSource currentAudioSource = audioSources[currentTrackIndex]; //Play all sources at scene load, to avoid desync
            if (currentAudioSource.volume < maxVolume)
            {
                currentAudioSource.volume += fadeSpeed * Time.deltaTime; //Slowly increase the volume of a track
                currentAudioSource.volume = Mathf.Min(currentAudioSource.volume, maxVolume);
            }
            else
            {
                currentTrackIndex++; // Move to the next track once the current one reaches max volume
            }
        }
    }
}
