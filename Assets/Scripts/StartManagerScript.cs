using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManagerScript : MonoBehaviour
{
    private float timer;
    private GameObject canvas;
    private Text title;

    private bool[] timepoints = { false, false, false, false, false, false, false };

    private AudioSource audioSrc;

    void Start()
    {
        canvas = GameObject.FindGameObjectsWithTag("TransitionCanvas")[0];
        title = GameObject.FindGameObjectsWithTag("Title")[0].GetComponent<Text>();

        audioSrc = GetComponent<AudioSource>();

        timer = 0f;
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
        if (timer < 2.0f) { } // Do nothing.
        else if(timer < 6.0f)
        {
            if(!timepoints[0])
            {
                SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                title.text = "My name is David.";
                timepoints[0] = true;
            }
        }
        else if (timer < 8.0f)
        {
            if (!timepoints[1])
            {
                title.text = "";
                timepoints[1] = true;
            }
        }
        else if (timer < 11.0f)
        {
            if (!timepoints[2])
            {
                SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                title.text = "This is my story.";
                timepoints[2] = true;
            }
        }
        else if(timer < 13.0f)
        {
            if(!timepoints[3])
            {
                title.text = "";
                timepoints[3] = true;
            }
        }
        else if(timer < 16.0f)
        {
            if(!timepoints[4])
            {
                SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                title.text = "October 11th, 2010";
                timepoints[4] = true;
            }
        }
        else if (timer < 18.0f)
        {
            if (!timepoints[5])
            {
                title.text = "";
                timepoints[5] = true;
            }
        }
        else if (timer < 22.0f)
        {
            if (!timepoints[6])
            {
                SoundManager.Play(audioSrc, SoundManager.Clip.TRANSITION);

                title.text = "I woke up in a strange place.";
                timepoints[6] = true;
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
