using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupEffect effect;

    void OnTriggerEnter2D (Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (effect == null)
        {
            Debug.LogWarning($"{name} collected with no effect assigned!");
            Destroy(gameObject);
            return;
        }

        effect.Apply(col.GetComponent<PlayerController>());
        Destroy(gameObject);
    }
}

