using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interfaceBtns : MonoBehaviour {

	public GameObject restartButton;
	public GameObject restartWindow;


	public void RestartButton () {
		if (!restartWindow.active) {
			restartWindow.SetActive(true);
		}
	}
	public void DeclineButton() {
		restartWindow.SetActive (false);
	}
	public void AcceptButton() {
		restartWindow.SetActive (false);
		Application.LoadLevel (0);
	}
	public void gotoSite () {
		Application.OpenURL("http://test.paksmet.ru/");
	}
}
