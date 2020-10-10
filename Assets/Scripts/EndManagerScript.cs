using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndManagerScript : MonoBehaviour
{
    private float timer;
    private GameObject canvas;
    private Text title;
    private DialogManagerScript dialogManager;

    private bool[] timepoints = { false, false, false, false, false, false };
    private bool mode2;

    private AudioSource audioSrc;

    void Start()
    {
        canvas = GameObject.FindGameObjectsWithTag("TransitionCanvas")[0];
        title = GameObject.FindGameObjectsWithTag("Title")[0].GetComponent<Text>();
        dialogManager = dialogManager = GameObject.FindObjectOfType<DialogManagerScript>();

        audioSrc = GetComponent<AudioSource>();

        timer = 0f;
        mode2 = false;
    }

    void Update()
    {
        Tick();
        Render();
    }

    void Tick()
    {
        timer += Time.deltaTime;
    }

    void Render()
    {
        if(timer < 2.0f) { } // Do nothing.
        else if (timer < 6.0f)
        {
            if(mode2)
            {
                if(!timepoints[4])
                {
                    title.text = "";
                    canvas.SetActive(true);
                    timepoints[4] = true;
                }
            }
            else
            {
                if(!timepoints[0])
                {
                    SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                    title.text = "5 years later...";
                    timepoints[0] = true;
                }
            }
        }
        else if (timer < 9.0f)
        {
            if(mode2)
            {
                if(!timepoints[5])
                {
                    SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                    title.text = "To be continued";
                    timepoints[5] = true;
                }
            }
            else
            {
                if(!timepoints[1])
                {
                    canvas.SetActive(false);
                    timepoints[1] = true;
                }
            }
        }
        else if (timer < 11.0f)
        {
            if(mode2)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                if(!timepoints[2])
                {
                    canvas.SetActive(false);
                    timepoints[2] = true;
                }
            }
        }
        else
        {
            if (!timepoints[3])
            {
                dialogManager.ShowDialog(() =>
                {
                    timer = 2.0f;
                    mode2 = true;
                },
                new Tuple<string, string>("David", "Five years ago, I almost died."),
                new Tuple<string, string>("David", "I will get my revenge, Pi..."));

                timepoints[3] = true;
            }
        }
    }
}
