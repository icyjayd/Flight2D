using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public float xMargin = 1f; // Distance in the x axis the player can move before the camera follows.
    public float yMargin = 1f; // Distance in the y axis the player can move before the camera follows.
    public float xSmooth = 8f; // How smoothly the camera catches up with it's target movement in the x axis.
    public float ySmooth = 8f; // How smoothly the camera catches up with it's target movement in the y axis.
    public Vector2 maxXAndY; // The maximum x and y coordinates the camera can have.
    public Vector2 minXAndY; // The minimum x and y coordinates the camera can have.
    Vector3 velocity = Vector3.zero;
    private Transform player; // Reference to the player's transform.
    Vector3 playerPos;
    Vector3 camPos;

    private Rigidbody2D playerRB;
    public float smoothTime = 1;

    //new camera attempt
    float xDelta;
    float yDelta;

    [SerializeField]
    float startSize;
    [SerializeField]
    float endSize;
    public float maxDistanceDelta;
    private void Awake()
    {
        //print(SystemInfo.graphicsDeviceName);
        // Setting up the reference.
        player = GameManager.GM.playerTransform;
        playerRB = player.GetComponent<Rigidbody2D>();
        playerPos = player.position;
        camPos = transform.position;

    }


    private bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) >= xMargin;
    }


    private bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) >= yMargin;
    }

    private void Update()
    {
        camPos = transform.position;
        playerPos = player.position;


    }
    private void FixedUpdate() {

        xDelta = Mathf.Clamp(playerPos.x-camPos.x, -xMargin, xMargin);
        yDelta = Mathf.Clamp(playerPos.y-camPos.y, -yMargin, yMargin);


    }
    private void LateUpdate()
    {
        //TrackPlayer();

        Follow();



    }

    private void Follow() {



        //The camera should only move when the player is beyond the boundaries 
        //If the player breaches the boundaries, the camera should follow at no more than a distance of 

        float newX = camPos.x ;
        if (CheckXMargin())
        {
            float xSign = (playerPos.x - camPos.x) / (Mathf.Abs(playerPos.x - camPos.x));


            newX = playerPos.x - xMargin * xSign;
        }

        float newY = camPos.y;
        if (CheckYMargin())
        {
            float ySign = (playerPos.y - camPos.y) / (Mathf.Abs(playerPos.y - camPos.y));


            newY = playerPos.y - yMargin * ySign;
        }
        //TODO: add tweening to maxDistanceDelta
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(newX, newY, transform.position.z), maxDistanceDelta * Time.fixedDeltaTime);

        //transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
    }
    private void TrackPlayer()
    {

        //the camera should not move until it is needed to
        //therefore there must be a max delta

        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
        {//get the sign of 
            float xSign = (playerPos.x - camPos.x) / (Mathf.Abs(playerPos.x - camPos.x));

            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = playerPos.x - xMargin * xSign; 

            targetX = Mathf.Lerp(camPos.x  , playerPos.x - xMargin * xSign,  xSmooth * Time.deltaTime);
        }
        //

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
        {
            float ySign = (playerPos.y - camPos.y) / (Mathf.Abs(playerPos.y - camPos.y));
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = playerPos.y - yMargin * ySign;
            targetY = Mathf.Lerp(camPos.y  , playerPos.y - yMargin * ySign , ySmooth * Time.deltaTime);
        }
        //+(playerPos.y - camPos.y)
        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3 (targetX, targetY, transform.position.z);

    }
    private void TrackPlayerOld() {

        Vector3 point = Camera.main.WorldToViewportPoint(playerPos);
        Vector3 delta = playerPos - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);

    }
}