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
        // Start bleeding only if the object has the "Tool" tag
        if (!isBleeding && other.CompareTag("Tool"))
        {
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
            // Spawn the particle system at the current position and rotation
            Instantiate(socketBleedParticleSystem, transform.position, transform.rotation);

            // Wait for 1 second before spawning the next particle system
            yield return new WaitForSeconds(1f);
        }
    }
}
