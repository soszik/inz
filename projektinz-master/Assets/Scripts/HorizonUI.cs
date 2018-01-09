using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HorizonUI : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnChange()
    {
        float param;
        if (float.TryParse(this.GetComponent<InputField>().text, out param))
        {
           MasterScript.master.horizonInstance.transform.localScale = new Vector3(1, 0.01f*param, 1);
        }

    }

}
