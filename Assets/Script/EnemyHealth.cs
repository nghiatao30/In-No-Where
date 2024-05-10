using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHeathSystem
{
    // Start is called before the first frame update
    [SerializeField] float maxHealth;
    float currentHealth;

    Animator playerAnim;
    void Die()
    {
        Debug.Log("Enemy is dead");
        Destroy(gameObject);

    }
    public void takeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

}
