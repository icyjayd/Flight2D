using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;


[RequireComponent(typeof(CharacterBehavior))]
public class PlayerController : MonoBehaviour {

    private CharacterBehavior character; //character controller attached to player object
    [HideInInspector]
    public enum PlayerState { Idle, Moving, Attacking, Dashing, Charging, Shielding, Stunned };
    public enum PlayerButton { Attack, Dash, Shield, Sub, MoveX, MoveUp, MoveDown, NONE };
    public PlayerState currentState;
    //public PlayerButton currentButton;
    [SerializeField]
    public List<Action> actionList;
    [SerializeField]
    bool detectingInputs = true;
    public List<PlayerButton> buttonList;
    public float timeSinceLastAction;
    Animator animator;
    // Use this for initialization
    void Start () {
        currentState = PlayerState.Idle;
        character = GetComponent<CharacterBehavior>();
        actionList = new List<Action>();
        timeSinceLastAction = 0;
        buttonList = new List<PlayerButton>();
        animator = GetComponent<Animator>();
	}

    private void Update()
    {
        timeSinceLastAction += Time.deltaTime;
        if (detectingInputs)
        {
            buttonList = GetButtons();
        }
        UpdateActions(buttonList);
        //foreach (action a in actionlist)
        //{
        //    print(a.actiontype.tostring() + actionlist.indexof(a));
        //}
    }
    // Update is called once per frame
    void FixedUpdate() {
        character.Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),
            Input.GetAxis("Dash") < 0 ? true : false);
        //print("DASH: " + CrossPlatformInputManager.GetAxis("Dash"));

    }
    

    //get buttons
    //compare button to state
    //add action to queue
    //read action queue
    //execute action
    //update state


        
        /// <summary>
        /// Gets all buttons in the input
        /// </summary>
   
        /// <returns></returns>
    List<PlayerButton> GetButtons()
    {
        List<PlayerButton> newButtons = new List<PlayerButton>();
        if (Input.GetButtonDown("Attack"))
        {
            print("attacking");
            newButtons.Add(PlayerButton.Attack);
        }

        if(Input.GetAxis("Dash") < 0)
        {
            newButtons.Add(PlayerButton.Dash);
        }

        if (Input.GetAxis("Horizontal") != 0)
        {
            newButtons.Add(PlayerButton.MoveX);
        }

        if(Input.GetAxis("Vertical") > .9f)
        {
            newButtons.Add(PlayerButton.MoveUp);
        }
        else if (Input.GetAxis("Vertical") < -0.9f)
        {
            newButtons.Add(PlayerButton.MoveDown);
        }

        if (Input.GetButton("Shield"))
        {
            newButtons.Add(PlayerButton.Shield);
        }

        if (Input.GetButtonDown("Sub"))
        {
            print("sub");

            newButtons.Add(PlayerButton.Sub);
        }

        return newButtons;

    }
      

    /// <summary>
    /// Get an action based on the current state of the player and the button pressed
    /// </summary>
    /// <param name="previousState">State to be compared to</param>
    /// <param name="button">Button pressed</param>
    /// <returns></returns>
    Action GetAction(PlayerState previousState, PlayerButton button)
    {
        float lastInputTime = timeSinceLastAction;
        timeSinceLastAction = 0;

        if (button == PlayerButton.Attack)
        {
            if (previousState == PlayerState.Idle || previousState == PlayerState.Moving)
            {
                return new Action(Action.ActionType.MeleeAttack, lastInputTime);
            }
            else if (previousState == PlayerState.Dashing)
            {
                return new Action(Action.ActionType.DashAttack, lastInputTime);
            }
            else if (previousState == PlayerState.Attacking)
            {
                return new Action(Action.ActionType.MeleeAttack, lastInputTime);


            }
            else if (previousState == PlayerState.Charging)
            {
                return new Action(Action.ActionType.BurstAttack, lastInputTime);

            }
        }
        else if (button == PlayerButton.Dash)
        {
            if (previousState == PlayerState.Moving)
            {

                return new Action(Action.ActionType.Dash, lastInputTime);

            }
            else if (previousState == PlayerState.Idle)
            {
                return new Action(Action.ActionType.Charge, lastInputTime);
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
                return new Action(Action.ActionType.Sub, lastInputTime);

            }

        }
        else if (button == PlayerButton.Shield)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Shield, lastInputTime);
            }
        }
        else if (button == PlayerButton.MoveUp)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Ascend, lastInputTime);
            }
        }
        else if (button == PlayerButton.MoveDown)
        {
            if (previousState != PlayerState.Attacking && previousState != PlayerState.Stunned)
            {
                return new Action(Action.ActionType.Descend, lastInputTime);
            }
        }

        timeSinceLastAction = lastInputTime;
        return null;
    }
    /// <summary>
    /// Adds actions to the list for each button pressed in a given frame
    /// </summary>
    /// <param name="buttons"></param>
    /// <param name="actions"></param>
    void UpdateActions(List<PlayerButton> buttons)
    {
        //clear "dead" actions;
        List<Action> newList = actionList;
        for(int i =0; i < actionList.Count; i++)
        {
            if(timeSinceLastAction >= actionList[i].decayTime)
            {
                newList.Remove(actionList[i]);
            }
        }

        actionList = newList;
        foreach (PlayerButton button in buttonList)
        {
            Action action = GetAction(currentState, button);
            //check if action exists
            if (action != null)
            {
                actionList.Add(action);
                //if action is the same 3 times in a row, remove the 3rd instance as this is probably button mashing

                if (actionList.Count > 2) {
                    int pos = actionList.IndexOf(action);
                    if (actionList[pos-2].actionType == actionList[pos-1].actionType && actionList[pos-1].actionType == actionList[pos].actionType)
                    {

                        actionList.Remove(action);
                    }
                }

            }

        }
    }

    void ExecuteActions()
    {
        switch (actionList[0].actionType)
        {
            case Action.ActionType.MeleeAttack:
                print("melee attack");
                detectingInputs = true;
                break;
            case Action.ActionType.DashAttack:
                print("dash attack");
                break;
            case Action.ActionType.Charge:
                print("Charge");
                break;
            case Action.ActionType.Dash:
                print("dash");
                animator.SetBool("Dashing", true);
                break;
            case Action.ActionType.Ascend:
                if(actionList.Count > 1)
                {
                    if(actionList[1].actionType == Action.ActionType.Dash)
                    {

                    }
                }
                print("ascend");
                break;
            case Action.ActionType.Descend:
                print("descend");
                break;
            case Action.ActionType.Sub:
                print("sub");
                break;
            case Action.ActionType.Interrupt:
                print("interrupt");
                break;

        
            
        }
    }
   
}
