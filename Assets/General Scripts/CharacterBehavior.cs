using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is meant to have actions which are usable by all characters, such as movement, attack, and basic rigidbody initialization.
/// </summary>
[RequireComponent(typeof(Health))]
public class CharacterBehavior : MonoBehaviour {
    [SerializeField]
    private float speedBuffer = 500;

    public float xSpeed = 10; //speed moving left and right
    public float ySpeed = 10;// speed moving up and down
    public float dashModifier = 2; //speed increase by dashing
    public float smoothing = 0.5f;
    public bool facingRight, attacking = false, charging = false, stunned = false;
    bool dashing;
    public Rigidbody2D rb;
    public GameManager gm;
    public TagManager tm;
    int comboCount = 0;
    float distance = 0;
    public Animator anim;
    RaycastHit2D[] hit = new RaycastHit2D[1];
    public Weapon weapon;
    public Health health;
    [SerializeField]
    private int mainComboLimit=4;
    public SpriteRenderer sp;
    Vector2 velocity = Vector2.zero;
    [SerializeField]
    SoundInfo soundInfo;
    AudioClip hitSound, comboSlashSound;

    AudioSource audioSource;


    //possibly temporary variables for setup
    public SpriteRenderer[] hitboxSprites;
    float weaponBufferX;
    public Collider2D[] attackHitBoxes;
    [SerializeField]
    bool shielding = false;
    public Shield shield;   
    Color chargeColor;
    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        tm = FindObjectOfType<TagManager>();
        shield = GetComponentInChildren<Shield>(includeInactive: true);
        weapon = GetComponentInChildren<Weapon>();
        //attackHitBoxes = new Collider[4];
        //foreach (Collider2D item in attackHitBoxes)
        //{

        //    if (item.name.ToLower().Contains("melee attack hitbox"))
        //    {
        //        print(item.name);
        //        item.gameObject.SetActive(false);
        //    }
        //}
    }
    public void Start() {
        
        sp = GetComponent<SpriteRenderer>();
        weapon = GetComponentInChildren<Weapon>();
        attackHitBoxes = weapon.GetComponentsInChildren<Collider2D>(includeInactive:true);
        audioSource = GetComponent<AudioSource>(); 
       

        //hitboxSprites = weapon.GetComponentsInChildren<SpriteRenderer>();
        hitboxSprites = weapon.GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        //foreach (SpriteRenderer spr in hitboxSprites)
        //{
        //    print(spr.name);
        //}

        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        weaponBufferX = weapon.transform.position.x - transform.position.x;
    }
    public void OnClash()///if you change this, make sure you change the Weapon.cs call!

    {
        foreach(Collider2D col in attackHitBoxes)
        {
            col.gameObject.SetActive(false);
        }
        print("OnClash triggered");
        anim.SetTrigger("Clash");
        stunned = true;
        //start clashing animation
        //activate weapon when idle is reached
    }

    public void Reset()
    {

        //reset things to base, useful after clashing
        weapon.knockback = false;
        stunned = false;
    }
    //private void Update()
    //{
    //    print(gm);
    //}
    public virtual void Move(Vector2 input, float dash = 1, bool backwards = false)
    {
        if (!stunned && rb.bodyType == RigidbodyType2D.Kinematic)
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


            FlipCheck(input.x);
        }
    }

    public virtual void FlipCheck(float moveX)
    {
        // If the input is moving the player right and the player is facing left...

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
    
    public void End()

    ///this is literally the simplest way for me to think of extending animation clip length without a new animation
    {
        weapon.knockback = false;
        return;
    }

    public void ToggleWeaponKnockback()
    {
        //print("weapon knocks back");
        weapon.toggleKnockback();
    }
    public void StopAttacking()
    {
        stunned = false;
        attacking = false;
        if (weapon.knockback)
        {
            weapon.knockback = false;
        }
    }
    public void Attack() {
        ///Continuing the combo and hitbox activation/deactivation are currently handled by the player animator
        ///The actual sprite work is handled by the weapon (for now)
        if (!stunned)
        {
            if (!attacking)
            {
                attacking = true;
                weapon.anim.SetTrigger("Attack");
                anim.SetTrigger("Attack");
                SetClip(soundInfo.comboSound, soundInfo.comboStart, volume:soundInfo.comboVolume);


            }
        }
    }

    public void PlayClip()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.Play();

    }
    void SetClip(AudioClip clip, float time = 0, float volume = 1)
    {
        audioSource.clip = clip;
        audioSource.time = time;
        audioSource.volume = volume;
    }
    public void Shoot()
    {
        ///Shoot() will simply shoot
        return;
    }
    public void RaiseShield()
    {
        shielding = true;
        shield.gameObject.SetActive(true);
    }

    public void LowerShield()
    {
        shielding = false;
        shield.gameObject.SetActive(false);
    }
    public virtual IEnumerator Knockback(float force)
    {///while this is running, launch
        yield return null;

    }
    public virtual IEnumerator Charge(Vector2 movementInput, bool button) 
    {
        return null;


    }
    public virtual IEnumerator Swing(Weapon weapon, Vector3 start, Vector3 end, float totalTime = .5f) {
        //update the position of weapon to the endpoint of the swing and let it rest there for the total time
        if (!attacking)
        {

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
    public string GetOppositeTag()
    {
        if (tag == tm.PlayerTag)
        {
            return tm.EnemyTag;
        }
        else if (tag == tm.EnemyTag)
        {
            return tm.PlayerTag;
        }
        else
        {
            return "tag unknown";
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("dynamic collision occurring");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
    //    print(System.String.Format("collider colliding with {0}: {1} of {2}", name, collision.name, collision.transform.root.name));

        //weapon tag is stored in parent of hitboxes with weapon script object
        if (collision.gameObject != collision.transform.root.gameObject)
        {
            if (collision.transform.parent.tag == tm.WeaponTag && collision.gameObject.transform.root.tag == GetOppositeTag())
            {
                Weapon attackingWeapon = collision.GetComponentInParent<Weapon>();
                //TODO: code shield hitbox interaction
                //if collider overlaps with shield collider and not behind shield collider, reduce damage
                float damage = Shield(attackingWeapon.damage);
                SetClip(soundInfo.hitSound, volume:soundInfo.hitVolume);
                PlayClip();
                health.TakeDamage(damage);
                Debug.Log(name + " hit by " + attackingWeapon.transform.parent.name);
                print(collision.gameObject.name);
                if (attackingWeapon.knockback)
                {
                    StopAllCoroutines();
                    print(name + "knocked back by" + attackingWeapon.transform.parent.name);
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    attackingWeapon.Launch(rb);
                    //TODO: make launching work
                }
            }
        }


    }

    float Shield(float damage)
    {
        
        if (shielding)
        {
            damage = damage - damage * shield.def;
        }
        return damage;
    }

    //public virtual void OnTriggerEnter2D(Collider2D collision)
    //{


    //    if(collision.gameObject.tag == tm.WeaponTag && collision.gameObject.transform.parent.tag == tm.EnemyTag)
    //    {
    //        Weapon attackingWeapon = collision.GetComponent<Weapon>();
            
            
    //    }
    //}

    public virtual void Flip()
    {
        if (sp)
        {
            // Switch the way the player is labelled as facing.
            facingRight = !facingRight;
            sp.flipX = !sp.flipX;
            
            weapon.sp.flipX = !weapon.sp.flipX;
            foreach (SpriteRenderer hitboxSprite in hitboxSprites)
            {
                hitboxSprite.flipX = !hitboxSprite.flipX;
            }
            foreach(Collider2D hitbox in attackHitBoxes)
            {
                //print(string.Format("{0} offset before: {1}", hitbox.name, hitbox.offset));
                hitbox.offset = new Vector2(hitbox.offset.x * -1, hitbox.offset.y);
                //print(string.Format("{0} offset after: {1}", hitbox.name, hitbox.offset));
            }
            //Debug.Log("before:" + weapon.transform.position.x.ToString());
            weaponBufferX = weaponBufferX * -1;
            // weapon.transform.localPosition = new Vector3(1, 0, 0);
            //weapon.transform.position = new Vector3(transform.position.x + weaponBufferX, weapon.transform.position.y, weapon.transform.position.z);
            weapon.transform.localPosition = new Vector3(weapon.transform.localPosition.x * -1, weapon.transform.localPosition.y, weapon.transform.localPosition.z);
            //Debug.Log("after:" + weapon.transform.position.x.ToString());

            // Multiply the player's x local scale by -1.
            // Vector3 theScale = transform.localScale;
            //theScale.x *= -1;
            //transform.localScale = theScale;
        }
    }
}
