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
    private bool active = false;
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }//boolean to determine whether the weapon is active all the time
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
        active = false;
        SendMessageUpwards("OnClash");
        print(string.Format("{0} weapon clashing!", transform.root.name));


    }
    private void Update()
    {
        print(string.Format("{0} weapon active status: {1}", transform.root.name, active));
    }
    public void Launch(Rigidbody2D rigidbody)
    {
        //print(rigidbody.name);
        Vector2 dir = (rigidbody.transform.position - transform.position).normalized;
        dir.y = 0;
        rigidbody.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        //launch back with force!
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
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
