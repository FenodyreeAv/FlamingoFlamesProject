using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBleed : MonoBehaviour
{
    [SerializeField] public GameObject socketBleedOrgan;
    [SerializeField] private GameObject socketBleedParticleSystem;
    private bool isBleeding = false;
    private Coroutine bleedCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Start bleeding only if the object's name is "Scalpel" or "Saw"
        if (!isBleeding)
        {
            Debug.Log("Bleeding started");
            isBleeding = true;
            bleedCoroutine = StartCoroutine(SpawnBleedParticles());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Stop bleeding when the object exits the trigger
        if (isBleeding && other.CompareTag("Tool"))
        {
            isBleeding = false;
            StopCoroutine(bleedCoroutine);
        }
    }

    private IEnumerator SpawnBleedParticles()
    {
        while (isBleeding)
        {
            // Generate a random rotation
            Quaternion randomRotation = Random.rotation;

            // Spawn the particle system at the current position with the random rotation
            Instantiate(socketBleedParticleSystem, transform.position, randomRotation);

            // Wait for 1 second before spawning the next particle system
            yield return new WaitForSeconds(1f);
        }
    }

}
