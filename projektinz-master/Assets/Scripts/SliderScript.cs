using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour {

    public InputField Textbox;
    public Slider CurrentSlider;
	// Use this for initialization
	void Start () {
        CurrentSlider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    // Update is called once per frame
    public void ValueChangeCheck()
    {
        Debug.Log(CurrentSlider.value);
        Textbox.text = CurrentSlider.value.ToString("0.0");
        /* Debug.Log(CurrentSlider.value);
         Textbox.text = CurrentSlider.value.ToString();*/
    }
}
