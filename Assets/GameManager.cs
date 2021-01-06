using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager gm;
    public static GameManager GM { get { return gm; } }
    public CharacterBehavior playerCB;
    public Transform playerTransform;
	// Use this for initialization
	void Awake () {
        playerCB = GameObject.FindObjectOfType<CharacterBehavior>();
        QualitySettings.vSyncCount = 0;
        if (gm != null && gm != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            gm = this;
        }
        playerTransform = playerCB.transform;
	}

    public Transform GetPlayerTransform() {
        return playerCB.transform;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
