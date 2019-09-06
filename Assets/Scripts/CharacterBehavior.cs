using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is meant to have actions which are usable by all characters, such as movement, attack, and basic rigidbody initialization.
/// </summary>
public class CharacterBehavior : MonoBehaviour {
    public float xSpeed = 10; //speed moving left and right
    public float ySpeed = 10;// speed moving up and down
    public float dashModifier = 2; //speed increase by dashing
    public float smoothing = 0.5f;
    public bool facingRight, attacking = false;
    bool dashing;
    public Rigidbody2D rb;
    public GameManager gm;
    SpriteRenderer sp;
    Vector2 velocity = Vector2.zero;
    // Use this for initialization
    public void Start () {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        sp = GetComponent<SpriteRenderer>();

    }
    public virtual void Move(float moveX, float moveY, bool dashing = false, ForceMode2D force = ForceMode2D.Force)
    {
        Vector3 currentPosition = new Vector3(transform.position.x, transform.position.y, 0);

      //  print(moveX);
        rb.AddForce(new Vector2(moveX * xSpeed, moveY * ySpeed) * Time.deltaTime);
//        cc.velocity = Vector2.SmoothDamp(cc.velocity, cc.velocity + new Vector2(moveX * xSpeed, moveY * ySpeed), ref velocity, smoothing);
//        rb.MovePosition(currentPosition + new Vector2 (moveX * xSpeed, moveY * ySpeed) * ((dashing) ? dashModifier:1) * Time.deltaTime);
///TODO: CHANGE RB TO CHARCON
        // If the input is moving the player right and the player is facing left...
        FlipCheck(moveX);
    }

    public virtual void FlipCheck(float moveX)
    {
        if (moveX > 0 && !facingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (moveX < 0 && facingRight)
        {
            // ... flip the player.
            Flip();
        }


    }
    public virtual IEnumerator Swing(Weapon weapon, Vector3 start, Vector3 end, float totalTime = .5f) {
        //update the position of weapon to the endpoint of the swing and let it rest there for the total time
        if (!attacking)
        {

            attacking = true;
            float elapsedTime = 0;
            while (elapsedTime < totalTime)
            {
                weapon.transform.localPosition = end;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            weapon.transform.localPosition = start;
            attacking = false;
            yield return null;
        }
        else {
            yield return null;

        }

    }
    public virtual void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;
        sp.flipX = !sp.flipX;
        // Multiply the player's x local scale by -1.
       // Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }
}
