using Interfaces;
using Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour, ICollidable
{
    [SerializeField] int maxHealth = 100;
    [SerializeField] Slider healthBar;
    [SerializeField] int damageTaken = 5;
    int health;
    // Start is called before the first frame update
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
    }

    public void OnCollision(Projectile source) 
    {
        //if (source is BeachBall)
        //    return;

        this.TakeDamage(this.damageTaken);

        source.Despawn();
    }
}
