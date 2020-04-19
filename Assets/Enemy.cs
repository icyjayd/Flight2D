using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : CharacterBehavior {

    public Transform playerTransform;
    Vector3 dir;
    Vector3 dist;
    private float moveX = 0, moveY = 0;
    private bool watching = true;
    [SerializeField]
    private float minDist = 1;
    [SerializeField]
    private float acceleration = 1.1f;
    // Use this for initialization
    new void Start () {
        base.Start();
        playerTransform = gm.GetPlayerTransform();
	}


    private void Update()
    {
        dir = playerTransform.position - transform.position;
        if (HelperFunctions.CheckBoundary(dir.x, minDist) == false)
        {
            moveX = Mathf.Clamp(moveX + HelperFunctions.Sign(dir.x) * Time.deltaTime * acceleration,xSpeed * -1, xSpeed);
        }
        else
        {
            moveX = 0;
        }
        if (HelperFunctions.CheckBoundary(dir.y, minDist) == false)
        {
            moveY = Mathf.Clamp(moveY + HelperFunctions.Sign(dir.y) * Time.deltaTime * acceleration, ySpeed * -1, ySpeed);
        }
        else
        {
            moveY = 0;
        }
        // print(moveX);
    }
    // Update is called once per frame
    void FixedUpdate () {
        //Move(moveX, moveY);



    }

    public override void FlipCheck(float moveX)
    {
        if (watching)
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
        
    }


    public virtual void Approach() {
        
    }



    //Right now, nothing is happening. I want the enemy to approach the player and turn around when they are facing away from the player


}
