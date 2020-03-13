using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnTargetManager : MonoBehaviour
{
    public GameObject Target;
    public float speed = 3.5f;
    [SerializeField] private float ZoomValue = 0.0f;

    private float X;
    private float Y;
    private PlayerMovement playerMovement;

    Vector3 currentPos = Vector3.zero;
    

    // Start is called before the first frame update
    void Start()
    {
        if (Target != null)
        {
            transform.SetParent(Target.transform);
        }

        currentPos = transform.localPosition;
        playerMovement = Target.GetComponent<PlayerMovement>();
     
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(Input.acceleration.normalized, Vector3.up);
        RotateTarget();
        //DragAndRotate();
        //ZoomIn();
        //ZoomOut();
        //ScrollCamera();
    }

    void RotateTarget()
    {
        if (Input.GetAxis("Mouse X") != 0)
        {
            Target.transform.Rotate(0, Input.GetAxis("Mouse X") * 80 * Time.deltaTime, 0);
        }

        if (Input.GetAxis("Mouse Y") != 0)
        {
            Target.transform.Rotate(Input.GetAxis("Mouse Y") * 80 * Time.deltaTime, 0, 0);
        }

        float z = Target.transform.eulerAngles.z;
        Target.transform.Rotate(0, 0, -z);
    }

    void DragAndRotate()
    {
        if (Input.GetMouseButton(0))
        {
            Target.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            Target.transform.rotation = Quaternion.Euler(X, Y, 0);
        }
    }

    void ZoomIn()
    {
        //Quaternion TargetRotation = Quaternion.Euler(Target.transform.eulerAngles.x, Target.transform.eulerAngles.y, Target.transform.eulerAngles.z);
        //transform.rotation = TargetRotation;
        //float currentX = Target.axis
        if (Input.GetMouseButtonDown(1))
        {
            //transform.parent = null;
            //transform.rotation = Target.transform.rotation;
            transform.localPosition += new Vector3(0, 0, ZoomValue);

            Debug.Log("Zzzzooooooom");
        }

    }

    void ZoomOut()
    {
        currentPos = transform.position;
        if (Input.GetMouseButtonUp(1))
        {
            transform.localPosition -= new Vector3(0, 0, ZoomValue);
        }
    }

    void ScrollCamera()
    {

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            GetComponent<Camera>().fieldOfView--;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            GetComponent<Camera>().fieldOfView++;
        }
    }
}