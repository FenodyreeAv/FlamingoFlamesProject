using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoint fixedJoint;

    [SerializeField] float breakForce = 10.0f;

    [SerializeField] Color highlightColor = Color.yellow;
    [SerializeField] Color defaultColor = Color.white;

    [SerializeField] Rigidbody targetRigidbody; // Reference to the Target's Rigidbody
    [SerializeField] Rigidbody tipRigidbody; // Reference to the Tip's Rigidbody
    [SerializeField] float forceAmount = 10.0f; // Force magnitude

    [SerializeField] Collider thisCollider;

    private GameObject highlightedObject = null;
    [SerializeField] private List<Collider> overlappingColliders = new List<Collider>(); // List to track overlapping colliders

    void Update()
    {
        HighlightClosestObject();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fixedJoint != null)
            {
                RemoveFixedJoint();
            }
            else
            {
                AddFixedJoint();
            }
        }

        // Control Target's position using forces in global directions
        if (targetRigidbody != null && tipRigidbody != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                targetRigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.forward * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.S))
            {
                targetRigidbody.AddForce(Vector3.back * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.back * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.A))
            {
                targetRigidbody.AddForce(Vector3.left * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.left * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.D))
            {
                targetRigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.right * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                tipRigidbody.AddForce(Vector3.up * forceAmount, ForceMode.Force);
            }
            if (Input.GetKey(KeyCode.E))
            {
                tipRigidbody.AddForce(Vector3.down * forceAmount, ForceMode.Force);
            }
        }
    }

    // Add collider to the list when it enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GrabbableObject"))
        {
            if (!overlappingColliders.Contains(other))
            {
                overlappingColliders.Add(other);
            }
        }
    }

    // Remove collider from the list when it exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("GrabbableObject"))
        {
            if (overlappingColliders.Contains(other))
            {
                overlappingColliders.Remove(other);
            }
        }
    }
    void HighlightClosestObject()
    {
        GameObject closestObject = null;

        // Find the closest overlapping collider with the "GrabbableObject" tag
        foreach (Collider collider in overlappingColliders)
        {
            if (collider.CompareTag("GrabbableObject"))
            {
                closestObject = collider.gameObject;
                break; // Stop at the first overlapping grabbable object
            }
        }

        // Check if there is an overlapping grabbable object
        if (closestObject != null)
        {
            if (highlightedObject != closestObject)
            {
                // Remove highlight from the previously highlighted object
                if (highlightedObject != null)
                {
                    SetObjectColor(highlightedObject, defaultColor);
                }

                // Highlight the new closest object
                highlightedObject = closestObject;
                SetObjectColor(highlightedObject, highlightColor);
            }
        }
        else
        {
            // Remove highlight if no object is overlapping
            if (highlightedObject != null)
            {
                SetObjectColor(highlightedObject, defaultColor);
                highlightedObject = null;
            }
        }
    }

    void SetObjectColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    void AddFixedJoint()
    {
        if (highlightedObject != null)
        {
            fixedJoint = highlightedObject.AddComponent<FixedJoint>();
            fixedJoint.connectedBody = this.GetComponent<Rigidbody>();
            fixedJoint.breakForce = breakForce;
        }
        else
        {
            Debug.Log("No grabbable object within range to attach the fixed joint.");
        }
    }

    void RemoveFixedJoint()
    {
        if (fixedJoint != null)
        {
            Destroy(fixedJoint);
            fixedJoint = null;
        }
    }
}
