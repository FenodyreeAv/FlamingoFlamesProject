using System.Collections;
using UnityEngine;

public class AudioRampUp : MonoBehaviour
{
    [SerializeField] private float maxVolume = 1.0f; // Maximum volume set in the inspector
    [SerializeField] private float fadeSpeed = 0.5f; // Speed of fade-in
    [SerializeField] private GameObject gameManagerObject; // Reference to the GameManagerObject

    private AudioSource[] audioSources;
    private float bloodValue; // Value from the GameManagerObject
    private int currentTrackIndex = 0;

    void Start()
    {
        // Get all AudioSource components attached to this GameObject
        audioSources = GetComponents<AudioSource>();

        // Ensure all audio sources start with volume 0
        foreach (var audioSource in audioSources)
        {
            audioSource.volume = 0;
        }
    }

    void Update()
    {
        // Get the public float value from the GameManagerObject
        if (gameManagerObject != null)
        {
            GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
            if (gameManager != null)
            {
                bloodValue = gameManager.bloodLoss; // Replace 'publicFloatValue' with the actual variable name
            }
        }

        // Determine the current track to fade in based on the gameManagerValue
        int targetTrackIndex = Mathf.FloorToInt(bloodValue / 20.0f);

        // Fade in the current track if it hasn't reached max volume
        if (currentTrackIndex <= targetTrackIndex && currentTrackIndex < audioSources.Length)
        {
            AudioSource currentAudioSource = audioSources[currentTrackIndex];
            if (currentAudioSource.volume < maxVolume)
            {
                currentAudioSource.volume += fadeSpeed * Time.deltaTime;
                currentAudioSource.volume = Mathf.Min(currentAudioSource.volume, maxVolume);
            }
            else
            {
                // Move to the next track once the current one reaches max volume
                currentTrackIndex++;
            }
        }
    }
}
