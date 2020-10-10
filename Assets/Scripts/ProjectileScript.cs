using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Vector3 vel;
    private int dmg;
    private string sprite;
    private byte teamtag;

    private new Transform transform;
    private new SpriteRenderer renderer;
    private Rigidbody2D rigidBody;

    public void Construct(Vector3 vel, int dmg, string sprite, byte teamtag)
    {
        this.vel = vel;
        this.dmg = dmg;
        this.sprite = sprite;
        this.teamtag = teamtag;
    }

    void Start()
    {
        transform = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = Resources.Load<Sprite>("Sprites/" + sprite);

        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.velocity = vel;

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        Vector2 spriteSize = renderer.sprite.bounds.size;

        collider.size = spriteSize;
        collider.offset = new Vector2(0, 0);
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Projectile")
            return;
        if(teamtag == 255 && other.tag == "PiStar")
            return;

        MonoBehaviour obj = other.GetComponent<MonoBehaviour>();

        if(obj is Alive)
        {
            if(((Alive)obj).GetTeamtag() != teamtag)
            {
                ((Alive)obj).TakeDamage(dmg);
                Destroy(gameObject);
            }
        }
        else Destroy(gameObject);

    }
}
