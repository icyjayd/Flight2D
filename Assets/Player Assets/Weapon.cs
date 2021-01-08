using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer sp;
    public float damage;
    float baseDamage;
    public bool knockback;
    public float knockbackForce = 100;
    [SerializeField]
    Color clashColor = Color.yellow;
    Color normalColor;
    SpriteRenderer spriteRenderer;
    private void Start()
    {
        
        spriteRenderer= GetComponent<SpriteRenderer>();
        normalColor = spriteRenderer.color;
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        baseDamage = damage;
    }
    public void toggleKnockback()
    {
        knockback = !knockback;

    }

    IEnumerator UseClashColor(float total = 0.25f)
    {
        float elapsed = 0;
        spriteRenderer.color = clashColor;
        while (elapsed < total)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = normalColor;
    }
    public void Clash()
    {
        StartCoroutine(UseClashColor());
        
        SendMessageUpwards("OnClash", options:SendMessageOptions.DontRequireReceiver); 
  //      print(string.Format("{0} weapon clashing!", transform.root.name));


    }
    private void Update()
    {
    }
    public static IEnumerator LaunchRB(Vector2 dir, Rigidbody2D rigidbody,float knockbackForce)
    {
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);
        float elapse = 0;
        while(rigidbody.velocity.magnitude > 0.2)
        {
            //print(rigidbody.velocity);
            if(elapse > 5)
            {
                break;

            }
            elapse += Time.deltaTime;
            yield return null;
        }
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }
    public void Launch(Rigidbody2D rigidbody)
    {
//        print(rigidbody.name);
        Vector2 dir = (rigidbody.transform.position - transform.position).normalized;
        //print(dir);
        dir.y = 0;
        StartCoroutine(LaunchRB(dir, rigidbody, knockbackForce));
        //launch back with force!
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.root.gameObject.activeInHierarchy)
        {
            if (collision.GetComponentInParent<Weapon>())
            {
                //weapon clashes cause both weapons to Clash, deactivating them and activating the clash animation of the character with that weapon
                //clashing ends when both opponents reach idle
                collision.GetComponentInParent<Weapon>().Clash();
                Clash();
            }
        }
        
    }
}
