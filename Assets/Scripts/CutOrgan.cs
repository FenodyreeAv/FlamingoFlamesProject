using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ryan
public class CutOrgan : MonoBehaviour
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Check if the collided object has the tag "Socket"
        if (other.gameObject.CompareTag("Socket"))
        {
            // Get the SocketBleed component from the collided object
            SocketBleed socketBleed = other.gameObject.GetComponent<SocketBleed>();

            if (socketBleed != null && socketBleed.socketBleedOrgan != null)
            {
                // Access the socketBleedOrgan and perform operations
                Rigidbody organRigidbody = socketBleed.socketBleedOrgan.GetComponent<Rigidbody>();

                if (organRigidbody != null)
                {
                    // Make the Rigidbody non-kinematic
                    organRigidbody.isKinematic = false;
                    Debug.Log("socketBleedOrgan's Rigidbody is now non-kinematic.");
                }
                else
                {
                    Debug.LogWarning("No Rigidbody found on the socketBleedOrgan.");
                }
            }
            else
            {
                Debug.LogWarning("SocketBleed component or socketBleedOrgan is missing.");
            }
        }
    }
}
