using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyController : MonoBehaviour {
    public float moveSpeed = 0.1f;
    public float moveThreshold = 0.03f;
    public float lookSensitivity = 3.0f;


    [Header("In editor")]
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    public bool WASDInEditor = true;
    

    void Update()
    {

        if (Application.isEditor)
        {
            if (WASDInEditor)
            {
                if (Input.GetMouseButton(1))
                {

                    rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
                    rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
                    rotationY = Mathf.Clamp(rotationY, -90, 90);

                    if (Input.GetKey(KeyCode.R))
                    {
                        transform.position = new Vector3(0f, 9f, -34f);
                        rotationX = rotationY = 0;
                    }

                    transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
                    transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

                    float speed = normalMoveSpeed;

                    if (Input.GetKey(KeyCode.LeftShift))
                        speed *= fastMoveFactor;
                    else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightShift))
                        speed *= slowMoveFactor;

                    transform.position += transform.forward * speed * Input.GetAxis("Vertical") * Time.deltaTime;
                    transform.position += transform.right * speed * Input.GetAxis("Horizontal") * Time.deltaTime;


                    if (Input.GetKey(KeyCode.E)) { transform.position += transform.up * speed * Time.deltaTime; }
                    if (Input.GetKey(KeyCode.Q)) { transform.position -= transform.up * speed * Time.deltaTime; }
                }
            }
        }
        else
        {
            if (LZWPlib.LzwpTracking.Instance.flysticks.Count < 1)
                return;

            float hor = LZWPlib.LzwpTracking.Instance.flysticks[0].joystickHorizontal;
            float ver = LZWPlib.LzwpTracking.Instance.flysticks[0].joystickVertical;


            Vector3 rotation = new Vector3(0, hor, 0) * lookSensitivity * Time.deltaTime;

            //if (rotation != Vector3.zero && LZWPlib.LzwpTracking.Instance.flysticks[0].fire.isActive)
                transform.Rotate(rotation);


            Vector3 pos = transform.position;

            if (Mathf.Abs(ver) >= moveThreshold)
                pos += transform.rotation * (LZWPlib.LzwpTracking.Instance.flysticks[0].rotation * Vector3.forward) * moveSpeed * ver * Time.deltaTime;

            transform.position = pos;
        }
    }
}
