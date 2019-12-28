using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour {
    private float xMove, yMove;
    private bool dash;
    Rigidbody2D rb;
    private CharacterBehavior cb;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
        cb = GetComponent<CharacterBehavior>();
 
    }
    float Dash(bool pressed)
    {
        if (pressed)
        {
            return 2f;


        }
        else{
            return 1f; 
        }
    }
    private void Update()
    {

        xMove = CrossPlatformInputManager.GetAxis("Horizontal");
        yMove = CrossPlatformInputManager.GetAxis("Vertical");
        dash = CrossPlatformInputManager.GetButton("Dash");
    }
    // Update is called once per frame
    void FixedUpdate() {
        cb.Move(xMove, yMove, Dash(dash));
        //rb.MovePosition(((new Vector2 (xMove, yMove) * speedBuffer * Time.fixedDeltaTime) + new Vector2(transform.position.x, transform.position.y)));


    }




}
