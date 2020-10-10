using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1ManagerScript : MonoBehaviour
{
    private float timer = 0f;
    private bool fuse = true;

    void Update()
    {
        if(fuse)
        {
            timer += Time.deltaTime;

            if(timer > 0.5f)
            {
                fuse = false;

                GameObject.FindObjectOfType<DialogManagerScript>().ShowDialog(
                    new Tuple<string, string>("David", "I found a piece of paper on the ground."),
                    new Tuple<string, string>("David", "It says:"),
                    new Tuple<string, string>("NOTE", "\"Use [WASD] and the mouse to move around.\""),
                    new Tuple<string, string>("NOTE", "\"Press [LEFT CLICK] to cast a PHP projectile.\""),
                    new Tuple<string, string>("NOTE", "\"You will also notice an arrow around you. That's the POINTER.\""),
                    new Tuple<string, string>("NOTE", "\"At any moment, the pointer can be either BLUE or RED.\""),
                    new Tuple<string, string>("NOTE", "\"If the pointer is BLUE, press [E] to teleport where the pointer points to.\""),
                    new Tuple<string, string>("NOTE", "\"If you teleport inside a wall, you will cause a segmentation fault (SIGSEGV).\""),
                    new Tuple<string, string>("NOTE", "\"If the pointer is RED, it points to an illegal memory address.\""),
                    new Tuple<string, string>("NOTE", "\"Attempting to teleport while the pointer is RED will also cause a SIGSEGV.\""),
                    new Tuple<string, string>("NOTE", "\"When a SIGSEGV occurs, the scene is reset.\""),
                    new Tuple<string, string>("NOTE", "\"Good luck.\""),
                    new Tuple<string, string>("David", "..."),
                    new Tuple<string, string>("David", "Okay.")
                );
            }
        }
    }
}
