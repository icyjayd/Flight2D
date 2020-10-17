using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SoundInfo : ScriptableObject
{
    //This class contains all the sound and corresponding information, i.e. volume and start/stop times for a character's standard sounds
    public AudioClip comboSound , 
        hitSound;
    public float comboStart = 0f, comboStop =100f, comboVolume=1, 
        hitStart=0f, hitStop=100f, hitVolume=1;

}
