using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMeta
{
    public SoundMeta(AudioClip clip, float start = 0, float stop = 0, float volume = 0, float pitch = 0)
    {
        Clip = clip;
        Start = start;
        Stop = stop;
        Volume = volume;
        Pitch = pitch;
    }
    public float Start { get; set; }
    public float Stop { get; set; }
    public float Volume { get; set; }
    public float Pitch { get; set; }
    public AudioClip Clip { get; set; }
}
