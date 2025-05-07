using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketBleed : MonoBehaviour
{

    [Header("Assign the Original Organ to this")]
    [SerializeField] public GameObject socketBleedOrgan;

    [Header("Assign the Particle System prefab to this")]
    [SerializeField] private GameObject socketBleedParticleSystem;

    private bool isBleeding = false;
    private Coroutine bleedCoroutine;
    private List<GameObject> instantiatedBleeds = new List<GameObject>();
    private Coroutine organContactCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NewOrgan"))
        {
            Debug.Log("NewOrgan entered the trigger");
            if (organContactCoroutine == null)
            {
                organContactCoroutine = StartCoroutine(HandleOrganContact(other)); //If a new organ stays for 1 second, make it kinematic & stop bleed
            }
        }
        if (other.CompareTag("GrabbableObject"))
        {
            Debug.Log("GrabbableObject entered the trigger");
            if (other.name == "Saw(Clone)" || other.name == "Scalpel(Clone)" || other.name == "Hammer(Clone)")
            {
                Debug.Log("GrabbableObject is a tool");
                if (!isBleeding)
                {
                    Debug.Log("Bleeding started");
                    isBleeding = true;
                    bleedCoroutine = StartCoroutine(SpawnBleedParticles());
                }

                Quaternion randomRotation = Random.rotation; //Spray blood in a random direction
                GameObject particleInstance = Instantiate(socketBleedParticleSystem, transform.position, randomRotation);
                particleInstance.transform.SetParent(transform.parent.parent, true);
                instantiatedBleeds.Add(particleInstance); //Keep track so we can delete it later
            }
            else if (other.transform.parent != null && other.transform.parent.CompareTag("NewOrgan"))
            {
                Debug.Log("GrabbableObject is a NewOrgan");
                organContactCoroutine = StartCoroutine(HandleOrganContact(other.transform.parent.GetComponent<Collider>()));
            }
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

    private IEnumerator SpawnBleedParticles()
    {
        while (isBleeding)
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
