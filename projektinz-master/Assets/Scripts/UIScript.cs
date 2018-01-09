using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {
    public string parameter;
    private Dropdown dropdown;

    public void OnChange()
    {
        if (parameter == "speed")
        {
            float speedo;
            foreach (GameObject b in MasterScript.master.Puzzles)
            {

                RingScript[] scripts = b.GetComponentsInChildren<RingScript>();
                for (int i = 0; i < scripts.Length; i++)
                {
                    if (scripts[i].tag == "Grupa " + dropdown.options[dropdown.value].text)
                        if (float.TryParse(this.GetComponent<InputField>().text, out speedo))
                            scripts[i].speed = speedo;

                }
            }
        }
        else
        {
            float param;
            foreach (GameObject b in MasterScript.master.Puzzles)
            {
                FlyingObjectScript[] scripts = b.GetComponentsInChildren<FlyingObjectScript>();
                if (float.TryParse(this.GetComponent<InputField>().text, out param))
                    for (int i = 0; i < scripts.Length; i++)
                    {
                        if (scripts[i].tag == "Grupa " + dropdown.options[dropdown.value].text)
                            switch (parameter)
                            {
                                case "bs":
                                    scripts[i].bezierSpeed = (int)param;
                                    break;
                                case "vfx":
                                    scripts[i].vibrationFrequency.x = param;
                                    break;
                                case "vfy":
                                    scripts[i].vibrationFrequency.y = param;
                                    break;
                                case "vfz":
                                    scripts[i].vibrationFrequency.z = param;
                                    break;
                                case "vax":
                                    scripts[i].vibrationAmplitude.x = param;
                                    break;
                                case "vay":
                                    scripts[i].vibrationAmplitude.y = param;
                                    break;
                                case "vaz":
                                    scripts[i].vibrationAmplitude.z = param;
                                    break;
                                case "pfx":
                                    scripts[i].pulsationFrequency.x = param;
                                    break;
                                case "pfy":
                                    scripts[i].pulsationFrequency.y = param;
                                    break;
                                case "pfz":
                                    scripts[i].pulsationFrequency.z = param;
                                    break;
                                case "pamaxx":
                                    scripts[i].pulsationAmplitudeMax.x = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "pamaxy":
                                    scripts[i].pulsationAmplitudeMax.y = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "pamaxz":
                                    scripts[i].pulsationAmplitudeMax.z = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "paminx":
                                    scripts[i].pulsationAmplitudeMin.x = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "paminy":
                                    scripts[i].pulsationAmplitudeMin.y = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "paminz":
                                    scripts[i].pulsationAmplitudeMin.z = param;
                                    scripts[i].calculateDiff();
                                    break;
                                case "rsx":
                                    scripts[i].rotationSpeed.x = param;
                                    break;
                                case "rsy":
                                    scripts[i].rotationSpeed.y = param;
                                    break;
                                case "rsz":
                                    scripts[i].rotationSpeed.z = param;
                                    break;
                                case "rmaxx":
                                    scripts[i].rotationMax.x = param;
                                    break;
                                case "rmaxy":
                                    scripts[i].rotationMax.y = param;
                                    break;
                                case "rmaxz":
                                    scripts[i].rotationMax.z = param;
                                    break;
                                case "rminx":
                                    scripts[i].rotationMin.x = param;
                                    break;
                                case "rminy":
                                    scripts[i].rotationMin.y = param;
                                    break;
                                case "rminz":
                                    scripts[i].rotationMin.z = param;
                                    break;
                            }
                    }
            }
        }

            foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + dropdown.options[dropdown.value].text))
            {
                if (parameter == "speed")
                {
                    float newspeed;
                    RingScript script = a.GetComponent<RingScript>();
                    if (script != null)
                    {
                        if (float.TryParse(this.GetComponent<InputField>().text, out newspeed))
                        {

                            script.speed = newspeed;
                        }
                    }
                }
                else
                {
                    float newPar;
                    

                        

                        
                    FlyingObjectScript script = a.GetComponent<FlyingObjectScript>();
                    if (script != null)
                    {

                        if (float.TryParse(this.GetComponent<InputField>().text, out newPar))
                            switch (parameter)
                            {
                                case "bs":
                                    script.bezierSpeed = (int)newPar;
                                    break;
                                case "vfx":
                                    script.vibrationFrequency.x = newPar;
                                    break;
                                case "vfy":
                                    script.vibrationFrequency.y = newPar;
                                    break;
                                case "vfz":
                                    script.vibrationFrequency.z = newPar;
                                    break;
                                case "vax":
                                    script.vibrationAmplitude.x = newPar;
                                    break;
                                case "vay":
                                    script.vibrationAmplitude.y = newPar;
                                    break;
                                case "vaz":
                                    script.vibrationAmplitude.z = newPar;
                                    break;
                                case "pfx":
                                    script.pulsationFrequency.x = newPar;
                                    break;
                                case "pfy":
                                    script.pulsationFrequency.y = newPar;
                                    break;
                                case "pfz":
                                    script.pulsationFrequency.z = newPar;
                                    break;
                                case "pamaxx":
                                    script.pulsationAmplitudeMax.x = newPar;
                                    script.calculateDiff();
                                    break;
                                case "pamaxy":
                                    script.pulsationAmplitudeMax.y = newPar;
                                    script.calculateDiff();
                                    break;
                                case "pamaxz":
                                    script.pulsationAmplitudeMax.z = newPar;
                                    script.calculateDiff();
                                    break;
                                case "paminx":
                                    script.pulsationAmplitudeMin.x = newPar;
                                    script.calculateDiff();
                                    break;
                                case "paminy":
                                    script.pulsationAmplitudeMin.y = newPar;
                                    script.calculateDiff();
                                    break;
                                case "paminz":
                                    script.pulsationAmplitudeMin.z = newPar;
                                    script.calculateDiff();
                                    break;
                                case "rsx":
                                    script.rotationSpeed.x = newPar;
                                    break;
                                case "rsy":
                                    script.rotationSpeed.y = newPar;
                                    break;
                                case "rsz":
                                    script.rotationSpeed.z = newPar;
                                    break;
                                case "rmaxx":
                                    script.rotationMax.x = newPar;
                                    break;
                                case "rmaxy":
                                    script.rotationMax.y = newPar;
                                    break;
                                case "rmaxz":
                                    script.rotationMax.z = newPar;
                                    break;
                                case "rminx":
                                    script.rotationMin.x = newPar;
                                    break;
                                case "rminy":
                                    script.rotationMin.y = newPar;
                                    break;
                                case "rminz":
                                    script.rotationMin.z = newPar;
                                    break;
                            }
                    }
                }
            }
    }
    

    // Use this for initialization
    void Start () {
        dropdown = GameObject.FindGameObjectWithTag("drop").GetComponent<Dropdown>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
