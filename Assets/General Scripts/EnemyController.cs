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
    private float minXDist = 1;
    [SerializeField]
    private float acceleration = 1.1f;
    [SerializeField]
    private float maxXDist = 5, maxYdist = 0.5f;
    CharacterBehavior cb;

    /// possibly temporary variables
    bool aggression = false; //might not use

    LineSegment path;
    float approachTime = 0;
    private float playerXDist, playerYDist;
    public float attackOdds = 0.7f;
    public float attackBias = 0f;//used to diminish the odds of too many successive attacks
    // Use this for initialization
    void Start()
    {
        cb = GetComponent<CharacterBehavior>();
        playerTransform = cb.gm.GetPlayerTransform();
        path = GetComponentInChildren<LineSegment>();
        aggression = (Random.Range(0, 1) <= attackOdds);
        StartCoroutine(Approach());
    }


    // Update is called once per frame
    void Update()
    {
        playerXDist = Mathf.Abs(transform.position.x - playerTransform.position.x);
        playerYDist = Mathf.Abs(transform.position.y - playerTransform.position.y);

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

    bool Aggression(float odds)
    {
        return (Random.Range(0, 1) <= odds);
    }
    private void ApproachPlayer()
    {
        //pass a "normalized input" calculated based on the player's position vs. this object's position
        if (playerYDist > maxYdist)
        {
            dir = new Vector2(0, playerTransform.position.y - transform.position.y);
        }
        else
        {
            dir = playerTransform.position - transform.position;
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
        print("approaching");
        float dist = (playerXDist >maxXDist) ? (maxXDist - (maxXDist- minXDist)/2) : minXDist;//if attacking, close the gap; otherwise, approach to regular range
       
        while (playerXDist > dist || playerYDist > maxYdist)
        {
            ApproachPlayer();
            yield return null;
        }
        if (dist == minXDist)
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
            print("attacking");
            attackBias +=  0.1f;
            StartCoroutine(Attack());
        }
        else
        {
            print("moving");
            attackBias -=   0.1f;
            StartCoroutine(FollowPath(path));
        }
    }

    IEnumerator Attack()
    {
        float elapsedTime = 0;
        while(elapsedTime <= 1f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (playerXDist <= minXDist || playerXDist > maxXDist || playerYDist > maxYdist)//if the player is in range at the end, begin attacking
        {
            dir = Vector3.zero;
            //movement = Vector3.zero;
            StartCoroutine(Approach());
            yield break;

        }

        AttackOrMove();
    }
    IEnumerator FollowPath(LineSegment lineSegment)
    {
        print("following movement path");
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
            dir = points[j];

             

            yield return null;
        }
        if (playerXDist <= minXDist || playerXDist > maxXDist || playerYDist > maxYdist)//if the player is in range at the end, begin attacking
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
