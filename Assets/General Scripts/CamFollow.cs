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


    private void Awake()
    {
        //print(SystemInfo.graphicsDeviceName);
        // Setting up the reference.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerRB = player.GetComponent<Rigidbody2D>();
        playerPos = player.position;
        camPos = transform.position;
    }


    private bool CheckXMargin()
    {
        // Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
        return Mathf.Abs(transform.position.x - player.position.x) > xMargin;
    }


    private bool CheckYMargin()
    {
        // Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
        return Mathf.Abs(transform.position.y - player.position.y) > yMargin;
    }

    private void Update()
    {



    }
    private void FixedUpdate() {
        camPos = transform.position;
        playerPos = player.position;


    }
    private void LateUpdate()
    {

        TrackPlayer();

    }

    private void Follow() {
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
    }
    private void TrackPlayer()
    {
        // By default the target x and y coordinates of the camera are it's current x and y coordinates.
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        // If the player has moved beyond the x margin...
        if (CheckXMargin())
        {
            // ... the target x coordinate should be a Lerp between the camera's current x position and the player's current x position.
            targetX = Mathf.Lerp(camPos.x, playerPos.x, xSmooth * Time.fixedDeltaTime);
        }

        // If the player has moved beyond the y margin...
        if (CheckYMargin())
        {
            // ... the target y coordinate should be a Lerp between the camera's current y position and the player's current y position.
            targetY = Mathf.Lerp(camPos.y, playerPos.y, ySmooth * Time.fixedDeltaTime);
        }

        // The target x and y coordinates should not be larger than the maximum or smaller than the minimum.
        targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
        targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
       
        // Set the camera's position to the target position with the same z component.
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
    private void TrackPlayerOld() {

        Vector3 point = Camera.main.WorldToViewportPoint(playerPos);
        Vector3 delta = playerPos - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, smoothTime);

    }
}