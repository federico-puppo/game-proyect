using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public float maxHealth = 100f;
    public Image healthBar;
    private float currentHealth;
    private Animator animator;
    private bool isDead = false;

    public enum CharacterType { Player, Enemy }
    public CharacterType characterType;

    public string winSceneName = "FinalConfrontacionWin";
    public string loseSceneName = "FinalConfrontacionLose";

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthBar();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // Espera unos segundos para cargar la escena
        Invoke(nameof(HandleDeathScene), 2f);
    }

    private void HandleDeathScene()
    {
        if (characterType == CharacterType.Player)
            UnityEngine.SceneManagement.SceneManager.LoadScene(loseSceneName);
        else if (characterType == CharacterType.Enemy)
            UnityEngine.SceneManagement.SceneManager.LoadScene(winSceneName);
    }

}

