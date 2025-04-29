using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Obstacle Settings")]
    public GameObject[] obstacles;
    
    [System.Serializable]
    public class PickupSpawnInfo
    {
        public GameObject pickupPrefab;
        [Range(0f, 1f)] public float spawnChance;  // 0 = never, 1 = always

        [Header("Stage-Limit")]
        [Min(0)] public int maxPerStage = 999;   // 0 = never, 999 = (practically) unlimited
    }
    
    [Header("Pickup Settings")]
    public List<PickupSpawnInfo> pickups; // All possible pickups with their spawn chances
    Dictionary<GameObject, int> pickupCounts = new Dictionary<GameObject, int>();

    [Header("Spawn Timing")]
    public float minSpawnY;
    public float maxSpawnY;
    public float spawnRate;
    private float lastSpawn;

    private float leftSpawnX;
    private float rightSpawnX;

    [Header("Wind Hazards")]
    public GameObject windPrefab;
    [Range(0f, 0.3f)] public float windChance = 0.10f;
    [SerializeField] float windCooldown = 6f;      // seconds after a gust ends
    [SerializeField] int   maxWindsPerStage = 2;   // cap per scene

    [Header("Difficulty curves")]
    [SerializeField] AnimationCurve pickupChanceCurve = AnimationCurve.Linear(1,1,6,0.4f);
    [SerializeField] AnimationCurve windChanceCurve   = AnimationCurve.Linear(1,0.05f,6,0.2f);

    bool  windRunning;           // is a gust currently active?
    float lastWindEndTime;       // for cooldown check
    int   windsSpawned;          // how many we’ve spawned this stage


    public static ObstacleSpawner instance;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    void Awake()
    {
        instance = this;
        foreach (var p in pickups)
            pickupCounts[p.pickupPrefab] = 0;
    }

    void Start()
    {
        Camera cam = Camera.main;
        float camWidth = (2.0f * cam.orthographicSize) * cam.aspect;

        leftSpawnX = -camWidth / 2;
        rightSpawnX = camWidth / 2;
        spawnRate = PlayerInfo.spawnRate;
    }

    void Update()
    {
        if (Time.time - lastSpawn >= spawnRate)
        {
            lastSpawn = Time.time;
            SpawnSomething();
        }
    }

    void SpawnSomething()
    {
        float pickupScale = PlayerInfo.pickupMultiplier;   // from LevelSettings
        float effectiveWindChance = windChance * PlayerInfo.windMultiplier;

        Vector3 spawnPos = GetSpawnPosition();
        bool spawned = false;

        /* ---------- PICKUP roll (weighted) ---------- */
        float sum = 0;
        foreach (var p in pickups)
        {
            if (pickupCounts[p.pickupPrefab] < p.maxPerStage)
                sum += p.spawnChance * pickupScale;      // still apply difficulty multiplier
        }

        if (Random.value < sum)
        {
            float r = Random.value * sum, cur = 0f;
            foreach (var p in pickups)
            {
                if (pickupCounts[p.pickupPrefab] >= p.maxPerStage) continue; // skip exhausted

                cur += p.spawnChance * pickupScale;
                if (r <= cur)
                {
                    Spawn(p.pickupPrefab, spawnPos, 20f);
                    pickupCounts[p.pickupPrefab]++;          // track usage
                    spawned = true;
                    break;
                }
            }
        }

        /* ---------- WIND roll (independent) ---------- */
        bool canSpawnWind = windPrefab && !windRunning && windsSpawned < maxWindsPerStage && Time.time - lastWindEndTime >= windCooldown;
        if (canSpawnWind && Random.value < effectiveWindChance)
        {
            var w = Spawn(windPrefab, spawnPos, 6f);
            w.GetComponent<WindGust>().direction = Random.value < .5f ? -1 : 1;

            windRunning = true;          // mark active
            windsSpawned++;
            spawned = true;
        }

        /* ---------- Fallback obstacle ---------- */
        if (!spawned)
        {
            var ob = Spawn(obstacles[Random.Range(0, obstacles.Length)], spawnPos, 20f);
            var osc = ob.GetComponent<Obstacle>();
            if (osc) osc.moveDir = new Vector3(ob.transform.position.x > 0 ? -1 : 1, 0, 0);
        }
    }

    public void ResetPickupLimits()
    {
        foreach (var key in pickupCounts.Keys)
            pickupCounts[key] = 0;
        // also reset windsSpawned etc. if you wish
    }

    IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        spawnedObjects.Remove(obj);
        if (obj != null)
            Destroy(obj);
    }

    Vector3 GetSpawnPosition()
    {
        float x = Random.Range(0, 2) == 1 ? leftSpawnX : rightSpawnX;
        float y = Random.Range(minSpawnY, maxSpawnY);

        return new Vector3(x, y, 0);
    }

    GameObject Spawn(GameObject prefab, Vector3 pos, float life)
    {
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        spawnedObjects.Add(go);
        StartCoroutine(DestroyAfterTime(go, life));
        return go;
    }

    public void OnWindEnded()
    {
        windRunning    = false;
        lastWindEndTime = Time.time;
    }

    
}
