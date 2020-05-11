using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerTransform;
    Vector2 dir;
    Vector2 dist;
    private float moveX = 0, moveY = 0;
    private bool watching = true;
    [SerializeField]
    private float minDist = 1;
    [SerializeField]
    private float acceleration = 1.1f;
    CharacterBehavior cb;

    /// possibly temporary variables
    float approachTime = 0;
    // Use this for initialization
    void Start()
    {
        cb = GetComponent<CharacterBehavior>();
        playerTransform = cb.gm.GetPlayerTransform();
    }

    // Update is called once per frame
    void Update()
    {
        approachTime += Time.deltaTime;
        if (approachTime % 10 < 5)
        {
            ApproachPlayer();
        }
    }
    private void ApproachPlayer()
    {
        //pass a "normalized input" calculated based on the player's position vs. this object's position
        dir = playerTransform.position - transform.position;
        dir = dir.normalized;
       // print(dir);
        //if(dir.x <0 && cb.facingRight)//if you're faicng left and the player is to your left...
        //{
        //    cb.Flip();
        //}
        //else if(dir.x >0 && !cb.facingRight)
        //{
        //    cb.Flip();
        //}
        cb.Move(dir);
        

    }
}
