using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using Helpers;

public class PlayerController : MonoBehaviour {
    private float xMove, yMove;
    private bool dash;
    Action lastMove;
    Rigidbody2D rb;
    private CharacterBehavior cb;
    public float inputThreshold = .2f;
    [SerializeField]
    public Queue<Action> inputBuffer;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        cb = GetComponent<CharacterBehavior>();
        inputBuffer = new Queue<Action>();
 
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
    void ProcessInput(Action action)//takes all input and adds it to the queue, catching held movement and new movement;
    {
        inputBuffer.Enqueue(action);
    }
    void UpdateBuffer ()
    {
        if (inputBuffer.Count > 0)
        {
            while (true)
            {
                if (Time.time - inputBuffer.Peek().Time > inputThreshold)
                {
                    inputBuffer.Dequeue();
                    if(inputBuffer.Count <= 0)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }
    private void Update()
    {
        UpdateBuffer();

        xMove = CrossPlatformInputManager.GetAxis("Horizontal");
        if (xMove != 0)
        {
            ProcessInput(new Action("xMove", Time.time, xMove));


        }
        yMove = CrossPlatformInputManager.GetAxis("Vertical");
        if(yMove != 0)
        {
            ProcessInput(new Action("yMove", Time.time, yMove));

        }

        dash = CrossPlatformInputManager.GetButton("Dash");
        if (CrossPlatformInputManager.GetButtonDown("Dash")){
            ProcessInput(new Action("dash", Time.time, CrossPlatformInputManager.GetButtonDown("Dash")));
        }
    }
    // Update is called once per frame
    void FixedUpdate() {
        cb.Move(xMove, yMove, Dash(dash));
        //rb.MovePosition(((new Vector2 (xMove, yMove) * speedBuffer * Time.fixedDeltaTime) + new Vector2(transform.position.x, transform.position.y)));


    }




}
