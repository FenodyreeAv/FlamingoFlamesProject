using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodGuage : MonoBehaviour
{
    [SerializeField] private GameObject gameManagerObject; // Reference to the GameManager object
    [SerializeField] private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    [SerializeField] private List<Sprite> bloodSprites; // List of sprites for different blood loss levels

    private float bloodloss; // Blood loss value from the GameManager

    void Start()
    {
        // Ensure the SpriteRenderer is assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        // Get the bloodloss value from the GameManager
        if (gameManagerObject != null)
        {
            GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
            if (gameManager != null)
            {
                bloodloss = Mathf.Clamp(gameManager.bloodLoss, 1, 100); // Ensure bloodloss is within range
            }
        }

        // Determine the sprite index based on bloodloss
        int spriteIndex = Mathf.FloorToInt((bloodloss - 1) / (100f / bloodSprites.Count));
        spriteIndex = Mathf.Clamp(spriteIndex, 0, bloodSprites.Count - 1);

        // Update the sprite
        if (spriteRenderer != null && bloodSprites.Count > 0)
        {
            spriteRenderer.sprite = bloodSprites[spriteIndex];
        }
    }
}
