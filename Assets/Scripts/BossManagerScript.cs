using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManagerScript : MonoBehaviour
{
    private float timer = 0f;
    private bool fuse = true;

    void Update()
    {
        if (fuse)
        {
            timer += Time.deltaTime;

            if (timer > 0.5f)
            {
                fuse = false;

                GameObject.FindObjectOfType<DialogManagerScript>().ShowDialog(
                    new Tuple<string, string>("???", "So, you're finally here."),
                    new Tuple<string, string>("David", "Who are you?"),
                    new Tuple<string, string>("???", "I shall introduce myself."),
                    new Tuple<string, string>("Pi", "My name is Pi, and I am a powerful entity."),
                    new Tuple<string, string>("Pi", "The reason you were brought here is correlated with my judgement."),
                    new Tuple<string, string>("Pi", "I believe you should perish."),
                    new Tuple<string, string>("Pi", "Therefore, I challenge you to a fight."),
                    new Tuple<string, string>("Pi", "Your PHP projectiles are way too slow."),
                    new Tuple<string, string>("Pi", "My stars will block them like they're nothing."),
                    new Tuple<string, string>("Pi", "Anyways, may the best one win.")
                );
            }
        }
    }
}
