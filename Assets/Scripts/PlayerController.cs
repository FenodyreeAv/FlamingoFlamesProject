using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject heart; // Reference to the heart object
    [SerializeField] SpringJoint springJoint; // Reference to the spring joint
    [SerializeField] float touchDistance = 1.0f; // Maximum distance to "touch" the heart
    [SerializeField] float springForce = 50.0f; // Spring force
    [SerializeField] float damper = 5.0f; // Damper value
    [SerializeField] float breakForce = 10.0f; // Break force


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //print("Space key pressed");
            if (springJoint != null)
            {
                RemoveSpringJoint();
            }
            else
            {
                AddSpringJoint();
            }
        }
    }

    void AddSpringJoint()
    {
        GameObject closestObject = null;
        float closestDistance = float.MaxValue;

        // Find all objects with the "GrabbableObject" tag
        GameObject[] grabbableObjects = GameObject.FindGameObjectsWithTag("GrabbableObject");

        foreach (GameObject obj in grabbableObjects)
        {
            // Check if the object has a Rigidbody component
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }
        }

        // Check if the closest object is within the touch distance
        if (closestObject != null && closestDistance <= touchDistance)
        {
            springJoint = closestObject.AddComponent<SpringJoint>();
            springJoint.connectedBody = this.GetComponent<Rigidbody>();
            springJoint.spring = springForce;
            springJoint.damper = damper;
            springJoint.anchor = Vector3.zero; // Adjust the anchor as needed
            springJoint.breakForce = breakForce; // Set the break force
        }
        else
        {
            Debug.Log("No grabbable object within range to attach the spring joint.");
        }
    }




    void RemoveSpringJoint()
    {
        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint = null;
        }
    }
}
