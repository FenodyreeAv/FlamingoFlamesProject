using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] FixedJoint fixedJoint;

    [SerializeField] float touchDistance = 1.0f;
    [SerializeField] float breakForce = 10.0f;

    [SerializeField] Color highlightColor = Color.yellow;
    [SerializeField] Color defaultColor = Color.white;

    private GameObject highlightedObject = null;

    // Update is called once per frame
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
    }

    void HighlightClosestObject()
    {
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        // Find all objects with the "GrabbableObject" tag
        GameObject[] grabbableObjects = GameObject.FindGameObjectsWithTag("GrabbableObject");

        foreach (GameObject obj in grabbableObjects)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = obj;
            }
        }

        // Check if the closest object is within the touch distance
        if (closestObject != null && closestDistance <= touchDistance)
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
            // Remove highlight if no object is within range
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
            fixedJoint.breakForce = breakForce; // Set the break force
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
