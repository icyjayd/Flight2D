using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
//using Helpers;

public class PlayerController : MonoBehaviour {
    private Vector2 movement;
    private bool dash, charging =false;
    Action lastMove;
    Rigidbody2D rb;
    private CharacterBehavior cb;
    public float inputThreshold = .2f, dashMultiplier = 2f;
    //
    public Queue<Action> inputBuffer;
    public InputMaster controls;
    private void Awake()
    {
        controls = new InputMaster();
        controls.Player.Attack.performed += ctx => Attack();
        controls.Player.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        controls.Player.Charge.started += ctx => Charge();
        controls.Player.Charge.canceled += ctx => EndCharge();
 
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cb = GetComponent<CharacterBehavior>();
        inputBuffer = new Queue<Action>();

    }
    private void Update()
    {


        //updatebuffer();

        //float xmove = CrossPlatformInputManager.GetAxis("Horizontal");
        //if (xmove != 0)
        //{
        //    processinput(new action("xmove", time.time, xmove));


        //}
        //float ymove = CrossPlatformInputManager.GetAxis("Vertical");
        //movement = new Vector2(xmove, ymove);
        //if(ymove != 0)
        //{
        //    processinput(new action("ymove", time.time, ymove));

        //}

        //dash = crossplatforminputmanager.getbutton("dash");
        //if (crossplatforminputmanager.getbuttondown("dash")){
        //    processinput(new action("dash", time.time, crossplatforminputmanager.getbuttondown("dash")));
        //}
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        cb.Move(movement, Dash(dash));

        //rb.MovePosition(((new Vector2 (xMove, yMove) * speedBuffer * Time.fixedDeltaTime) + new Vector2(transform.position.x, transform.position.y)));


    }

    void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    float Dash(bool pressed)
    {
        if (pressed)
        {
            return dashMultiplier;


        }
        else{
            return 1f; 
        }
    }

    void Charge()
    {
        ///moving disables charging and enables dash, charging disables movement and dash
        bool moving = (movement.magnitude != 0);
        if (moving)
        {
            print("dashing");
            dash = true;
            charging = false;
        }
        else
        {
            print("charging");
            dash = false;
            charging = true;

        }
    }

    void EndCharge()
    {
        dash = false;
        charging = false;
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
    public void Attack()
    {
        cb.Attack();

    }

    public void Move(Vector2 dir)
    {
        ///moving disables charging and enables dash, charging disables movement and dash
   //     Debug.Log(dir);
        if (!charging)
        {
            movement = dir;
        }
        else
        {
            movement = Vector2.zero;
        }
    }





}
