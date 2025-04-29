using UnityEngine;
using System.Collections;

public class WindGust : MonoBehaviour
{
    [Tooltip("Direction of push: -1 = left, +1 = right")]
    public GameObject windVFXPrefab;   // ← drag WindVFX here
    public int direction = 1;


    [SerializeField] float pushSpeed  = 3f;   // units/second of drift
    [SerializeField] float activeTime = 3f;   // seconds wind lasts
    [SerializeField] float vfxRate     = 0.15f;  // seconds between spawns


    void Awake()
    {

        Vector3 s  = transform.localScale;
        s.x        = Mathf.Abs(s.x) * direction;
        transform.localScale = s;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;

        var pc = col.GetComponent<PlayerController>();
        if (pc == null) return;

        pc.StartCoroutine(ApplyWind(pc, activeTime, direction, pushSpeed));
        Destroy(gameObject);                  // consume hazard
    }

    IEnumerator ApplyWind(PlayerController pc, float dur, int dir, float speed)
    {
        pc.windPush = dir * speed;
        
        float t = 0f;
        float nextVFX = 0f;

        while (t < dur)
        {
            // spawn VFX every vfxRate seconds
            if (Time.time >= nextVFX && windVFXPrefab)
            {
                SpawnWindVFX();
                nextVFX = Time.time + vfxRate;
            }

            t += Time.deltaTime;
            yield return null;
        }

        pc.windPush = 0f;
        ObstacleSpawner.instance?.OnWindEnded();       // inform spawner
    }

    void SpawnWindVFX()
    {
        Vector3 viewPos = new Vector3(Random.value, Random.Range(0.2f, 0.8f), 10); // z=10 so it’s in front of camera
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);
        GameObject vfx   = Instantiate(windVFXPrefab, worldPos, Quaternion.identity);

        Vector3 vs = vfx.transform.localScale;
        vs.x       = Mathf.Abs(vs.x) * direction;
        vfx.transform.localScale = vs;

        // auto-destroy when animation ends
        float clipLen = vfx.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        Destroy(vfx, clipLen);
    }
}
