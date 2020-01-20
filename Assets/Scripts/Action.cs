using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public class Maneuver {
//    public enum ActionType { MeleeAttack, DashAttack, BurstAttack, Charge, Dash, Sub, Shield, Ascend, Descend, Interrupt};
//    public ActionType actionType; //type of action being performed/queued
//    [HideInInspector]
//    public float timeSinceLast; //time since last action queued
//    [HideInInspector]
//    public float decayTime;

//    public Maneuver(ActionType action, float t)
//    {
//        actionType = action;
//        timeSinceLast = t;
//        SetDecayTime(.5f);
//    }
//    /// <summary>
//    ///set amount of time for action to remain in potential combo
//    /// </summary>
//    /// <param name="t"> Time within which this action can form a combo</param>
//    public void SetDecayTime(float t)
//    {
//        decayTime = t;
//    }
//}
public class Action : Object
{
    public string Name { get; set; }
    public float Time { get; set; }
    public float Magnitude { get; set; }

    public Action(string name, float time, object magnitude)
    {
        Name = name;
        Time = time;
        Magnitude = System.Convert.ToSingle(magnitude);
    }

    public bool GetBoolMagnitude(float threshold = 0)
    {
        if (Magnitude >= threshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

