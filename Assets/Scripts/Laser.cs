using UnityEngine;

public class Laser : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Collider2D hitboxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        hitboxCollider.enabled = false;
    }

    public void EnableHitbox()
    {
        hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        hitboxCollider.enabled = false;
    }

    public void DestroyLaser()
    {
        Destroy(gameObject);
    }
}
