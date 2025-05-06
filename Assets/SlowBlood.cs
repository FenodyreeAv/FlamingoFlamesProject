using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBlood : MonoBehaviour
{
    [SerializeField] private float bloodLossRate = 0.5f; // Amount to reduce bloodLoss every second
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameManager in the scene
        gameManager = GameObject.FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
            return;
        }

        // Start the coroutine to reduce bloodLoss
        StartCoroutine(ReduceBloodOverTime());
    }

    private IEnumerator ReduceBloodOverTime()
    {
        while (true)
        {
            if (gameManager != null)
            {
                // Reduce bloodLoss by the specified rate
                gameManager.bloodLoss = Mathf.Max(0, gameManager.bloodLoss + bloodLossRate);
            }

            // Wait for 1 second before repeating
            yield return new WaitForSeconds(1f);
        }
    }
}
