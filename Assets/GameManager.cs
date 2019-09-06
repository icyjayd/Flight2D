using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager gm;
    public PlayerController player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindObjectOfType<PlayerController>();
        QualitySettings.vSyncCount = 0;
        gm = this;
	}

    public Transform GetPlayerTransform() {
        return player.transform;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
