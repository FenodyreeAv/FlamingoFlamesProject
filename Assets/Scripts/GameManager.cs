using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float bloodLoss = 0f; // Blood loss value (1-100)
    [SerializeField] private Image bloodGuage; // Reference to the UI Image component
    [SerializeField] private Image screenOverlay; // Reference to the UI Image for darkening the screen

    [SerializeField] private Sprite[] bloodGuageSprites; // Array of 5 sprites for the blood gauge

    // Start is called before the first frame update
    void Start()
    {
        if (bloodGuageSprites.Length != 5)
        {
            Debug.LogError("Please assign exactly 5 sprites to the bloodGuageSprites array.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBloodGuage();
    }
    private void UpdateBloodGuage()
    {
        if (bloodGuageSprites.Length == 5 && bloodGuage != null)
        {
            // Map bloodLoss (1-100) to an index (0-4)
            int spriteIndex = Mathf.Clamp(Mathf.FloorToInt((bloodLoss - 1) / 20), 0, 4);

            // Update the sprite of the blood gauge
            bloodGuage.sprite = bloodGuageSprites[spriteIndex];
        }

        // Darken the screen overlay when bloodLoss exceeds 80%
        if (screenOverlay != null)
        {
            if (bloodLoss > 80)
            {
                float opacity = Mathf.Clamp01((bloodLoss - 80) / 20f); // Map 80-100 to 0-1
                Color overlayColor = screenOverlay.color;
                overlayColor.a = opacity; // Adjust the alpha channel
                screenOverlay.color = overlayColor;
            }
        }

        // Restart the scene when bloodLoss reaches 100%
        if (bloodLoss >= 100)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
