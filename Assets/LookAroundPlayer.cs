using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundPlayer : MonoBehaviour
{
    public GameObject Target;
    

    public float dragSpeed = 120f;
    public float stdSpeed = 80f;
    public float scrollSpeed = 30f;
    public float resetTimer = 5f;
    public float xMin = 0f;
    public float xMax = 0f;


    private float X;
    private float Y;
    private float timeLeft = 0f;
    private bool startTimer = false;
    private float mouseX = 0f;
    Vector3 z = new Vector3 (0, 0, 1.9f);
    Quaternion StartRot = Quaternion.identity;

    // Start is called before the first frame update
    void Start()
    {
        if (Target != null)
        {
            transform.SetParent(Target.transform);
        }

        transform.localPosition = Target.transform.position - z;
        StartRot = Target.transform.localRotation;
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        DragAndRotate();
        ScrollZoom();
        //Debug.Log("X: " + (Input.mousePosition.x - (Screen.width/2)) + " Y: " + (Input.mousePosition.y - (Screen.height / 2)));
        if (Input.GetKey(KeyCode.F)) { Target.transform.localRotation = StartRot; }
        if (startTimer) { Timer(); }

        //RotateCamera();

    }
    float mouseY, mouseXXX;
    float rotX;
    float rotY;

    private void RotateCamera()
    {
        mouseY = Input.GetAxis("Mouse Y");
        mouseXXX = Input.GetAxis("Mouse X");

        rotX += mouseY * dragSpeed * Time.deltaTime;
        rotY += mouseXXX * dragSpeed * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -90, 90);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = localRot;
    }

    void DragAndRotate()
    {
        if (Input.GetMouseButton(0))
        {
            Target.transform.Rotate(Input.GetAxis("Mouse Y") * dragSpeed * Time.deltaTime, -Input.GetAxis("Mouse X") * dragSpeed * Time.deltaTime, 0);
            float z = Target.transform.eulerAngles.z;
            Target.transform.Rotate(0, 0, -z);
            startTimer = false;

        }
        else if(Input.GetMouseButtonUp(0))
        {
            timeLeft = resetTimer;
            startTimer = true;
        }
        else
        {
            Target.transform.parent.Rotate(0, Input.GetAxis("Mouse X") * stdSpeed * Time.deltaTime, 0, Space.Self);
            //mouseX = 90 * ((Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2));
            Target.transform.parent.Rotate(0, mouseX * Time.deltaTime, 0, Space.Self);

            float z = Target.transform.parent.eulerAngles.z;
            Target.transform.parent.Rotate(0, 0, -z);
        }

        Vector3 xRotation = Target.transform.localEulerAngles;
        xRotation.x = (xRotation.x > 180) ? xRotation.x - 360 : xRotation.x;
        xRotation.x = Mathf.Clamp(xRotation.x, xMin, xMax);

        Target.transform.localRotation = Quaternion.Euler(xRotation);
 
    }

    void ScrollZoom()
    {
        float scrollValue = Input.GetAxisRaw("Mouse ScrollWheel");
        
        if ((transform.position - (Target.transform.position + new Vector3(0,0.5f,0))).magnitude < 1 )
        {
            scrollValue = Mathf.Clamp(scrollValue, -1, 0);
        }
        else if((transform.position - (Target.transform.position + new Vector3(0, 0.5f, 0))).magnitude > 10)
        {
            scrollValue = Mathf.Clamp(scrollValue, 0, 1);
        }
        transform.Translate(0, 0, scrollValue * scrollSpeed);

        
    }

    void Timer()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            LerpBack();
        }
    }

    void LerpBack()
    {
        Target.transform.localRotation = Quaternion.Lerp(Target.transform.localRotation, StartRot, 2f * Time.deltaTime);
        
        if (Quaternion.Angle(Target.transform.localRotation, StartRot) <= 0.7f)
        {
            Target.transform.localRotation = StartRot;
            startTimer = false;
        }

    }
}
