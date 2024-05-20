using Interfaces;
using Projectiles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, ICollidable
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] int damageTaken = 5;
    int health;

    void Start()
    {
        this.health = this.maxHealth;
        this.healthBar.maxValue = this.maxHealth;
        this.healthBar.value = this.health;
    }
    public void TakeDamage(int damage)
    {
        this.health -= damage;
        this.healthBar.value = this.health;

        if (this.health <= 0)
        {
            PlayerPrefs.SetString("LevelToLoad", "MainMenuScreen");
            PlayerPrefs.Save();

            SceneManager.LoadScene("LoadingScreen");
        }
    }

    public void OnCollision(Projectile source) 
    {
        this.TakeDamage(this.damageTaken);

        source.Despawn();
    }
}
