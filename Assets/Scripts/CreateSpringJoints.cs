using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpringJoints : MonoBehaviour
{
    [Header("Spring Joint Settings")]
    [SerializeField] private float spring = 50.0f;
    [SerializeField] private float damper = 5.0f;
    [SerializeField] private float minDistance = 0.0f;
    [SerializeField] private float maxDistance = 0.0f;
    [SerializeField] private bool configureConnectedAnchor = true;

    private SpringJoint[] springJoints;

    void Start()
    {
        CreateJoints();
        IgnoreChildCollisions();
        springJoints = GetComponentsInChildren<SpringJoint>();
    }

    void IgnoreChildCollisions() //Stops the organs exploding when their internal colliders overlap
    {
        Collider[] childColliders = GetComponentsInChildren<Collider>(true);

        for (int i = 0; i < childColliders.Length; i++)
        {
            for (int j = i + 1; j < childColliders.Length; j++)
            {
                Physics.IgnoreCollision(childColliders[i], childColliders[j]);
            }
        }
    }

    void CreateJoints()
    {
        Transform[] children = GetComponentsInChildren<Transform>(true);
        List<Transform> childTransforms = new List<Transform>();

        foreach (Transform child in children)
        {
            if (child.parent == transform)
            {
                childTransforms.Add(child);
            }
        }

        foreach (Transform child in childTransforms) // Attach children to each other
        {
            foreach (Transform target in childTransforms)
            {
                if (child != target)
                {
                    SpringJoint springJoint = child.gameObject.AddComponent<SpringJoint>();
                    springJoint.connectedBody = target.GetComponent<Rigidbody>();
                    springJoint.spring = spring;
                    springJoint.damper = damper;
                    springJoint.minDistance = minDistance;
                    springJoint.maxDistance = maxDistance;
                    springJoint.autoConfigureConnectedAnchor = configureConnectedAnchor;
                }
            }
        }

        foreach (Transform child in childTransforms) //Attach the parent to the child
        {
            SpringJoint parentSpringJoint = this.gameObject.AddComponent<SpringJoint>();
            parentSpringJoint.connectedBody = child.GetComponent<Rigidbody>();
            parentSpringJoint.spring = spring * 2.0f; //Extra strong, for general rigidity
            parentSpringJoint.damper = damper;
            parentSpringJoint.minDistance = minDistance;
            parentSpringJoint.maxDistance = maxDistance;
            parentSpringJoint.autoConfigureConnectedAnchor = configureConnectedAnchor;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild) //Only run this in debug builds
        {
            foreach (SpringJoint springJoint in springJoints) //For debugging the organ jiggle
            {
                springJoint.spring = spring;
                springJoint.damper = damper;
                springJoint.minDistance = minDistance;
                springJoint.maxDistance = maxDistance;
                springJoint.autoConfigureConnectedAnchor = configureConnectedAnchor;
            }
        }
    }
}
