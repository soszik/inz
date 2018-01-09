using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LZWPlib;
using System;
//[RequireComponent(typeof(CharacterController))]
public class FPSConrtoller : MonoBehaviour
{
    [SerializeField] private bool m_IsWalking;
    [SerializeField] private float m_WalkSpeed;
    [SerializeField] private float m_RunSpeed;
    [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
    [SerializeField] private float m_JumpSpeed;
    [SerializeField] private float m_StickToGroundForce;
    [SerializeField] public float m_GravityMultiplier;
    [SerializeField] private bool m_UseFovKick;
    [SerializeField] private bool m_UseHeadBob;
    [SerializeField] private float m_StepInterval;

    private Camera m_Camera;
    private bool m_Jump;
    private float m_YRotation;
    private Vector2 m_Input;
    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;
    private CollisionFlags m_CollisionFlags;
    private bool m_PreviouslyGrounded;
    //private Vector3 m_OriginalCameraPosition;
    private float m_StepCycle;
    private float m_NextStep;
    private bool m_Jumping;

    public float moveSpeed = 0.1f;
    public float moveThreshold = 0.03f;
    public float lookSensitivity = 3.0f;
    public List<GameObject> flyingObjects = new List<GameObject>();

    [Header("In editor")]
    public float cameraSensitivity = 90;
    public float climbSpeed = 4;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    //private float rotationX = 0.0f;
    //private float rotationY = 0.0f;
    





    // Use this for initialization
    private void Start()
    {
        if (!LzwpManager.Instance.isServer)
            RemoveController();

        m_CharacterController = GetComponent<CharacterController>();

        if (m_CharacterController == null)
            RemoveController();

        m_Camera = Camera.main;
        //m_OriginalCameraPosition = m_Camera.transform.localPosition;
        m_StepCycle = 0f;
        m_NextStep = m_StepCycle / 2f;
        m_Jumping = false;
    }

    void RemoveController()
    {
        try
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<CharacterController>());
            Destroy(GetComponent<FPSConrtoller>());
        }
        catch (System.Exception) { }
    }


    // Update is called once per frame
    private void Update()
    {
        if (LzwpTracking.Instance.flysticks[0].button1.wasPressed)
        {
            foreach (GameObject b in MasterScript.master.Puzzles)
            {

                RingScript[] scripts = b.GetComponentsInChildren<RingScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].speed = 0;
                }
            }
            foreach (GameObject b in MasterScript.master.Puzzles)
            {
                FlyingObjectScript[] scripts = b.GetComponentsInChildren<FlyingObjectScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].bezierSpeed = 0;
                    scripts[i].vibrating = false;
                    scripts[i].pulsation = false;
                    scripts[i].rotate = false;
                }
            }
            for (int i = 0; i < 5; i++)
                foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + i.ToString()))
                {
                    RingScript script = a.GetComponent<RingScript>();
                    if (script != null)
                    {
                        script.speed = 0;
                    }
                    FlyingObjectScript scriptf = a.GetComponent<FlyingObjectScript>();
                    if (scriptf != null)
                    {
                        scriptf.bezierSpeed = 0;
                        scriptf.vibrating = false;
                        scriptf.pulsation = false;
                        scriptf.rotate = false;

                    }
                }
        }


        if (LzwpTracking.Instance.flysticks[0].button2.wasPressed)
        {
            foreach (GameObject b in MasterScript.master.Puzzles)
            {

                RingScript[] scripts = b.GetComponentsInChildren<RingScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].speed = 50;
                    try
                    {
                        if (int.Parse(scripts[i].tag.Substring(5)) % 2 == 0)
                        {
                            scripts[i].right = true;
                        }
                        else
                        {
                            scripts[i].right = false;
                        }
                    }
                    catch (Exception e) { }
                }
            }
            foreach (GameObject b in MasterScript.master.Puzzles)
            {
                FlyingObjectScript[] scripts = b.GetComponentsInChildren<FlyingObjectScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].vibrating = true;
                    scripts[i].pulsation = true;
                    scripts[i].rotate = true;
                    scripts[i].bezierSpeed = 1000;
                    scripts[i].vibrationFrequency.x = 2;
                    scripts[i].vibrationFrequency.y = 3;
                    scripts[i].vibrationFrequency.z = 2.5f;
                    scripts[i].vibrationAmplitude.x = 0.01f;
                    scripts[i].vibrationAmplitude.y = 0.01f;
                    scripts[i].vibrationAmplitude.z = 0.01f;
                    scripts[i].pulsationFrequency.x = 0.35f;
                    scripts[i].pulsationFrequency.y = 0.25f;
                    scripts[i].pulsationFrequency.z = 0.5f;
                    scripts[i].pulsationAmplitudeMax.x = 2;
                    scripts[i].pulsationAmplitudeMax.y = 3;
                    scripts[i].pulsationAmplitudeMax.z = 1;
                    scripts[i].pulsationAmplitudeMin.x = 1;
                    scripts[i].pulsationAmplitudeMin.y = 2;
                    scripts[i].pulsationAmplitudeMin.z = 0.5f;
                    scripts[i].rotationSpeed.x = 35;
                    scripts[i].rotationSpeed.y = 30;
                    scripts[i].rotationSpeed.z = 25;
                    scripts[i].rotationMax.x = 0;
                    scripts[i].rotationMax.y = 0;
                    scripts[i].rotationMax.z = 0;
                    scripts[i].rotationMin.x = 0;
                    scripts[i].rotationMin.y = 0;
                    scripts[i].rotationMin.z = 0;
                }
            }
            for (int i = 0; i < 5; i++)
                foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + i.ToString()))
                {
                RingScript script = a.GetComponent<RingScript>();
                if (script != null)
                {
                        script.speed = 50;
                        try
                        {
                            if (int.Parse(script.tag.Substring(5)) % 2 == 0)
                            {
                                script.right = true;
                            }
                            else
                            {
                                script.right = false;
                            }
                        }
                        catch (Exception e) { }
                    }
                FlyingObjectScript scriptf = a.GetComponent<FlyingObjectScript>();
                if (scriptf != null)
                {
                        scriptf.vibrating = true;
                        scriptf.pulsation = true;
                        scriptf.rotate = true;
                        scriptf.bezierSpeed = 1000;
                        scriptf.vibrationFrequency.x = 2;
                        scriptf.vibrationFrequency.y = 3;
                        scriptf.vibrationFrequency.z = 2.5f;
                        scriptf.vibrationAmplitude.x = 0.01f;
                        scriptf.vibrationAmplitude.y = 0.01f;
                        scriptf.vibrationAmplitude.z = 0.01f;
                        scriptf.pulsationFrequency.x = 0.35f;
                        scriptf.pulsationFrequency.y = 0.25f;
                        scriptf.pulsationFrequency.z = 0.5f;
                        scriptf.pulsationAmplitudeMax.x = 2;
                        scriptf.pulsationAmplitudeMax.y = 3;
                        scriptf.pulsationAmplitudeMax.z = 1;
                        scriptf.pulsationAmplitudeMin.x = 1;
                        scriptf.pulsationAmplitudeMin.y = 2;
                        scriptf.pulsationAmplitudeMin.z = 0.5f;
                        scriptf.rotationSpeed.x = 35;
                        scriptf.rotationSpeed.y = 30;
                        scriptf.rotationSpeed.z = 25;
                        scriptf.rotationMax.x = 0;
                        scriptf.rotationMax.y = 0;
                        scriptf.rotationMax.z = 0;
                        scriptf.rotationMin.x = 0;
                        scriptf.rotationMin.y = 0;
                        scriptf.rotationMin.z = 0;

                    }
            }
        }


        if (LzwpTracking.Instance.flysticks[0].button3.wasPressed)
        {
            foreach (GameObject b in MasterScript.master.Puzzles)
            {

                RingScript[] scripts = b.GetComponentsInChildren<RingScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].speed = 500;
                    try
                    {
                        if (int.Parse(scripts[i].tag.Substring(5)) % 2 == 0)
                        {
                            scripts[i].right = true;
                        }
                        else
                        {
                            scripts[i].right = false;
                        }
                    }
                    catch (Exception e) { }
                }
            }
            foreach (GameObject b in MasterScript.master.Puzzles)
            {
                FlyingObjectScript[] scripts = b.GetComponentsInChildren<FlyingObjectScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    scripts[i].vibrating = true;
                    scripts[i].pulsation = true;
                    scripts[i].rotate = true;
                    scripts[i].bezierSpeed = 30000;
                    scripts[i].vibrationFrequency.x = 6;
                    scripts[i].vibrationFrequency.y = 6;
                    scripts[i].vibrationFrequency.z = 6f;
                    scripts[i].vibrationAmplitude.x = 0.01f;
                    scripts[i].vibrationAmplitude.y = 0.01f;
                    scripts[i].vibrationAmplitude.z = 0.01f;
                    scripts[i].pulsationFrequency.x = 3.35f;
                    scripts[i].pulsationFrequency.y = 2.25f;
                    scripts[i].pulsationFrequency.z = 5.5f;
                    scripts[i].pulsationAmplitudeMax.x = 2;
                    scripts[i].pulsationAmplitudeMax.y = 3;
                    scripts[i].pulsationAmplitudeMax.z = 1;
                    scripts[i].pulsationAmplitudeMin.x = 1;
                    scripts[i].pulsationAmplitudeMin.y = 2;
                    scripts[i].pulsationAmplitudeMin.z = 0.5f;
                    scripts[i].rotationSpeed.x = 150;
                    scripts[i].rotationSpeed.y = 130;
                    scripts[i].rotationSpeed.z = 170;
                    scripts[i].rotationMax.x = 0;
                    scripts[i].rotationMax.y = 0;
                    scripts[i].rotationMax.z = 0;
                    scripts[i].rotationMin.x = 0;
                    scripts[i].rotationMin.y = 0;
                    scripts[i].rotationMin.z = 0;
                }
            }
            for (int i = 0; i < 5; i++)
                foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + i.ToString()))
                {
                    RingScript script = a.GetComponent<RingScript>();
                    if (script != null)
                    {
                        script.speed = 500;
                        try
                        {
                            if (int.Parse(script.tag.Substring(5)) % 2 == 0)
                            {
                                script.right = false;
                            }
                            else
                            {
                                script.right = true;
                            }
                        }
                        catch (Exception e) { }
                    }
                    FlyingObjectScript scriptf = a.GetComponent<FlyingObjectScript>();
                    if (scriptf != null)
                    {

                        scriptf.vibrating = true;
                        scriptf.pulsation = true;
                        scriptf.rotate = true;
                        scriptf.bezierSpeed = 30000;
                        scriptf.vibrationFrequency.x = 6;
                        scriptf.vibrationFrequency.y = 6;
                        scriptf.vibrationFrequency.z = 6f;
                        scriptf.vibrationAmplitude.x = 0.01f;
                        scriptf.vibrationAmplitude.y = 0.01f;
                        scriptf.vibrationAmplitude.z = 0.01f;
                        scriptf.pulsationFrequency.x = 3.35f;
                        scriptf.pulsationFrequency.y = 2.25f;
                        scriptf.pulsationFrequency.z = 5.5f;
                        scriptf.pulsationAmplitudeMax.x = 2;
                        scriptf.pulsationAmplitudeMax.y = 3;
                        scriptf.pulsationAmplitudeMax.z = 1;
                        scriptf.pulsationAmplitudeMin.x = 1;
                        scriptf.pulsationAmplitudeMin.y = 2;
                        scriptf.pulsationAmplitudeMin.z = 0.5f;
                        scriptf.rotationSpeed.x = 150;
                        scriptf.rotationSpeed.y = 130;
                        scriptf.rotationSpeed.z = 170;
                        scriptf.rotationMax.x = 0;
                        scriptf.rotationMax.y = 0;
                        scriptf.rotationMax.z = 0;
                        scriptf.rotationMin.x = 0;
                        scriptf.rotationMin.y = 0;
                        scriptf.rotationMin.z = 0;

                    }
                }
        }
        if (LzwpTracking.Instance.flysticks[0].button4.wasPressed)
        {
            foreach (GameObject b in MasterScript.master.Puzzles)
            {

                RingScript[] scripts = b.GetComponentsInChildren<RingScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    try
                    {
                        if (int.Parse(scripts[i].tag.Substring(5)) % 2 == 0)
                        {
                            scripts[i].right = true;
                            scripts[i].speed = 50;
                 
                        }
                        else
                        {
                            scripts[i].right = false;
                            scripts[i].speed = 300;
                        }
                    }
                    catch (Exception e) { }
                }
            }
            foreach (GameObject b in MasterScript.master.Puzzles)
            {
                FlyingObjectScript[] scripts = b.GetComponentsInChildren<FlyingObjectScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    try
                    {
                        if (int.Parse(scripts[i].tag.Substring(5)) % 2 == 0)
                        {
                            scripts[i].vibrating = true;
                            scripts[i].pulsation = true;
                            scripts[i].rotate = true;
                            scripts[i].bezierSpeed = 1000;
                            scripts[i].vibrationFrequency.x = 2;
                            scripts[i].vibrationFrequency.y = 3;
                            scripts[i].vibrationFrequency.z = 2.5f;
                            scripts[i].vibrationAmplitude.x = 0.01f;
                            scripts[i].vibrationAmplitude.y = 0.01f;
                            scripts[i].vibrationAmplitude.z = 0.01f;
                            scripts[i].pulsationFrequency.x = 0.35f;
                            scripts[i].pulsationFrequency.y = 0.25f;
                            scripts[i].pulsationFrequency.z = 0.5f;
                            scripts[i].pulsationAmplitudeMax.x = 2;
                            scripts[i].pulsationAmplitudeMax.y = 3;
                            scripts[i].pulsationAmplitudeMax.z = 1;
                            scripts[i].pulsationAmplitudeMin.x = 1;
                            scripts[i].pulsationAmplitudeMin.y = 2;
                            scripts[i].pulsationAmplitudeMin.z = 0.5f;
                            scripts[i].rotationSpeed.x = 30;
                            scripts[i].rotationSpeed.y = 30;
                            scripts[i].rotationSpeed.z = 30;
                            scripts[i].rotationMax.x = 0;
                            scripts[i].rotationMax.y = 0;
                            scripts[i].rotationMax.z = 0;
                            scripts[i].rotationMin.x = 0;
                            scripts[i].rotationMin.y = 0;
                            scripts[i].rotationMin.z = 0;
                        }
                        else
                        {
                            scripts[i].vibrating = true;
                            scripts[i].pulsation = true;
                            scripts[i].rotate = true;
                            scripts[i].bezierSpeed = 30000;
                            scripts[i].vibrationFrequency.x = 6;
                            scripts[i].vibrationFrequency.y = 6;
                            scripts[i].vibrationFrequency.z = 6f;
                            scripts[i].vibrationAmplitude.x = 0.01f;
                            scripts[i].vibrationAmplitude.y = 0.01f;
                            scripts[i].vibrationAmplitude.z = 0.01f;
                            scripts[i].pulsationFrequency.x = 3.35f;
                            scripts[i].pulsationFrequency.y = 2.25f;
                            scripts[i].pulsationFrequency.z = 5.5f;
                            scripts[i].pulsationAmplitudeMax.x = 2;
                            scripts[i].pulsationAmplitudeMax.y = 3;
                            scripts[i].pulsationAmplitudeMax.z = 1;
                            scripts[i].pulsationAmplitudeMin.x = 1;
                            scripts[i].pulsationAmplitudeMin.y = 2;
                            scripts[i].pulsationAmplitudeMin.z = 0.5f;
                            scripts[i].rotationSpeed.x = 150;
                            scripts[i].rotationSpeed.y = 130;
                            scripts[i].rotationSpeed.z = 170;
                            scripts[i].rotationMax.x = 0;
                            scripts[i].rotationMax.y = 0;
                            scripts[i].rotationMax.z = 0;
                            scripts[i].rotationMin.x = 0;
                            scripts[i].rotationMin.y = 0;
                            scripts[i].rotationMin.z = 0;
                        }
                    }
                    catch (Exception e) { }
                }
            }
            for (int i = 0; i < 5; i++)
                foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + i.ToString()))
                {
                    RingScript script = a.GetComponent<RingScript>();
                    if (script != null)
                    {
                        
                        try
                        {
                            if (int.Parse(script.tag.Substring(5)) % 2 == 0)
                            {
                                script.speed = 50;
                                script.right = true;
                            }
                            else
                            {
                                script.speed = 300;
                                script.right = false;
                            }
                        }
                        catch (Exception e) { }
                    }
                    FlyingObjectScript scriptf = a.GetComponent<FlyingObjectScript>();
                    if (scriptf != null)
                    {
                        try
                        {
                            if (int.Parse(scriptf.tag.Substring(5)) % 2 == 0)
                            {

                                scriptf.vibrating = true;
                                scriptf.pulsation = true;
                                scriptf.rotate = true;
                                scriptf.bezierSpeed = 1000;
                                scriptf.vibrationFrequency.x = 2;
                                scriptf.vibrationFrequency.y = 3;
                                scriptf.vibrationFrequency.z = 2.5f;
                                scriptf.vibrationAmplitude.x = 0.01f;
                                scriptf.vibrationAmplitude.y = 0.01f;
                                scriptf.vibrationAmplitude.z = 0.01f;
                                scriptf.pulsationFrequency.x = 0.35f;
                                scriptf.pulsationFrequency.y = 0.25f;
                                scriptf.pulsationFrequency.z = 0.5f;
                                scriptf.pulsationAmplitudeMax.x = 2;
                                scriptf.pulsationAmplitudeMax.y = 3;
                                scriptf.pulsationAmplitudeMax.z = 1;
                                scriptf.pulsationAmplitudeMin.x = 1;
                                scriptf.pulsationAmplitudeMin.y = 2;
                                scriptf.pulsationAmplitudeMin.z = 0.5f;
                                scriptf.rotationSpeed.x = 30;
                                scriptf.rotationSpeed.y = 30;
                                scriptf.rotationSpeed.z = 30;
                                scriptf.rotationMax.x = 0;
                                scriptf.rotationMax.y = 0;
                                scriptf.rotationMax.z = 0;
                                scriptf.rotationMin.x = 0;
                                scriptf.rotationMin.y = 0;
                                scriptf.rotationMin.z = 0;
                            }
                            else
                            {

                                scriptf.vibrating = true;
                                scriptf.pulsation = true;
                                scriptf.rotate = true;
                                scriptf.bezierSpeed = 30000;
                                scriptf.vibrationFrequency.x = 6;
                                scriptf.vibrationFrequency.y = 6;
                                scriptf.vibrationFrequency.z = 6f;
                                scriptf.vibrationAmplitude.x = 0.01f;
                                scriptf.vibrationAmplitude.y = 0.01f;
                                scriptf.vibrationAmplitude.z = 0.01f;
                                scriptf.pulsationFrequency.x = 3.35f;
                                scriptf.pulsationFrequency.y = 2.25f;
                                scriptf.pulsationFrequency.z = 5.5f;
                                scriptf.pulsationAmplitudeMax.x = 2;
                                scriptf.pulsationAmplitudeMax.y = 3;
                                scriptf.pulsationAmplitudeMax.z = 1;
                                scriptf.pulsationAmplitudeMin.x = 1;
                                scriptf.pulsationAmplitudeMin.y = 2;
                                scriptf.pulsationAmplitudeMin.z = 0.5f;
                                scriptf.rotationSpeed.x = 150;
                                scriptf.rotationSpeed.y = 130;
                                scriptf.rotationSpeed.z = 170;
                                scriptf.rotationMax.x = 0;
                                scriptf.rotationMax.y = 0;
                                scriptf.rotationMax.z = 0;
                                scriptf.rotationMin.x = 0;
                                scriptf.rotationMin.y = 0;
                                scriptf.rotationMin.z = 0;
                            }
                        }
                        catch (Exception e) { }

                    }
                }
        }


        if (MasterScript.master.environment == MasterScript.Mode.Tunnel)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.up, out hit, 3f))
            {
                if (hit.transform.GetComponent<RingScript>() != null)
                {
                    if (GetComponent<AudioSource>().clip != MasterScript.master.AudioItems[0])
                    {
                        GetComponent<AudioSource>().clip = MasterScript.master.AudioItems[0];
                        GetComponent<AudioSource>().Play();
                    }
                }
                else if (GetComponent<AudioSource>().clip != MasterScript.master.AudioItems[1])
                {
                    GetComponent<AudioSource>().clip = MasterScript.master.AudioItems[1];
                    GetComponent<AudioSource>().Play();
                }

            }
        }
        else if (GetComponent<AudioSource>().clip != MasterScript.master.AudioItems[0])
        {
            GetComponent<AudioSource>().clip = MasterScript.master.AudioItems[0];
            GetComponent<AudioSource>().Play();
        }
        //RotateView();
        if (MasterScript.master.environment == MasterScript.Mode.OpenSpace)
        {
            Vector3 displacement = new Vector3(transform.position.x, 0, transform.position.z);
            transform.position = new Vector3(0, transform.position.y, 0);
            foreach (GameObject a in flyingObjects)
            {
                a.transform.position = new Vector3(a.transform.position.x - displacement.x, a.transform.position.y, a.transform.position.z - displacement.z);
                FlyingObjectScript script = a.GetComponent<FlyingObjectScript>();
                for (int i =0; i < script.bezierPoints.Count; i++)
                {
                    script.bezierPoints[i]= new Vector3(script.bezierPoints[i].x - displacement.x, script.bezierPoints[i].y, script.bezierPoints[i].z-displacement.z);
                }
            }
        }
        // the jump state needs to read here to make sure it is not missed
        if (!m_Jump)
        {
            if (LzwpTracking.Instance.flysticksCount > 0)
                m_Jump = LzwpTracking.Instance.flysticks[0].fire.wasPressed;
        }

        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            //StartCoroutine(m_JumpBob.DoBobCycle());
            PlayLandingSound();
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;
    }


    private void PlayLandingSound()
    {
        m_NextStep = m_StepCycle + .5f;
    }


    private void FixedUpdate()
    {
        float speed;
        GetInput(out speed);
        // always move along the camera forward as it is the direction that it being aimed at
        //Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;










        Vector3 desiredMove = Vector3.zero;

        float hor = 0f;
        float ver = 0f;

        if (LzwpTracking.Instance.flysticksCount > 0)
        {
            hor = LzwpTracking.Instance.flysticks[0].joystickHorizontal;
            ver = LzwpTracking.Instance.flysticks[0].joystickVertical;
        }


        Vector3 rotation = new Vector3(0, hor, 0) * lookSensitivity * Time.deltaTime;

        //if (rotation != Vector3.zero && LzwpInput.Instance.flysticks[0].fire.isActive)
        //    transform.Rotate(rotation);

        if (rotation != Vector3.zero)
            transform.Rotate(rotation);


        Vector3 pos = transform.position;

        if (Mathf.Abs(ver) >= moveThreshold)
        {
            //pos += (LZWPlib.Input.Instance.flysticks[0].rotation * Vector3.forward) * moveSpeed * ver;
            desiredMove += transform.rotation * (LzwpTracking.Instance.flysticks[0].rotation * Vector3.forward) * moveSpeed * ver * Time.deltaTime;
            //pos += flystick.localRotation * Vector3.forward * moveSpeed * ver;
        }




        desiredMove.y = 0f;










        // get a normal for the surface that is being touched to move along it
        RaycastHit hitInfo;
        Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                            m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
        desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

        m_MoveDir.x = desiredMove.x * speed;
        m_MoveDir.z = desiredMove.z * speed;


        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                PlayJumpSound();
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

        ProgressStepCycle(speed);
        //UpdateCameraPosition(speed);

        //m_MouseLook.UpdateCursorLock();
    }


    private void PlayJumpSound()
    {
    }


    private void ProgressStepCycle(float speed)
    {
        if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
        {
            m_StepCycle += (m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                            Time.fixedDeltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }


    private void PlayFootStepAudio()
    {
        if (!m_CharacterController.isGrounded)
        {
            return;
        }
    }


    /*
    private void UpdateCameraPosition(float speed)
    {
        Vector3 newCameraPosition;
        if (!m_UseHeadBob)
        {
            return;
        }
        if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
        {
            m_Camera.transform.localPosition = m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude + (speed * (m_IsWalking ? 1f : m_RunstepLenghten)));
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
        }
        else
        {
            newCameraPosition = m_Camera.transform.localPosition;
            newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
        }
        m_Camera.transform.localPosition = newCameraPosition;
    }
    */


    private void GetInput(out float speed)
    {

        float horizontal = 0f;
        float vertical = 0f;

        // Read input
        if (LzwpTracking.Instance.flysticksCount > 0)
        {
            horizontal = LzwpTracking.Instance.flysticks[0].joystickHorizontal;
            vertical = LzwpTracking.Instance.flysticks[0].joystickVertical;
        }

        bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
        // On standalone builds, walk/run speed is modified by a key press.
        // keep track of whether or not the character is walking or running
        m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
        // set the desired speed to be walking or running
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);

        // normalize input if it exceeds 1 in combined length:
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }

        // handle speed change to give an fov kick
        // only if the player is going to a run, is running and the fovkick is to be used
        if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
        {
            StopAllCoroutines();
            //StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
        }
    }


    private void RotateView()
    {
        //m_MouseLook.LookRotation (transform, m_Camera.transform);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        //dont move the rigidbody if the character is on top of it
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }
}
