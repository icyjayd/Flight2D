using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputBuffer
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

{
    Queue<InputFrame> frameInputs;
    int capacity;
    public InputBuffer(int size)
    {
        capacity = size;
        frameInputs = new Queue<InputFrame>(size);

    }
    public void AddFrame(InputFrame frame)
    {
       if(frameInputs.Count == capacity)
        {
            frameInputs.Dequeue();
        }
        frameInputs.Enqueue(frame);
    }
}
