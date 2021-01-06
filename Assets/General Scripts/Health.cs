using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float health = 100, maxHealth = 100;
	// Use this for initialization
	void Start () {
		
	}

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        
    }
	void Die()
    {
        if(health <= 0)
        {

        }
    }
}
