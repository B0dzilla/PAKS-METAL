using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intAnimation : MonoBehaviour {

	public GameObject logo, helpButton, restartButton, gotoSiteButton;
	public Transform logoPosition, helpButtonPosition;
	public float btnSpacing = 20.0f;
	public float smoothTime = 0.15F;
	float xVelocity1 = 0.0F, xVelocity2 = 0.0F, xVelocity3 = 0.0F;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		float newPosition1 = Mathf.SmoothDamp (logo.transform.position.y, logoPosition.position.y, ref xVelocity1, smoothTime);
		logo.transform.position = new Vector3 (logo.transform.position.x, newPosition1, logo.transform.position.z);
		float newPosition2 = Mathf.SmoothDamp (logo.transform.position.x, logoPosition.position.x, ref xVelocity2, smoothTime);
		logo.transform.position = new Vector3 (newPosition2, logo.transform.position.y, logo.transform.position.z);
		float newPosition3 = Mathf.SmoothDamp (helpButton.transform.position.x, helpButtonPosition.position.x, ref xVelocity3, smoothTime);
		helpButton.transform.position = new Vector3 (newPosition3, logoPosition.transform.position.y, helpButton.transform.position.z);

		//restartButton.transform.position = new Vector3 (helpButton.transform.position.x - Screen.width/20 - 28.5f, helpButton.transform.position.y, helpButton.transform.position.z);
		//gotoSiteButton.transform.position = new Vector3 (restartButton.transform.position.x - Screen.width/20 - 75f, restartButton.transform.position.y, restartButton.transform.position.z);
	}
}
