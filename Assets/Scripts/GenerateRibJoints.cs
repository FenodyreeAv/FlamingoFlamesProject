using UnityEngine;

public class GenerateRibJoints : MonoBehaviour
{
    [SerializeField]
    private float breakForce = 100f; // Force at which the joint will break

    void Start()
    {
        CreateBreakableJoints(transform);
    }

    private void CreateBreakableJoints(Transform parent)
    {
        // Iterate through all child objects
        foreach (Transform child in parent)
        {
            // Add a FixedJoint to the parent object
            FixedJoint joint = parent.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = child.GetComponent<Rigidbody>();

            // Set the break force
            joint.breakForce = breakForce;

            // Recursively process the child's children
            CreateBreakableJoints(child);
        }
    }
}
