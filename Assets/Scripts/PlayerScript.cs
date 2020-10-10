using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : Alive
{
    public readonly float BULLET_SPEED = 4.0f;
    public readonly float BULLET_SPAWN_DIST = 0.75f;
    public readonly int BULLET_DAMAGE = 10;

    private readonly string[] BULLET_TYPES = { "Bullet_php0", "Bullet_php1", "Bullet_php2", "Bullet_php3", "Bullet_php4", "Bullet_php5" };

    public readonly float PTR_DISTANCE = 3.0f;
    public readonly float PTR_OMEGA = 180f;
    public readonly Color PTR_OK = new Color(0f, 0f, 1f);
    public readonly Color PTR_SIG = new Color(1f, 0f, 0f);
    public readonly float PTR_SIG_RATE = 0.1f; // Sig check and balance frequency.
    public readonly float PTR_BASE_OKSIG = 0.025f;
    public readonly float PTR_PERC_BALANCE_FACTOR = 0.025f;

    private float ptrTimer;
    private float ptrSigTimer;
    private float sigokPerc;
    private float oksigPerc;
    private bool dead;

    private bool isSig;

    public float speed = 4.5f;
    public float cooldown = 0.2f;
    public bool isAtBoss = false;

    public GameObject bulletPrefab;

    private Transform ptrArrow;
    private Transform ptrHead;
    private SpriteRenderer ptrArrowRenderer;
    private SpriteRenderer ptrHeadRenderer;
    private DialogManagerScript dialogManager;
    private PiScript boss;

    public new Transform transform;
    public Rigidbody2D rigidBody;
    public AudioSource audioSrc;

    private float currentCooldown;

    void Start()
    {
        transform = GetComponent<Transform>();
        rigidBody = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        dialogManager = GameObject.FindObjectOfType<DialogManagerScript>();

        if(isAtBoss)
            boss = GameObject.FindGameObjectsWithTag("Pi")[0].GetComponent<PiScript>();

        currentCooldown = 0f;

        InitHp();

        ptrTimer = 0f;
        ptrSigTimer = 0f;

        ptrArrow = GameObject.FindGameObjectsWithTag("Arrow")[0].GetComponent<Transform>();
        ptrHead = GameObject.FindGameObjectsWithTag("ArrowHead")[0].GetComponent<Transform>();

        ptrArrowRenderer = ptrArrow.gameObject.GetComponent<SpriteRenderer>();
        ptrHeadRenderer = ptrHead.gameObject.GetComponent<SpriteRenderer>();

        ptrArrow.localScale = new Vector3(ptrArrow.localScale.x, PTR_DISTANCE / ptrArrowRenderer.bounds.size.y, ptrArrow.localScale.z);

        ptrArrowRenderer.color = PTR_OK;
        ptrHeadRenderer.color = PTR_OK;

        sigokPerc = 1f - PTR_BASE_OKSIG;
        oksigPerc = PTR_BASE_OKSIG;

        isSig = false;
        dead = false;
    }

    void Update()
    {
        if(Time.timeScale == 0)
            return;

        if (!dead)
        {
            HandleMovement();
            HandleAttack();
            HandleArrow();
            RenderHp();
        }
    }

    void HandleMovement() {
        // Keyboard.

        Vector2 vel = Vector2.zero;

        if(Input.GetKey(KeyCode.W))
            vel += new Vector2(0f, 1.0f);
        if(Input.GetKey(KeyCode.S))
            vel += new Vector2(0f, -1.0f);
        if(Input.GetKey(KeyCode.D))
            vel += new Vector2(1.0f, 0f);
        if(Input.GetKey(KeyCode.A))
            vel += new Vector2(-1.0f, 0f);

        rigidBody.velocity = ((Vector3)(speed * vel.normalized));

        // Mouse.

        float angle = MathSET.AngleBetween(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position);

        transform.eulerAngles = new Vector3(0f, 0f, angle);
    }

    void HandleAttack()
    {
        currentCooldown = Mathf.Max(0f, currentCooldown - Time.deltaTime);

        if(Input.GetMouseButton(0))
        {
            if(currentCooldown == 0f)
            {
                currentCooldown = cooldown;

                Shoot();
            }
        }
    }

    void HandleArrow()
    {
        ptrTimer = (ptrTimer + PTR_OMEGA * Time.deltaTime) % 360f;

        Vector3 src = transform.position;
        Vector3 target = new Vector3(transform.position.x + PTR_DISTANCE * Mathf.Cos(ptrTimer * Mathf.Deg2Rad), transform.position.y + PTR_DISTANCE * Mathf.Sin(ptrTimer * Mathf.Deg2Rad), transform.position.z);

        ptrArrow.position = (src + target) / 2;
        ptrArrow.eulerAngles = new Vector3(0f, 0f, ptrTimer - 90);

        ptrHead.position = target;
        ptrHead.eulerAngles = new Vector3(0f, 0f, ptrTimer - 90);

        if(oksigPerc != PTR_BASE_OKSIG)
        {
            float val = PTR_PERC_BALANCE_FACTOR * Time.deltaTime / PTR_SIG_RATE; // 100ms;
            oksigPerc = Mathf.Max(PTR_BASE_OKSIG, oksigPerc - val);
            sigokPerc = Mathf.Min(1f - PTR_BASE_OKSIG, sigokPerc + val);
        }

        ptrSigTimer += Time.deltaTime;

        if(ptrSigTimer > PTR_SIG_RATE)
        {
            ptrSigTimer -= PTR_SIG_RATE;

            if(isSig)
            {
                if(UnityEngine.Random.Range(0, 100) / 100f < sigokPerc)
                {
                    isSig = false;
                    ptrArrowRenderer.color = PTR_OK;
                    ptrHeadRenderer.color = PTR_OK;
                }
            }
            else
            {
                if(UnityEngine.Random.Range(0, 100) / 100f < oksigPerc)
                {
                    isSig = true;
                    ptrArrowRenderer.color = PTR_SIG;
                    ptrHeadRenderer.color = PTR_SIG;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            rigidBody.position = target;
            oksigPerc = 1 - PTR_BASE_OKSIG;
            sigokPerc = PTR_BASE_OKSIG;

            if (isSig)
            {
                SoundManager.Play(audioSrc, SoundManager.Clip.SIGSEGV);
                SIGSEGV();
            } else SoundManager.Play(audioSrc, SoundManager.Clip.PTR);
        }
    }

    void Shoot()
    {
        SoundManager.Play(audioSrc, SoundManager.Clip.PLAYER_SHOOT);

        Vector3 pos = transform.position + transform.up * BULLET_SPAWN_DIST;

        ProjectileScript proj = Instantiate(bulletPrefab, pos, transform.rotation).GetComponent<ProjectileScript>();
        proj.Construct(transform.up * BULLET_SPEED, BULLET_DAMAGE, GetBulletType(), 0);
    }

    string GetBulletType()
    {
        return BULLET_TYPES[UnityEngine.Random.Range(0, BULLET_TYPES.Length)];
    }

    protected override void Die()
    {
        if(!dead)
        {
            dead = true;

            RenderHp();

            if(isAtBoss)
            {
                if(MathSET.Map(boss.currentHp, 0f, boss.maxHp, 0f, 1f) > 0.05f)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                else dialogManager.ShowDialog(() =>
                   {
                       SoundManager.PlayWith(audioSrc, SoundManager.Clip.SIGSEGV, () =>
                       {
                           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                       });
                   },
                   new Tuple<string, string>("Pi", "Phew, that was close."),
                   new Tuple<string, string>("David", "..."),
                   new Tuple<string, string>("David", "I'm doomed."),
                   new Tuple<string, string>("Pi", "You see, I'm actually immortal."),
                   new Tuple<string, string>("Pi", "Even if you wanted to, you couldn't kill me."),
                   new Tuple<string, string>("Pi", "Anyway, looks like you're kind of dead."),
                   new Tuple<string, string>("Pi", "I'll take my leave. Goodbye."));
            }
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void SIGSEGV()
    {
        dialogManager.ShowDialog(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        },
        new Tuple<string, string>("SYSTEM", "Program received signal SIGSEGV (0xB)\nSegmentation fault (core dumped)"));
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
        return 0;
    }
}
