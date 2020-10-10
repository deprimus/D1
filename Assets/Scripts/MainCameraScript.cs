using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour
{
    private readonly float ZOOM_BASE_DISTANCE = 5f;
    private readonly float ZOOM_OUT_DISTANCE = 15f;
    private readonly float ZOOM_SPEED = 4.25f;

    public bool isEnd = false;
    public bool zoomedOut;
    public bool isZoomingOut;
    public bool isZoomingIn;

    private float zoom;

    private new Transform transform;
    private new Camera camera;
    private Transform playerTransform;

    void Start()
    {
        camera = GetComponent<Camera>();
        transform = GetComponent<Transform>();

        if(!isEnd)
            playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<Transform>();

        zoomedOut = false;
        isZoomingOut = false;
        isZoomingIn = false;
        zoom = ZOOM_BASE_DISTANCE;
    }

    void Update()
    {
        if(!isEnd)
            transform.localPosition = new Vector3(playerTransform.position.x, playerTransform.position.y, -0.5f);

        UpdateZoom();
    }

    void UpdateZoom()
    {
        if(isZoomingOut)
        {
            zoom = Mathf.Min(ZOOM_OUT_DISTANCE, zoom + ZOOM_SPEED * Time.unscaledDeltaTime);

            if (zoom == ZOOM_OUT_DISTANCE)
            {
                zoomedOut = true;
                isZoomingOut = false;
            }

            camera.orthographicSize = zoom;
        }
        else if(isZoomingIn)
        {
            zoom = Mathf.Max(ZOOM_BASE_DISTANCE, zoom - ZOOM_SPEED * Time.unscaledDeltaTime);

            if (zoom == ZOOM_BASE_DISTANCE)
            {
                zoomedOut = false;
                isZoomingIn = false;
                Time.timeScale = 1f;
            }

            camera.orthographicSize = zoom;
        }
    }

    public void StartZoomOut()
    {
        isZoomingOut = true;
        Time.timeScale = 0f;
    }

    public void StartZoomIn()
    {
        isZoomingIn = true;
    }
}
