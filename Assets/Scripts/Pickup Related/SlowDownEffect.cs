using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Pickup/SlowDown")]
public class SlowDownEffect : PickupEffect
{
    [Range(0.1f,1f)] public float slowFactor   = 0.5f;
    public float                slowDuration = 5f;

    static bool      isSlowed;
    static Coroutine activeRoutine;
    public static bool  IsActive      => isSlowed;
    public static float CurrentFactor => isSlowed ? activeFactor : 1f;

    static float activeFactor = 1f;

    public override void Apply(PlayerController player)
    {
        ScreenFlashFX.instance?.Flash();

        var host = ObstacleSpawner.instance;             
        if (host == null) { Debug.LogWarning("No ObstacleSpawner in scene!"); return; }

        /* already slowed? –> just restart timer */
        if (activeRoutine != null) host.StopCoroutine(activeRoutine);

        /* first pick-up in current slow window: apply factor */
        if (!isSlowed)
        {
            foreach (var ob in FindObjectsOfType<Obstacle>())
                ob.moveSpeed *= slowFactor;

            foreach (var pm in FindObjectsOfType<PickupMover>())
                pm.moveSpeed *= slowFactor;

            activeFactor = slowFactor;     // ❶  remember factor
            isSlowed     = true;
        }

        activeRoutine = host.StartCoroutine(RestoreAfterDelay());
        AudioManager.instance?.PlaySFX(AudioManager.instance.slowDownPickup);
    }

    IEnumerator RestoreAfterDelay()
    {
        yield return new WaitForSeconds(slowDuration);

        foreach (var ob in FindObjectsOfType<Obstacle>())
            ob.moveSpeed /= slowFactor;

        foreach (var pm in FindObjectsOfType<PickupMover>())
            pm.moveSpeed /= slowFactor;

        isSlowed      = false;
        activeFactor  = 1f;                // ❷  back to normal
        activeRoutine = null;
    }
}
