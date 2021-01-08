using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float health = 100, maxHealth = 100;
    public GameObject deathSprite;
    DeathAnimation deathSpriteObject;
    // Use this for initialization
	void Start () {
        deathSpriteObject = (Instantiate(deathSprite) as GameObject).GetComponent<DeathAnimation>();
        deathSpriteObject.gameObject.SetActive(false);
	}

    public void TakeDamage(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if(health <= 0) {
            Die();

        }

    }
    void Die()
    {

        print("dying");
        deathSpriteObject.gameObject.SetActive(true);
        deathSpriteObject.transform.position = transform.position;
        deathSpriteObject.Invoke("Deactivate", 0.25f);
        gameObject.SetActive(false);

    }
}
