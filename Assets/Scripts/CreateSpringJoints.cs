using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSpringJoints : MonoBehaviour
{
    public float spring = 50.0f;
    public float damper = 5.0f;
    public float minDistance = 0.0f;
    public float maxDistance = 0.0f;
    public bool configureConnectedAnchor = false;

    SpringJoint[] springJoints;

    // Start is called before the first frame update
    void Start()
    {
        CreateJoints();
        IgnoreChildCollisions();
        springJoints = GetComponentsInChildren<SpringJoint>();
    }

    void IgnoreChildCollisions()
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

        // Attach children to each other
        foreach (Transform child in childTransforms)
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

        foreach (Transform child in childTransforms)
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
            foreach (SpringJoint springJoint in springJoints)
            {
                // Update the spring properties in real time
                springJoint.spring = spring;
                springJoint.damper = damper;
                springJoint.minDistance = minDistance;
                springJoint.maxDistance = maxDistance;
                springJoint.autoConfigureConnectedAnchor = configureConnectedAnchor;
            }
        }
    }
}
