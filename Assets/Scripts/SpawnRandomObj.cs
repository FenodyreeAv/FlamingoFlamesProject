using System.Collections.Generic;
using UnityEngine;

 // Ryan
public class SpawnRandomObj : MonoBehaviour
{
    [Header("Total Number of Objects Spawned")]
    [SerializeField] private int heartsToSpawn = 1;
    [SerializeField] private int liversToSpawn = 1;
    [SerializeField] private int lungsToSpawn = 1;
    [SerializeField] private int kidneysToSpawn = 1;

    [SerializeField] private int scalpelsToSpawn = 1;
    [SerializeField] private int hammersToSpawn = 1;
    [SerializeField] private int sawsToSpawn = 1;

    private List<GameObject> availableSpawners;

    [Header("Set These to Object Prefabs")]
    public GameObject[] monKidneys;
    public GameObject[] monLivers;
    public GameObject[] monLungs;
    public GameObject[] monHearts;

    public GameObject[] scalpels;
    public GameObject[] hammers;
    public GameObject[] saws;

    void Start()
    {
        availableSpawners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Spawner"));

        if (availableSpawners.Count == 0)
        {
            Debug.LogError("No spawners found");
            return;
        }

        SpawnObjects(monHearts, heartsToSpawn);
        SpawnObjects(monLivers, liversToSpawn);
        SpawnObjects(monLungs, lungsToSpawn);
        SpawnObjects(monKidneys, kidneysToSpawn);

        SpawnObjects(scalpels, scalpelsToSpawn);
        SpawnObjects(hammers, hammersToSpawn);
        SpawnObjects(saws, sawsToSpawn);
    }

    void SpawnObjects(GameObject[] objectsToSpawn, int count)
    {
        if (objectsToSpawn.Length == 0 || count == 0) return;

        for (int i = 0; i < count; i++)
        {
            if (availableSpawners.Count == 0)
            {
                Debug.LogWarning("No spawners left");
                break;
            }

            int randomObjectIndex = Random.Range(0, objectsToSpawn.Length);
            int randomSpawnerIndex = Random.Range(0, availableSpawners.Count);

            GameObject spawner = availableSpawners[randomSpawnerIndex];
            Instantiate(objectsToSpawn[randomObjectIndex], randomizePosition(spawner.transform.position), randomizeRotation(spawner.transform.rotation));

            availableSpawners.RemoveAt(randomSpawnerIndex); //Remove spawner to avoid reusing it
        }
    }

    Vector3 randomizePosition(Vector3 spawnerPosition)
    {
        Vector3 randomPos = new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
        return spawnerPosition + randomPos;
    }
    Quaternion randomizeRotation(Quaternion spawnerRotation)
    {
        Quaternion randomRot = Quaternion.Euler(0, Random.Range(-15f, 15f), 0);
        return spawnerRotation * randomRot;
    }
}