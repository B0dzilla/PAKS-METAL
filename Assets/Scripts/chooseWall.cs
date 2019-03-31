using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class chooseWall : MonoBehaviour {


	//ПЕРЕМЕННЫЕ ДЛЯ АНИМАЦИИ
	public GameObject mainObject;
	public Sprite lone, lone_higl, angle, angle_higl;
	public GameObject btn_1, btn_2, text1, text2;
	public Transform choose1, choose2, mainObjectPosition;
	public float smoothTime = 0.15F;
	float xVelocity = 0.0F, xVelocity1 = 0.0F, offset;
	int selected = 0; 

	// Use this for initialization
	void Start () {
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, mainObject.transform.position.y -300.0f, mainObject.transform.position.z);
		btn_1.GetComponent<Image> ().sprite = lone;
		btn_2.GetComponent<Image> ().sprite = angle;
	}
	
	// Update is called once per frame
	void Update () {

		float newPosition1 = Mathf.SmoothDamp (mainObject.transform.position.y, mainObjectPosition.position.y, ref xVelocity1, smoothTime);
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, newPosition1, mainObject.transform.position.z);

		if (selected == 2) {
			btn_1.GetComponent<Image> ().sprite = lone;
			btn_2.GetComponent<Image> ().sprite = angle_higl;
			text1.SetActive (false);
			text2.SetActive (true);
		} 
		else if (selected == 1) {
			btn_2.GetComponent<Image> ().sprite = angle;
			btn_1.GetComponent<Image> ().sprite = lone_higl;
			text1.SetActive (true);
			text2.SetActive (false);
		}
	}

	public void onMouseEnt (int i){
		selected = i;
	}
}
