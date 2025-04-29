using UnityEngine;

[CreateAssetMenu(menuName = "Pickup/Shield")]
public class ShieldEffect : PickupEffect
{

    public override void Apply(PlayerController player)
    {
        var ps = player.GetComponent<PlayerShield>();
        if (ps == null) ps = player.gameObject.AddComponent<PlayerShield>();

        if (ps.HasShield)
        {
            return;
        }

        ps.Activate();
        AudioManager.instance?.PlaySFX(AudioManager.instance.shieldPickup);
    }

}
