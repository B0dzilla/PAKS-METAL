using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class chooseSize : MonoBehaviour {

	public Text sliderValue1, sliderValue2;
	public Slider slider1, slider2;
	float wall1, wall2; 

	public GameObject mainObject;
	public Transform mainObjectPosition;
	public float smoothTime = 0.15F;
	float xVelocity1 = 0.0F;

	// Use this for initialization
	void Start () {
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, mainObject.transform.position.y -300.0f, mainObject.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
		float newPosition1 = Mathf.SmoothDamp (mainObject.transform.position.y, mainObjectPosition.position.y, ref xVelocity1, smoothTime);
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, newPosition1, mainObject.transform.position.z);
	}

	public void onSliderMove(int slider){

		if (slider1.value < 10)
			slider1.value = 10;
		if (slider2.value < 10)
			slider2.value = 10;

		if (slider == 1) {
			wall1 = Mathf.RoundToInt (slider1.value);
			sliderValue1.text = wall1 / 10 + " м.";
		}
		else if (slider == 2) {
			wall2 = Mathf.RoundToInt (slider2.value);
			sliderValue2.text = wall2 / 10 + " м.";
		}
	}


}
