using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : Alive
{
    public readonly float BULLET_SPEED = 10.0f;
    public readonly float BULLET_SPAWN_DIST = 0.85f;
    public readonly int BULLET_DAMAGE = 15;

    public float cooldown = 0.9f;
    public GameObject bulletPrefab;

    public new Transform transform;
    public Rigidbody2D rigidBody;
    public AudioSource audioSrc;
    public Transform playerTransform;

    private EnemyManagerScript enemyManager;

    private float currentCooldown;
    private bool dead;

    void Start()
    {
        transform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();

        enemyManager = GameObject.FindObjectOfType<EnemyManagerScript>();

        currentCooldown = 0f;
        dead = false;

        InitHp();
    }

    void Update()
    {
        if(Time.timeScale == 0f)
            return;

        HandleDirection();
        HandleAttack();
        RenderHp();
    }

    void HandleDirection()
    {
        float angle = MathSET.AngleBetween(playerTransform.position,transform.position);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void HandleAttack()
    {
        currentCooldown = Mathf.Max(0f, currentCooldown - Time.deltaTime);

        if (currentCooldown == 0f)
        {
            currentCooldown = cooldown;

            Shoot();
        }
    }

    void Shoot()
    {
        SoundManager.Play(audioSrc, SoundManager.Clip.ENEMY_SHOOT);

        Vector3 pos = transform.position + transform.up * BULLET_SPAWN_DIST;

        ProjectileScript proj = Instantiate(bulletPrefab, pos, transform.rotation).GetComponent<ProjectileScript>();
        proj.Construct(transform.up * BULLET_SPEED, BULLET_DAMAGE, "Bullet", 1);
    }

    protected override void Die()
    {
        if(!dead)
        {
            dead = true;

            Destroy(hpEmptyTransform.gameObject);
            Destroy(hpOverlayTransform.gameObject);
            Destroy(gameObject);

            --enemyManager.enemyCount;
        }
    }

    protected override Transform GetTransform()
    {
        return transform;
    }

    protected override AudioSource GetAudioSource()
    {
        return audioSrc;
    }

    protected override float GetHpScale()
    {
        return 0.2f;
    }

    public override byte GetTeamtag()
    {
        return 1;
    }
}
