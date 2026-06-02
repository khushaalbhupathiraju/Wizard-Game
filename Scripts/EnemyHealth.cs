using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public float health = 100;
    private float maxHealth;
    public Slider healthBar;
    public GameObject healthBarActivity;

    void Start()
    {
        healthBarActivity.SetActive(false);
        maxHealth = health;
    }

    private void Update()
    {
        healthBar.value = health;
        if(health < maxHealth)
        {
            healthBarActivity.SetActive(true);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
