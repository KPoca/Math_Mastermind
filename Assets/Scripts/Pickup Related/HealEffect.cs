using UnityEngine;

[CreateAssetMenu(menuName = "Pickup/Heal")]
public class HealEffect : PickupEffect
{
    public int healAmount;
    public override void Apply(PlayerController player)
    {
        player.GetComponent<PlayerStatus>().Heal(healAmount);
        AudioManager.instance?.PlaySFX(AudioManager.instance.healPickup);
    }
}