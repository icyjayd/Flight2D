using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CharacterBehavior))]
public class PlayerController : MonoBehaviour {

    private CharacterBehavior character; //character controller attached to player object
    [HideInInspector]
    public enum PlayerState { Idle, Moving, Attacking, Dashing, Charging, Shielding, Stunned };
    public enum PlayerButton { Attack, Charge, Shield, Sub, MoveX, MoveUp, MoveDown, NONE };
    public PlayerState currentState;
    public PlayerButton currentButton;
    public Queue<Action> actionQueue;
    float timeSinceLastAction;
    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterBehavior>();
        actionQueue = new Queue<Action>();
        timeSinceLastAction = 0;
	}

    private void Update()
    {
        timeSinceLastAction += Time.deltaTime;
    }
    // Update is called once per frame
    void FixedUpdate() {
        character.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
            Input.GetAxis("Dash") < 0 ? true : false);
        //print("DASH: " + CrossPlatformInputManager.GetAxis("Dash"));

    }
    /// <summary>
    /// Get an action based on the current state of the player and the button pressed
    /// </summary>
    /// <param name="previousState">State to be compared to</param>
    /// <param name="button">Button pressed</param>
    /// <returns></returns>
    Action getAction(PlayerState previousState, PlayerButton button)
    {
        if (button == PlayerButton.Attack)
        {
            if (previousState == PlayerState.Idle || previousState == PlayerState.Moving)
            {
                return new Action(Action.ActionType.MeleeAttack, timeSinceLastAction);
            }
            else if (previousState == PlayerState.Dashing)
            {
                return new Action(Action.ActionType.DashAttack, timeSinceLastAction);
            }
            else if (previousState == PlayerState.Attacking)
            {
                return new Action(Action.ActionType.MeleeAttack, timeSinceLastAction);


            }
            else if (previousState == PlayerState.Charging)
            {
                return new Action(Action.ActionType.BurstAttack, timeSinceLastAction);

            }
        }
        else if (button == PlayerButton.Charge)
        {
            if (previousState == PlayerState.Moving)
            {

                return new Action(Action.ActionType.Dash, timeSinceLastAction);

            }
            else if (previousState == PlayerState.Idle)
            {
                return new Action(Action.ActionType.Charge, timeSinceLastAction);
            }
            else
            {
                return null;
            }
        }
        else if (button == PlayerButton.Sub)
        {
            if (previousState == PlayerState.Idle || previousState == PlayerState.Moving)
            {
                return new Action(Action.ActionType.Sub, timeSinceLastAction);

            }

        }
        else if (button == PlayerButton.Shield)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Shield, timeSinceLastAction);
            }
        }
        else if (button == PlayerButton.MoveUp)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Ascend, timeSinceLastAction);
            }
        }
        else if (button == PlayerButton.MoveDown)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Descend, timeSinceLastAction);
            }
        }

        return null;
    }

    //get buttons
    //compare button to state
    //add action to queue
    //read action queue
    //execute action
    //update state
   
}
