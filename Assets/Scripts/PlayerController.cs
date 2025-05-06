using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component Refs")]
    [SerializeField] private FixedJoint fixedJoint;
    [SerializeField] private Rigidbody targetRigidbody; // Reference to the Target's Rigidbody
    [SerializeField] private Rigidbody tipRigidbody; // Reference to the Tip's Rigidbody
    [SerializeField] private Collider thisCollider;
    [SerializeField] private List<Collider> overlappingColliders = new List<Collider>(); // List to track overlapping colliders
    [SerializeField] private InputActionAsset playerControls;

    [Header("Grabbed Object Joint Strength")]
    [SerializeField] float breakForce = 10.0f;

    [Header("Targetted Object Highlighting")]
    [SerializeField] Color highlightColor = Color.yellow;
    [SerializeField] Color defaultColor = Color.white;

    [Header("Player Arm Move Speed")]
    [SerializeField] float forceAmount = 10.0f; // Force magnitude

    [Header("Input Actions")]
    [SerializeField] private string playerNumber;

    private GameObject highlightedObject = null;

    private InputAction grabAction;
    private InputAction moveAction;
    private InputAction rotateAction;

    private void Awake()
    {
        // Initialize Input Actions
        //Debug.Log("Player" + playerNumber + " controls initialized.");
        //Debug.Log("P" + playerNumber + " Grab");
        //Debug.Log("P" + playerNumber + " Grab");

        grabAction = playerControls.FindActionMap("Player" + playerNumber).FindAction("P" + playerNumber + " Grab");
        moveAction = playerControls.FindActionMap("Player" + playerNumber).FindAction("P" + playerNumber + " Move");
        rotateAction = playerControls.FindActionMap("Player" + playerNumber).FindAction("P" + playerNumber + " Wrist");


        // Enable Input Actions
        grabAction.Enable();
        moveAction.Enable();
        rotateAction.Enable();
    }

    private void OnEnable()
    {
        grabAction.Enable();
        moveAction.Enable();
        rotateAction.Enable();
    }
    private void OnDisable()
    {
        grabAction.Disable();
        moveAction.Disable();
        rotateAction.Disable();
    }

    void Update()
    {
        HighlightClosestObject();

        if (grabAction.triggered)
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


        if (targetRigidbody != null && tipRigidbody != null)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput.y > 0)
            {
                targetRigidbody.AddForce(Vector3.forward * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.forward * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (moveInput.y < 0)
            {
                targetRigidbody.AddForce(Vector3.back * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.back * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (moveInput.x < 0)
            {
                targetRigidbody.AddForce(Vector3.left * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.left * (forceAmount * 0.2f), ForceMode.Force);
            }
            if (moveInput.x > 0)
            {
                targetRigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Force);
                tipRigidbody.AddForce(Vector3.right * (forceAmount * 0.2f), ForceMode.Force);
            }

            if (rotateAction.ReadValue<float>() > 0)
            {
                tipRigidbody.AddForce(Vector3.up * forceAmount, ForceMode.Force);
            }
            if (rotateAction.ReadValue<float>() < 0)
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

                    // Reset the parent's color if it has the "Organ" tag
                    Transform parent = highlightedObject.transform.parent;
                    if (parent != null && parent.CompareTag("Organ"))
                    {
                        SetObjectColor(parent.gameObject, defaultColor);
                    }
                }

                // Highlight the new closest object
                highlightedObject = closestObject;
                SetObjectColor(highlightedObject, highlightColor);

                // Check if the parent has the "Organ" tag and change its color
                Transform newParent = highlightedObject.transform.parent;
                if (newParent != null && newParent.CompareTag("Organ"))
                {
                    SetObjectColor(newParent.gameObject, highlightColor);
                }
            }
        }
        else
        {
            // Remove highlight if no object is overlapping
            if (highlightedObject != null)
            {
                SetObjectColor(highlightedObject, defaultColor);

                // Reset the parent's color if it has the "Organ" tag
                Transform parent = highlightedObject.transform.parent;
                if (parent != null && parent.CompareTag("Organ"))
                {
                    SetObjectColor(parent.gameObject, defaultColor);
                }

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

            AudioSource audioSource = highlightedObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f); // Slight pitch variation
                audioSource.volume = Random.Range(0.8f, 1.0f); // Slight volume variation
                audioSource.Play();
            }
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
