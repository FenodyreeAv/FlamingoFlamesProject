using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRibs : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rib"))
        {
            collision.gameObject.tag = "GrabbableObject";

            Rigidbody ribRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (ribRigidbody != null)
            {
                ribRigidbody.isKinematic = false;
            }

            AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.8f, 1.2f);
                audioSource.volume = Random.Range(0.7f, 1.0f);
                audioSource.Play();
            }
        }
    }
}
