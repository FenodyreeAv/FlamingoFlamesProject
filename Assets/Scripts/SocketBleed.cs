using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBleed : MonoBehaviour
{
    [SerializeField] public GameObject socketBleedOrgan;
    [SerializeField] private GameObject socketBleedParticleSystem;
    private bool isBleeding = false;
    private Coroutine bleedCoroutine;
    private List<GameObject> instantiatedBleeds = new List<GameObject>();
    private Coroutine organContactCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NewOrgan"))
        {
            if (organContactCoroutine == null)
            {
                organContactCoroutine = StartCoroutine(HandleOrganContact(other));
            }
        }
        else
        {
            if (!isBleeding)
            {
                Debug.Log("Bleeding started");
                isBleeding = true;
                bleedCoroutine = StartCoroutine(SpawnBleedParticles());
            }
            Quaternion randomRotation = Random.rotation;
            GameObject particleInstance = Instantiate(socketBleedParticleSystem, transform.position, randomRotation);
            particleInstance.transform.SetParent(transform.parent.parent, true);
            instantiatedBleeds.Add(particleInstance);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NewOrgan") && organContactCoroutine != null)
        {
            StopCoroutine(organContactCoroutine);
            organContactCoroutine = null;
        }

        if (isBleeding && other.CompareTag("Tool"))
        {
            isBleeding = false;
            StopCoroutine(bleedCoroutine);
        }
    }

    private IEnumerator HandleOrganContact(Collider organ)
    {
        yield return new WaitForSeconds(1f);

        if (organ != null && organ.CompareTag("NewOrgan"))
        {
            Debug.Log("Deleting instantiated bleeds due to NewOrgan contact");
            foreach (var bleed in instantiatedBleeds)
            {
                if (bleed != null)
                {
                    Destroy(bleed);
                }
            }
            instantiatedBleeds.Clear();
            Rigidbody organRigidbody = organ.GetComponent<Rigidbody>();
            if (organRigidbody != null)
            {
                organRigidbody.isKinematic = true;
                Debug.Log("Organ's Rigidbody is now kinematic");
            }
        }
    }

    private IEnumerator SpawnBleedParticles()
    {
        while (isBleeding)
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
