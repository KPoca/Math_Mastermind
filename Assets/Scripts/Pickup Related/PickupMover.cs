using UnityEngine;

public class PickupMover : MonoBehaviour
{
    public float moveSpeed = 2f;
    private Vector3 moveDir;

    void Start()
    {
        if (SlowDownEffect.IsActive)
        moveSpeed *= SlowDownEffect.CurrentFactor;
        
        moveDir = new Vector3(transform.position.x > 0 ? -1 : 1, 0, 0);
    }

    void Update()
    {
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
}