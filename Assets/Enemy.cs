using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBehavior {

    public Transform playerTransform;
    Vector3 dir;
    // Use this for initialization
    new void Start () {
        base.Start();
        playerTransform = gm.GetPlayerTransform();
	}


    private void Update()
    {
        dir = playerTransform.position.normalized - transform.position.normalized;
    }
    // Update is called once per frame
    void FixedUpdate () {
        Approach();
	}

    public override void FlipCheck(float moveX)
    {
        if (dir.x > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (dir.x < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }
    }


    public virtual void Approach() {
        
        Move(dir.x, dir.y);
    }



    //Right now, nothing is happening. I want the enemy to approach the player and turn around when they are facing away from the player


}
