using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is meant to have actions which are usable by all characters, such as movement, attack, and basic rigidbody initialization.
/// </summary>
public class CharacterBehavior : MonoBehaviour {
    [SerializeField]
    private float speedBuffer = 500;

    public float xSpeed = 10; //speed moving left and right
    public float ySpeed = 10;// speed moving up and down
    public float dashModifier = 2; //speed increase by dashing
    public float smoothing = 0.5f;
    public bool facingRight, attacking = false;
    bool dashing;
    public Rigidbody2D rb;
    public GameManager gm;
    float distance = 0;
    RaycastHit2D[] hit = new RaycastHit2D[1];
    Weapon weapon;
 
    SpriteRenderer sp;
    Vector2 velocity = Vector2.zero;
    // Use this for initialization

    //possibly temporary variables for setup
    float weaponBufferX;

    public void Start () {
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
        sp = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Weapon>();
        weaponBufferX = weapon.transform.position.x - transform.position.x;
    }
    public virtual void Move(Vector2 input, float dash = 1)
    {
        Vector2 pos = new Vector2(transform.position.x, transform.position.y);
        Vector2 dir = input * dash * speedBuffer * Time.fixedDeltaTime;
        distance = dir.magnitude;
        int results = rb.Cast(dir.normalized, hit, dir.magnitude * 4 - 0.01f);
        if (results > 0)
        {
            //print("detected");
            distance = hit[0].fraction * dir.normalized.magnitude;
        }
        // rb.velocity = dir ;
        if (distance < 0.01f)
        {
            distance = 0;
            //rb.velocity = Vector2.zero;
        }
        rb.MovePosition(pos + dir * distance);
        //Collider2D[] cols = 
        //rb.velocity = new Vector2(moveX, moveY) * dash * speedBuffer * Time.fixedDeltaTime;
        // rb.AddForce(new Vector2(moveX, moveY));

        // If the input is moving the player right and the player is facing left...
        FlipCheck(input.x);
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
    public void Attack() {
        weapon.anim.SetTrigger("Attack");
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
        weapon.sp.flipX = !weapon.sp.flipX;
        Debug.Log("before:" + weapon.transform.position.x.ToString());
        weaponBufferX = weaponBufferX * -1;
        // weapon.transform.localPosition = new Vector3(1, 0, 0);
        //weapon.transform.position = new Vector3(transform.position.x + weaponBufferX, weapon.transform.position.y, weapon.transform.position.z);
        weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x * -1, weapon.transform.localPosition.y, weapon.transform.localPosition.z);
        Debug.Log("after:" + weapon.transform.position.x.ToString());

        // Multiply the player's x local scale by -1.
        // Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }
}
