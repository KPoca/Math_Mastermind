using UnityEngine;

public class LaserSpawner : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform[] spawnPoints;

    public float minDelay = 5f;   
    public float maxDelay = 10f;   

    float nextSpawnIn;            

    void Start()
    {
        ScheduleNext();           
    }

    void Update()
    {
        if (nextSpawnIn <= 0f)
        {
            SpawnLaser();
            ScheduleNext();
        }
        else
        {
            nextSpawnIn -= Time.deltaTime;   
        }
    }

    void ScheduleNext()
    {
        nextSpawnIn = Random.Range(minDelay, maxDelay);
    }

    void SpawnLaser()
    {
        if (laserPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("LaserSpawner: prefab or spawn points missing!");
            return;
        }

        int idx = Random.Range(0, spawnPoints.Length);
        Instantiate(laserPrefab, spawnPoints[idx].position, Quaternion.identity);
    }
}
