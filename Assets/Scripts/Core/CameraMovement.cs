using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;    
    [SerializeField] private GameObject camScreen;
    [SerializeField] private GameObject farm;
    [SerializeField] private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;
    private bool move;

    private int currentAngle;

    void Update()
    {
        if(move == true)
        {
            PanCamera();
            ZoomCamera();
        }
    }

    public void ToggleCameraMove()
    {
        camScreen.SetActive(!camScreen.activeSelf);
        move =  (!move);
    }

    private void PanCamera()
    {
        if(Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButton(0))
        {
            
            Vector3 diff = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += diff;
        }
    }

    private void ZoomCamera()
    {
        if (Input.GetAxis ("Mouse ScrollWheel") > 0)
            ZoomIn();

        if (Input.GetAxis ("Mouse ScrollWheel") < 0)
            ZoomOut();
    }

    public void RotateCamera()
    {
        int newAngle = 0;

        if (currentAngle == 0)
            newAngle = 180;
        else if (currentAngle == 180)
            newAngle = 0;
        
        farm.transform.Rotate(0,0,newAngle);

        for(int i = 0; i < farm.transform.childCount; i++)
            farm.transform.GetChild(i).gameObject.transform.Rotate(0,0,newAngle);
    }
    
    private void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }

    private void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }
}
