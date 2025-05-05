using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutOrgan : MonoBehaviour
{
    private void OnTriggerEnter(UnityEngine.Collider other)
    {
        // Check if the collided object has the tag "Socket"
        if (other.gameObject.CompareTag("Socket"))
        {
            // Find the Rigidbody of the child of the socket object
            Rigidbody childRigidbody = other.gameObject.GetComponentInChildren<Rigidbody>();

            if (childRigidbody != null)
            {
                // Make the Rigidbody non-kinematic
                childRigidbody.isKinematic = false;
                Debug.Log("Child Rigidbody of the socket is now non-kinematic.");
            }
            else
            {
                Debug.LogWarning("No Rigidbody found in the children of the socket object.");
            }
        }
    }
}
