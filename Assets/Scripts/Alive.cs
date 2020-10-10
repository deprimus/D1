using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Alive : MonoBehaviour
{
    public readonly float HP_BAR_DISTANCE = 1.0f;

    public GameObject hpEmptyPrefab;
    public GameObject hpOverlayPrefab;

    protected Transform hpEmptyTransform;
    protected Transform hpOverlayTransform;

    public int maxHp;
    public int currentHp;

    public virtual void TakeDamage(int dmg)
    {
        if (GetAudioSource() != null)
            SoundManager.Play(GetAudioSource(), SoundManager.Clip.HIT);
        currentHp -= dmg;

        if (currentHp <= 0) {
            currentHp = 0;
            Die();
        }
    }

    public abstract byte GetTeamtag();

    protected virtual void InitHp()
    {
        GameObject hpEmpty = Instantiate(hpEmptyPrefab, GetTransform().position, Quaternion.identity);
        GameObject hpOverlay = Instantiate(hpOverlayPrefab, GetTransform().position, Quaternion.identity);

        hpEmptyTransform = hpEmpty.GetComponent<Transform>();
        hpOverlayTransform = hpOverlay.GetComponent<Transform>();

        currentHp = maxHp;
    }

    protected virtual void RenderHp()
    {
        float scale = GetHpScale();

        hpEmptyTransform.localScale = new Vector3(scale, scale, 1f);
        hpOverlayTransform.localScale = new Vector3(MathSET.Map(currentHp, 0f, maxHp, 0f, scale), scale, 1f);

        Vector3 pos = GetTransform().position + (-GetTransform().up * HP_BAR_DISTANCE);
        Vector3 angle = GetTransform().eulerAngles;

        hpEmptyTransform.position = pos;
        hpOverlayTransform.position = pos;

        hpEmptyTransform.eulerAngles = angle;
        hpOverlayTransform.eulerAngles = angle;
    }

    protected abstract void Die();

    protected abstract Transform GetTransform();

    protected abstract AudioSource GetAudioSource();

    protected abstract float GetHpScale();
}
