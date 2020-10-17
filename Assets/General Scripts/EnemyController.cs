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
    private float meleeRangeThresholdX = 1;
    [SerializeField]
    private float acceleration = 1.1f;
    [SerializeField]
    private float midRangeThresholdX = 5, maxYDist = 0.5f;
    private float pointBlankRange = 0.7f;//TODO: implement this
    CharacterBehavior cb;
    [SerializeField]
    RangeState rangeState;

    /// possibly temporary variables
    bool aggression = false; //might not use

    LineSegment path;
    float approachTime = 0;
    public float playerDistX, playerYDist;
    public float attackOdds = 0.7f;
    public float attackBias = 0f;//used to diminish the odds of too many successive attacks
    //General notes:
    //RANGES
    //Long range behavior of the basic baddy: approach in "travel mode" then transition to mid range mode 
    //Mid range: try to bait player into attacking with dodge patterns, leaving the player to whiff
    //Melee range: defined as the range in which a dash attack has a high chance of success unless there is a dedicated dodge or shield
    //Point blank range: defined as the range in which a melee attack will definitely hit its target unless shielded
    void Start()
    {
        cb = GetComponent<EnemyBehavior>();
        playerTransform = cb.gm.GetPlayerTransform();
        path = GetComponentInChildren<LineSegment>();
        aggression = (Random.Range(0, 1) <= attackOdds);
        StartCoroutine(Approach());
        
        
    }
    

    // Update is called once per frame
    void Update()
    {
        playerDistX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        playerYDist = Mathf.Abs(transform.position.y - playerTransform.position.y);
        rangeState = GetState();
        //approachTime += Time.deltaTime;
        //if (approachTime % 10 < 5)
        //{
        //    ApproachPlayer();
        //}
    }
    private void FixedUpdate()
    {

  
            cb.Move(dir);
        
    }
    RangeState GetState()
    {
        if (playerDistX > midRangeThresholdX || playerYDist > maxYDist)
        {
            return RangeState.Long;
        }
        else if (playerDistX > meleeRangeThresholdX)
        {
            return RangeState.Mid;
        }
        else if (playerDistX > pointBlankRange)
        {
            return RangeState.Short;
        }
        else
        {
            return RangeState.PointBlank;
        }
    }
    bool Aggression(float odds)
    {
        return (Random.Range(0, 1) <= odds);
    }
    private void ApproachPlayer()
    {
        //pass a "normalized input" calculated based on the player's position vs. this object's position
        if (playerYDist > maxYDist)
        {
            dir = new Vector2(0, playerTransform.position.y - transform.position.y);
        }
        else if (playerDistX > meleeRangeThresholdX)
        {

            dir = playerTransform.position - transform.position;
        }
        else
        {
            dir = Vector3.zero;
        }
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



//        cb.Move(dir);
    }


    IEnumerator Approach(bool attacking = false)
    {
        float dist = (playerDistX >midRangeThresholdX) ? (midRangeThresholdX - (midRangeThresholdX- meleeRangeThresholdX)/2) : meleeRangeThresholdX;//if attacking, close the gap; otherwise, approach to regular range
       
        while (playerDistX > dist || playerYDist > maxYDist)
        {
            //print("approaching");

            ApproachPlayer();
            yield return null;
        }
        if (dist == meleeRangeThresholdX)
        {
            StartCoroutine(Attack());
            yield break;
        }
        else
        {
            StartCoroutine(FollowPath(path));
            yield break;
        }

    }


    void AttackOrMove()
    {
        if (Aggression(attackOdds -attackBias))
        {
            attackBias +=  0.1f;
            StartCoroutine(Attack());
        }
        else
        {
           // print("moving");
            attackBias -=   0.1f;
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator Attack()
    {
        dir = Vector3.zero;
        
        float elapsedTime = 0;

        yield return new WaitForSeconds(Time.deltaTime * 2);
        int lim = 3;
        int i = 0;
       
        while(i <lim)
        {

            if (!cb.anim.GetBool("Attack"))
            {
                i += 1;
                cb.anim.SetTrigger("Attack");
                yield return null;
            }
            if (playerDistX <= meleeRangeThresholdX || playerDistX > midRangeThresholdX || playerYDist > maxYDist)//if the player is in range at the end, begin attacking
            {
                //movement = Vector3.zero;
                StartCoroutine(Approach());
                yield break;

            }
            elapsedTime += Time.deltaTime;
            yield return null;

        }
       

        AttackOrMove();
    }
    IEnumerator FollowPath(LineSegment lineSegment)
    {
        List < Vector2 > points = lineSegment.points;
        float totalDuration = 0;
        
        for (int i = 0; i < lineSegment.durations.Count; i++)
        {
            totalDuration += lineSegment.durations[i];
        }
        Vector2 startPos = transform.position;
        int j = 0;
        float elapsedTime = 0;
        float pastDurationSum = 0;
        while (elapsedTime <= totalDuration)
        {
           // print("following movement path");

            elapsedTime += Time.deltaTime;
            //if the difference between the elapsed time and the total duration of each segment thus far is greater than the current duration
            //switch to the next duration
            if (elapsedTime - pastDurationSum >= lineSegment.durations[j])
            {
                pastDurationSum += lineSegment.durations[j];
                j = Mathf.Clamp(j + 1, 0, lineSegment.durations.Count-1);
 

                startPos = transform.position;
            }
            //Vector2 movement = Vector2.Lerp(startPos, startPos + lineSegment.points[j], (elapsedTime - pastDurationSum) / lineSegment.durations[j]);
            //Vector2 movement = lineSegment.points[j];
            //ApproachPlayer();


            //if (!cb.facingRight && points[j].x == lineSegment.points[j].x) 
            //{
            //    points[j] = new Vector2(points[j].x * -1, points[j].y);
            //}
            //ApproachPlayer();
            dir = points[j];
            //dir = movement - startPos;

             

            yield return null;
        }
        if (playerDistX <= meleeRangeThresholdX || playerDistX > midRangeThresholdX || playerYDist > maxYDist)//if the player is in range at the end, begin attacking
        {
            dir = Vector3.zero;
            //movement = Vector3.zero;
            StartCoroutine(Approach());
            yield break;

        }
        //else if (playerXDist > maxXDist)
        //{
        //    dir = Vector3.zero;
        //    StartCoroutine(Approach());
        //    yield break;

        //}
        AttackOrMove();
    }
}
