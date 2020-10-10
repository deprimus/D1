using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class DialogManagerScript : MonoBehaviour
{
    private GameObject canvas;
    private Text characterText;
    private Text dialogText;
    private new MainCameraScript camera;

    private bool waitingForCameraOut;
    private bool waitingForCameraIn;
    private bool shown;
    private Delegates.ShallowDelegate onEnd;
    private Queue<Tuple<string, string>> queue;

    void Start()
    {
        canvas = GameObject.FindGameObjectsWithTag("DialogCanvas")[0];
        characterText = GameObject.FindGameObjectsWithTag("CharacterText")[0].GetComponent<Text>();
        dialogText = GameObject.FindGameObjectsWithTag("DialogText")[0].GetComponent<Text>();
        camera = GameObject.FindObjectOfType<MainCameraScript>();

        canvas.SetActive(false);
        waitingForCameraOut = false;
        waitingForCameraOut = true;
        shown = false;

        queue = new Queue<Tuple<string, string>>();
    }

    void Update()
    {
        if(waitingForCameraOut && camera.zoomedOut)
        {
            waitingForCameraOut = false;
            shown = true;
            canvas.SetActive(true);

            Next();
        }
        else if(waitingForCameraIn && (!camera.zoomedOut && !camera.isZoomingOut && !camera.isZoomingIn))
        {
            waitingForCameraIn = false;

            if(onEnd != null)
            {
                onEnd();
                onEnd = null;
            }
        }

        HandleInput();
    }

    void HandleInput()
    {
        if(shown && Input.GetMouseButtonDown(0))
            Next();
    }

    void Next()
    {
        if(queue.Count > 0)
        {
            characterText.text = queue.Peek().Item1;
            dialogText.text = queue.Peek().Item2;

            queue.Dequeue();
        }
        else
        {
            shown = false;
            waitingForCameraIn = true;
            canvas.SetActive(false);

            camera.StartZoomIn();
        }
    }

    public void ShowDialog(params Tuple<string, string>[] dialog)
    {
        camera.StartZoomOut();
        waitingForCameraOut = true;

        foreach(Tuple<string, string> entry in dialog)
            queue.Enqueue(entry);
    }

    public void ShowDialog(Delegates.ShallowDelegate onEnd, params Tuple<string, string>[] dialog)
    {
        camera.StartZoomOut();
        waitingForCameraOut = true;

        this.onEnd = onEnd;

        foreach (Tuple<string, string> entry in dialog)
            queue.Enqueue(entry);
    }
}
