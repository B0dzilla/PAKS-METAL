using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Loading : MonoBehaviour {

	public float loadSpeed;
	public Slider indicator;
	bool loaded = false;

	public GameObject mainObject, lable, logo;
	public Transform mainObjectPosition;
	public float smoothTime = 0.15F;
	float xVelocity1 = 0.0F, xVelocity2 = 0.0F, xVelocity3 = 0.0F;
	int frame;

	// Use this for initialization
	void Start () {
		Application.ExternalCall ("hideprogress");
		StartCoroutine (showLogo());
		OnLoaded ();
		logo.SetActive (false);
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, mainObject.transform.position.y -300.0f, mainObject.transform.position.z);
		lable.transform.position = new Vector3 (lable.transform.position.x, lable.transform.position.y -400.0f, lable.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (showLogo());
		float newPosition1 = Mathf.SmoothDamp (mainObject.transform.position.y, mainObjectPosition.position.y, ref xVelocity1, smoothTime);
		mainObject.transform.position = new Vector3 (mainObject.transform.position.x, newPosition1, mainObject.transform.position.z);
		float newPosition2 = Mathf.SmoothDamp (lable.transform.position.y, mainObjectPosition.position.y + 50.0f, ref xVelocity2, smoothTime);
		lable.transform.position = new Vector3 (lable.transform.position.x, newPosition2, lable.transform.position.z);
		
		if (!loaded && indicator.value < 100f)
			indicator.value += loadSpeed * Time.smoothDeltaTime;
		else if (!loaded) {
			OnLoaded ();
		}
	}

	void OnLoaded(){
		loaded = true;
		pageControl p = GameObject.Find("pageControls").GetComponent<pageControl>();
		p.SetActiveInterfaceLayout (true);
		p.openChooseWall();

	}

	IEnumerator showLogo(){
		yield return new WaitForSeconds (0.4f);
		logo.SetActive (true);
		float newPosition3 = Mathf.SmoothDamp (logo.transform.position.y, mainObjectPosition.position.y - 70.0f, ref xVelocity3, smoothTime);
		logo.transform.position = new Vector3 (logo.transform.position.x, newPosition3, logo.transform.position.z);
	}
}
