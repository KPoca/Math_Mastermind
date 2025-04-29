using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerShield : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] Sprite  shieldSprite;
    [SerializeField] Color   glowColour = Color.cyan;
    [SerializeField] float   radius      = 1.2f;
    [SerializeField] string  sortingLayer = "Player Shield";
    [SerializeField] int     sortingOrder = 0;

    SpriteRenderer glow;
    bool hasShield;

    public bool HasShield => hasShield;         

    void Awake()
    {
        var go   = new GameObject("ShieldGlow");
        go.transform.SetParent(transform, false);
        glow     = go.AddComponent<SpriteRenderer>();
        glow.sprite        = shieldSprite;
        glow.sortingLayerName = sortingLayer;
        glow.sortingOrder  = sortingOrder;
        glowColour.a = 50f / 255f;
        glow.color   = glowColour;
        glow.transform.localScale = Vector3.one * radius;

        glow.enabled = false;
    }

    public void Activate()
    {
        if (hasShield) return;
        hasShield   = true;
        glow.enabled = true;
    }

    public bool ConsumeHit()
    {
        if (!hasShield) return false;

        hasShield   = false;
        glow.enabled = false;
        return true;
    }
}
