using UnityEngine;

public class SimpleIL : MonoBehaviour
{
    public Transform pivot, upper, lower, effector, tip;
    public Transform targetTransform;
    public Vector3 normal = Vector3.up;

    public float speed = 5.0f;
    public float maxDistanceFromTip = 5.0f; // Maximum allowable distance from the tip

    private Rigidbody targetRigidbody; // Rigidbody for the target

    float upperLength, lowerLength, effectorLength, pivotLength;
    Vector3 effectorTarget, tipTarget;

    void Reset()
    {
        pivot = transform;
        try
        {
            upper = pivot.GetChild(0);
            lower = upper.GetChild(0);
            effector = lower.GetChild(0);
            tip = effector.GetChild(0);
        }
        catch (UnityException)
        {
            Debug.Log("Could not find required transforms, please assign manually.");
        }
    }

    void Awake()
    {
        upperLength = (lower.position - upper.position).magnitude;
        lowerLength = (effector.position - lower.position).magnitude;
        effectorLength = (tip.position - effector.position).magnitude;
        pivotLength = (upper.position - pivot.position).magnitude;

        if (targetTransform != null)
        {
            // Ensure the targetTransform has a Rigidbody
            targetRigidbody = targetTransform.GetComponent<Rigidbody>();
            if (targetRigidbody == null)
            {
                targetRigidbody = targetTransform.gameObject.AddComponent<Rigidbody>();
            }

            // Configure the Rigidbody
            targetRigidbody.isKinematic = false;
            targetRigidbody.useGravity = false;
        }
    }

    void Update()
    {
        if (targetTransform == null || targetRigidbody == null)
        {
            Debug.LogWarning("Target Transform or Rigidbody is not assigned.");
            return;
        }

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        float moveUpDown = Input.GetAxis("UpDown");

        // Calculate movement vector
        Vector3 movement = new Vector3(moveHorizontal, moveUpDown, moveVertical) * speed;

        // Apply movement using Rigidbody
        targetRigidbody.velocity = movement;

        // Constrain the targetTransform to stay within maxDistanceFromTip
        float distanceFromTip = Vector3.Distance(targetTransform.position, tip.position);

        //if (distanceFromTip > maxDistanceFromTip)
        //{
        //    // Calculate direction towards the tip
        //    Vector3 direction = (tip.position - targetTransform.position).normalized;

        //    // Move the targetTransform closer to the tip
        //    Vector3 newPosition = targetTransform.position + direction * (distanceFromTip - maxDistanceFromTip);

        //    // Apply the new position to the Rigidbody
        //    targetRigidbody.MovePosition(newPosition);

        //    // Stop movement if constrained
        //    targetRigidbody.velocity = Vector3.zero;
        //}

        // Update effector and tip targets
        tipTarget = targetTransform.position;
        effectorTarget = tipTarget + normal * effectorLength;

        // Solve the IK
        Solve();
    }


    void Solve()
    {
        var pivotDir = effectorTarget - pivot.position;
        pivot.rotation = Quaternion.LookRotation(pivotDir);

        var upperToTarget = (effectorTarget - upper.position);
        var a = upperLength;
        var b = lowerLength;
        var c = upperToTarget.magnitude;

        var B = Mathf.Acos((c * c + a * a - b * b) / (2 * c * a)) * Mathf.Rad2Deg;
        var C = Mathf.Acos((a * a + b * b - c * c) / (2 * a * b)) * Mathf.Rad2Deg;

        if (!float.IsNaN(C))
        {
            // Constrain upper rotation to X-axis only
            var upperRotation = Quaternion.AngleAxis(-B, Vector3.right);
            upper.localRotation = upperRotation;

            var lowerRotation = Quaternion.AngleAxis(180 - C, Vector3.right);
            lower.localRotation = lowerRotation;
        }

        var effectorRotation = Quaternion.LookRotation(tipTarget - effector.position);
        effector.rotation = effectorRotation;
    }

}
