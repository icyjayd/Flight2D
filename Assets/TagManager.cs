using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour
{
    private static TagManager tm;
    public static TagManager TM { get { return tm; } }
    public string PlayerTag, EnemyTag, WeaponTag, ItemTag;

    private void Awake()
    {
        tm = this;
    }
}
