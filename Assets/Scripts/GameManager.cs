using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Bloodloss counts up from 0 to 100")]
    public float bloodLoss = 0f; // Blood loss value (1-100)

    [Header("Set to UI Image object")]
    [SerializeField] private Image bloodGuage;
    [SerializeField] private Image screenOverlay;

    [Header("Assign the 5 animated guage sprites")]
    [SerializeField] private Sprite[] bloodGuageSprites; // Array of 5 sprites for the blood gauge

    void Update()
    {
        UpdateBloodGuage();
    }
    private void UpdateBloodGuage()
    {
        if (bloodGuageSprites.Length == 5 && bloodGuage != null)
        {
            int spriteIndex = Mathf.Clamp(Mathf.FloorToInt((bloodLoss - 1) / 20), 0, 4); //5 Steps of bloodloss

            bloodGuage.sprite = bloodGuageSprites[spriteIndex];
        }

        
        if (screenOverlay != null)
        {
            if (bloodLoss > 80) //Increase alpha of screen overlay at 20% health
            {
                float opacity = Mathf.Clamp01((bloodLoss - 80) / 20f);
                Color overlayColor = screenOverlay.color;
                overlayColor.a = opacity;
                screenOverlay.color = overlayColor;
            }
        }


        if (bloodLoss >= 100)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Game over when bloodloss at 100
        }
    }
}
