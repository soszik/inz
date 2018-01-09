using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonScript : MonoBehaviour {

    public bool val;
    public string parameter;
    private Dropdown dropdown;

    public void OnChange()
    {
        foreach (var a in GameObject.FindGameObjectsWithTag("Grupa " + dropdown.options[dropdown.value].text))
        {
            if (parameter == "rotDir")
            {
                RingScript script = a.GetComponent<RingScript>();
                if (script != null)
                {
                    script.right = val;
                }
            }
            else
            {
                FlyingObjectScript script = a.GetComponent<FlyingObjectScript>();
                if (script != null)
                {
                    switch (parameter)
                    {
                        case "v":
                            script.vibrating = val;
                            break;
                        case "p":
                            script.pulsation = val;
                            break;
                        case "r":
                            script.rotate = val;
                            break;
                    }
                }
            }
        }
    }
    // Use this for initialization
    void Start()
    {
        dropdown = GameObject.FindGameObjectWithTag("drop").GetComponent<Dropdown>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
