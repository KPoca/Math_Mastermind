using UnityEngine;
using TMPro;

public class PlayerStatus : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth = 3;

    [SerializeField] TextMeshProUGUI healthText;

    // Update is called once per frame
    void Start()
    {
        currentHealth = 3;
        UpdateHealthText();
    }

    void UpdateHealthText()
    {
        healthText.text = "x " + currentHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateHealthText();
        ScreenShake.instance?.Shake();
        if (currentHealth <= 0) Die();
    }

    public void Heal(int amount) {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHealthText();
    }

    void Die()
    {
        Debug.Log("Player has died!");
        GameManager.instance.Lose();
    }
}
