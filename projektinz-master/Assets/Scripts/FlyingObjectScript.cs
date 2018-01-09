using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LZWPlib;
public class FlyingObjectScript : MonoBehaviour {

    public List<Vector3> bezierPoints;
    public int bezierSpeed=1;
    private double bezierPos = 0;
    private int bezierPair = 0;
    public Vector3 vibrationFrequency;
    public Vector3 vibrationAmplitude;
    public Vector3 vibrationPos = new Vector3(0, 0, 0);
    public Vector3 vibrationDir=new Vector3(1,1,1);
    public bool vibrating;
    public bool pulsation;
    public Vector3 pulsationFrequency;
    public Vector3 pulsationAmplitudeMax;
    public Vector3 pulsationAmplitudeMin;
    public Vector3 baseScale;
    private Vector3 pulsePhase;
    private Vector3 pulseDiff;
    private Vector3 pulseDir = new Vector3(1, 1, 1);
    public Vector3 rotationSpeed;
    public Vector3 rotationMax;
    public Vector3 rotationMin;
    public Vector3 rotationDir = new Vector3(1,1,1);
    private Vector3 rotation=new Vector3(0,0,0);
    private Vector3 startPos = new Vector3(0, 0, 0);
    public bool rotate;
    public int group = 0;
    // Use this for initialization


    public void ChangeGroupNumber(int number)
    {
        group = number;
        transform.tag = "Grupa " + group.ToString();
    }
    void Vibrate()
    {
        vibrationPos.x += vibrationAmplitude.x * vibrationFrequency.x * vibrationDir.x * Time.deltaTime;
        if (vibrationPos.x >= vibrationAmplitude.x)
            vibrationDir.x = -1;
        else if (vibrationPos.x <= -vibrationAmplitude.x)
            vibrationDir.x = 1;

        vibrationPos.y += vibrationAmplitude.y * vibrationFrequency.y * vibrationDir.y * Time.deltaTime;
        if (vibrationPos.y >= vibrationAmplitude.y)
            vibrationDir.y = -1;
        else if (vibrationPos.y <= -vibrationAmplitude.y)
            vibrationDir.y = 1;
        vibrationPos.z += vibrationAmplitude.z * vibrationFrequency.z * vibrationDir.z * Time.deltaTime;
        if (vibrationPos.z >= vibrationAmplitude.z)
            vibrationDir.z = -1;
        else if (vibrationPos.z <= -vibrationAmplitude.z)
            vibrationDir.z = 1;

        transform.Translate(vibrationPos, Space.World);
    }
    void MoveBezier()
    {
        transform.position = BezierCalculation.BezierCurve(bezierPoints[0 + bezierPair * 2], bezierPoints[1 + bezierPair * 2], bezierPoints[2 + bezierPair * 2], bezierPoints[3 + bezierPair * 2], bezierPos);
        if (MasterScript.master.environment==MasterScript.Mode.Tunnel)
            transform.position += transform.parent.position;
        bezierPos += 0.00001 * bezierSpeed*Time.deltaTime;
        if (bezierPos >= 1)
        {
            bezierPos = 0;
            bezierPair++;
            if (bezierPair * 2 + 4 > bezierPoints.Count)
                bezierPair = 0;
        }
    }
    void Pulse()
    {
        pulsePhase.x += pulseDiff.x * pulsationFrequency.x * pulseDir.x * Time.deltaTime;
        if (pulsePhase.x >= pulsationAmplitudeMax.x)
            pulseDir.x = -1;
        else if (pulsePhase.x <= pulsationAmplitudeMin.x)
            pulseDir.x = 1;
        pulsePhase.y += pulseDiff.y * pulsationFrequency.y * pulseDir.y * Time.deltaTime;
        if (pulsePhase.y >= pulsationAmplitudeMax.y)
            pulseDir.y = -1;
        else if (pulsePhase.y <= pulsationAmplitudeMin.y)
            pulseDir.y = 1;
        pulsePhase.z += pulseDiff.z * pulsationFrequency.z * pulseDir.z * Time.deltaTime;
        if (pulsePhase.z >= pulsationAmplitudeMax.z)
            pulseDir.z = -1;
        else if (pulsePhase.z <= pulsationAmplitudeMin.z)
            pulseDir.z = 1;

        if (MasterScript.master.environment == MasterScript.Mode.Tunnel)
            transform.localScale = Vector3.Scale(pulsePhase, new Vector3(100, 100, 100));
        else
            transform.localScale = pulsePhase;
    }

    void Rotate()
    {
            transform.Rotate(rotationSpeed.x* rotationDir.x * Time.deltaTime, rotationSpeed.y * rotationDir.y * Time.deltaTime, rotationSpeed.z * rotationDir.z * Time.deltaTime);
        if (rotationMax.x != rotationMin.x)
        {
            rotation.x += rotationSpeed.x * rotationDir.x * Time.deltaTime;
            if (rotation.x >= rotationMax.x)
                rotationDir.x = -1;
            if (rotation.x <= rotationMin.x)
                rotationDir.x = 1;
        }
        if (rotationMax.y != rotationMin.y)
        {
            rotation.y += rotationSpeed.y * rotationDir.y * Time.deltaTime;
            if (rotation.y >= rotationMax.y)
                rotationDir.y = -1;
            if (rotation.y <= rotationMin.y)
                rotationDir.y = 1;
        }
        if (rotationMax.z != rotationMin.z)
        {
            rotation.z += rotationSpeed.z * rotationDir.z * Time.deltaTime;
            if (rotation.z >= rotationMax.z)
                rotationDir.z = -1;
            if (rotation.z <= rotationMin.z)
                rotationDir.z = 1;
        }

    }

    public void calculateDiff()
    {
        pulseDiff = new Vector3(pulsationAmplitudeMax.x - pulsationAmplitudeMin.x, pulsationAmplitudeMax.y - pulsationAmplitudeMin.y, pulsationAmplitudeMax.z - pulsationAmplitudeMin.z);
    }
    void Start () {
        calculateDiff();
        pulsePhase = new Vector3(pulsationAmplitudeMin.x + pulseDiff.x / 2, pulsationAmplitudeMin.y + pulseDiff.y / 2, pulsationAmplitudeMin.z + pulseDiff.z / 2);
        if (bezierPoints.Count <= 4)
            startPos = transform.position;
        baseScale = transform.localScale;
        transform.tag = "Grupa " + group.ToString();
    }
	
	// Update is called once per frame
	void Update () {
        if (LzwpManager.Instance.isServer)
        {
            if (bezierPoints.Count >= 4)
                MoveBezier();
            else
                transform.position = startPos;
            if (vibrating)
                Vibrate();
            if (pulsation)
                Pulse();
            if (rotate)
                Rotate();
        }

    }
}
