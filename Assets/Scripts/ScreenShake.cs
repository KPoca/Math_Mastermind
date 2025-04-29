using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake instance;      // quick singleton
    [Header("Shake settings")]
    [SerializeField] float duration  = 0.15f;
    [SerializeField] float magnitude = 0.25f;

    Vector3 startPos;

    void Awake()
    {
        instance  = this;
        startPos  = transform.localPosition;
    }

    public void Shake(float dur = -1f, float mag = -1f)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(dur < 0 ? duration : dur,
                                    mag < 0 ? magnitude : mag));
    }

    IEnumerator ShakeRoutine(float d, float m)
    {
        float elapsed = 0f;

        while (elapsed < d)
        {
            float offsetX = Random.Range(-1f, 1f) * m;
            float offsetY = Random.Range(-1f, 1f) * m;

            transform.localPosition = startPos + new Vector3(offsetX, offsetY, 0);
            elapsed += Time.unscaledDeltaTime;          // shake even when game paused
            yield return null;
        }
        transform.localPosition = startPos;
    }
}