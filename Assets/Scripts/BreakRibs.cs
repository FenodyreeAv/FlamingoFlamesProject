using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakRibs : MonoBehaviour
{
    // This method is called when the collider on this object collides with another collider
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object's tag is "Rib"
        if (collision.gameObject.CompareTag("Rib"))
        {
            // Change the tag to "GrabbableObject"
            collision.gameObject.tag = "GrabbableObject";

            // Get the Rigidbody component of the object
            Rigidbody ribRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // Make the object non-kinematic if it has a Rigidbody
            if (ribRigidbody != null)
            {
                ribRigidbody.isKinematic = false;
            }
        }
    }
}
