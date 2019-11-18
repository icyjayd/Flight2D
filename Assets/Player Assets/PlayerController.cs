using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


public class PlayerController : MonoBehaviour {
    [SerializeField]
    private float speedBuffer = 10;
    private float xMove, yMove;
    private bool dash;
    Rigidbody2D rb;


    void Start () {
        rb = GetComponent<Rigidbody2D>();
 
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
        rb.velocity = new Vector2(xMove, yMove) * speedBuffer * Dash(dash) * Time.fixedDeltaTime;

        //rb.MovePosition(((new Vector2 (xMove, yMove) * speedBuffer * Time.fixedDeltaTime) + new Vector2(transform.position.x, transform.position.y)));


    }




}
