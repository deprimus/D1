using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiScript : Alive
{
    public readonly float STAR_COUNT = 6;
    public readonly float STAR_DISTANCE = 6f;
    public readonly float[] STAR_OMEGAS = { 55f, 62f, 78f, 94f, 165f }; // > 75% 50% 25% 5%
    public readonly int BULLET_DAMAGE = 20;
    public readonly float[] BULLET_SPEEDS = { 4.0f, 5.0f, 7.5f, 15.0f, 40.0f }; // > 75% 50% 25% 5%
    public readonly float[] BULLET_COUNT =  { 5f, 8f, 12f, 15f, 75f }; // > 75% 50% 25% 5%
    public readonly float[] BULLET_FREQ = { 2f, 1f, 1f, 0.5f, 0.15f }; // > 75% 50% 25% 5%

    public GameObject starPrefab;
    public GameObject bulletPrefab;

    private float angleOffset;
    private int rageIndex;

    private float attackTimer;

    private new Transform transform;
    private AudioSource audioSrc;
    private List<Transform> stars;

    void Start()
    {
        transform = GetComponent<Transform>();
        audioSrc = GetComponent<AudioSource>();

        stars = new List<Transform>();

        for(float i = 0; i < STAR_COUNT; ++i)
        {
            GameObject star = Instantiate(starPrefab, transform.position, Quaternion.identity);
            stars.Add(star.GetComponent<Transform>());
        }

        angleOffset = 0f;
        attackTimer = 0f;

        InitHp();
    }

    void Update()
    {
        CalcIndex();
        CalcStars();
        HandleAttack();
        RenderHp();
    }

    void CalcIndex()
    {
        float perc = MathSET.Map(currentHp, 0, maxHp, 0, 1);

        if(perc > 0.75f)
            rageIndex = 0;
        else if (perc > 0.5f)
            rageIndex = 1;
        else if (perc > 0.25f)
            rageIndex = 2;
        else if (perc > 0.05f)
            rageIndex = 3;
        else rageIndex = 4;
    }

    void CalcStars()
    {
        angleOffset = (angleOffset + STAR_OMEGAS[rageIndex] * Time.deltaTime) % 360f;

        for (int i = 0; i < stars.Count; ++i)
        {
            float angle = ((MathSET.Map(i, 0, stars.Count, 0f, 360f) + angleOffset) % 360f) * Mathf.Deg2Rad;
            stars[i].position = new Vector3(transform.position.x + STAR_DISTANCE * Mathf.Cos(angle), transform.position.y + STAR_DISTANCE * Mathf.Sin(angle), transform.position.z);
        }
    }

    void HandleAttack()
    {
        attackTimer += Time.deltaTime;

        if(attackTimer >= BULLET_FREQ[rageIndex])
        {
            attackTimer -= BULLET_FREQ[rageIndex];

            Shoot();
        }
    }

    void Shoot()
    {
        for (float i = 0; i < BULLET_COUNT[rageIndex]; ++i)
        {
            SoundManager.Play(audioSrc, SoundManager.Clip.BOSS_SHOOT);
            float angle = ((MathSET.Map(i, 0, BULLET_COUNT[rageIndex], 0f, 360f) + angleOffset) % 360f) * Mathf.Deg2Rad;

            Vector3 vel = new Vector3(BULLET_SPEEDS[rageIndex] * Mathf.Cos(angle),BULLET_SPEEDS[rageIndex] * Mathf.Sin(angle), 0f);

            ProjectileScript proj = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg)).GetComponent<ProjectileScript>();
            proj.Construct(vel, BULLET_DAMAGE, "Bullet_pi", 255);
        }
    }

    protected override void Die()
    {
        return;
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
        return 1.0f;
    }

    public override byte GetTeamtag()
    {
        return 255;
    }
}
