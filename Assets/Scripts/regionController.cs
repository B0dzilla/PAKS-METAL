using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class regionController : MonoBehaviour {

	public int selectedRegion = 1;
	public GameObject dropDownList;

	// Use this for initialization
	void Start () {
		
	}
	
	public void changeRegion() {
		selectedRegion = (dropDownList.GetComponent<Dropdown> ().value) + 1;
	}
}
