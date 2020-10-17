using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFrame
{
    ///this class is used to take in all inputs from a single standard gamepad
    /// an inputFrame is comprised of the following variables in order: 
    /// 0 leftStickX, 
    /// 1 leftStickY, 
    /// 2 rightStickX, 
    /// 3 rightStickY, 
    /// 4 dpadX, 
    /// 5 dpadY,
    /// 6 buttonNorth, 
    /// 7 buttonSouth, 
    /// 8 buttonEast, 
    /// 9 buttonWest,
    /// 10 leftShoulder, 
    /// 11 rightShoulder, 
    /// 12 leftTrigger, 
    /// 13 rightTrigger,
    /// 14 startButton, 
    /// 15 selectButton, 
    /// 16 leftStickButton, 
    /// 17 rightStickButton;


    public float[] rawFrameInputs; 
    public InputFrame()
    {
        rawFrameInputs = new float[17];
        
    }
    public Vector2 LeftStick
    {
        get { return new Vector2(rawFrameInputs[0], rawFrameInputs[1]); }
        set { rawFrameInputs[0] = value.x; rawFrameInputs[1] = value.y; }
    }
    public Vector2 RightStick
    {
        get { return new Vector2(rawFrameInputs[2], rawFrameInputs[3]); }
        set { rawFrameInputs[2] = value.x; rawFrameInputs[3] = value.y; }
    }
    public Vector2 Dpad
    {
        get { return new Vector2(rawFrameInputs[4], rawFrameInputs[5]); }
        set { rawFrameInputs[4] = value.x; rawFrameInputs[5] = value.y; }
    }
    public float ButtonNorth
    {
        get { return rawFrameInputs[6]; }
        set { rawFrameInputs[6] = value; }
    }
    public float ButtonSouth
    {
        get { return rawFrameInputs[7]; }
        set { rawFrameInputs[7] = value; }
    }
    public float ButtonEast
    {
        get { return rawFrameInputs[8]; }
        set { rawFrameInputs[8] = value; }
    }
    public float ButtonWest
    {
        get { return rawFrameInputs[9]; }
        set { rawFrameInputs[9] = value; }
    }
    public float LeftShoulder
    {
        get { return rawFrameInputs[10]; }
        set { rawFrameInputs[10] = value; }
    }
    public float RightShoulder
    {
        get { return rawFrameInputs[11]; }
        set { rawFrameInputs[11] = value; }
    }
    public float LeftTrigger
    {
        get { return rawFrameInputs[12]; }
        set { rawFrameInputs[12] = value; }
    }
    public float RightTrigger
    {
        get { return rawFrameInputs[13]; }
        set { rawFrameInputs[13] = value; }
    }
    public float StartButton
    {
        get { return rawFrameInputs[14]; }
        set { rawFrameInputs[14] = value; }
    }
    public float SelectButton
    {
        get { return rawFrameInputs[15]; }
        set { rawFrameInputs[15] = value; }
    }
    public float LeftStickButton
    {
        get { return rawFrameInputs[16]; }
        set { rawFrameInputs[16] = value; }
    }
    public float RightStickButton
    {
        get { return rawFrameInputs[17]; }
        set { rawFrameInputs[17] = value; }
    }






}
