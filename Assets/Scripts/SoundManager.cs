using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Clip
    {
        PLAYER_SHOOT,
        ENEMY_SHOOT,
        BOSS_SHOOT,
        HIT,
        TRANSITION,
        CLICK,
        PTR,
        SIGSEGV
    }

    private static Dictionary<Clip, AudioClip> clips;

    static SoundManager()
    {
        clips = new Dictionary<Clip, AudioClip>();

        Array clipTypes = Enum.GetValues(typeof(Clip));

        for (int i = 0; i < clipTypes.Length; ++i)
        {
            Clip type = (Clip)clipTypes.GetValue(i);
            clips.Add(type, Resources.Load<AudioClip>("Audio/" + type.ToString().ToLower()));
        }
    }

    public static void Play(AudioSource src, Clip clip) => src.PlayOneShot(clips[clip]);

    public static void PlayWith(AudioSource src, Clip clip, Delegates.ShallowDelegate next)
    {
        src.PlayOneShot(clips[clip]);

        StaticCoroutine.Start(FollowPlay(clips[clip], next));
    }

    private static IEnumerator FollowPlay(AudioClip clip, Delegates.ShallowDelegate next)
    {
        yield return new WaitForSeconds(clip.length);

        next();
    }
}
