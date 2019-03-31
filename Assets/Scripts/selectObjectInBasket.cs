using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class selectObjectInBasket : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	PriceController pc;
	public string thisGOtext;
	string BufferText;

	// Use this for initialization
	
	// Update is called once per frame
	void Update () {
		thisGOtext = gameObject.transform.GetChild (1).gameObject.GetComponent<Text> ().text;
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		backgroundLight ();
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		MouseExit ();
	}

	public void backgroundLight () {
		objectPrice[] ap = FindObjectsOfType (typeof(objectPrice)) as objectPrice[];
		foreach (objectPrice op in ap) {
			if (op.itemName == thisGOtext)
				op.gameObject.GetComponent<Image> ().color = Color.green;
		}
	}

	public void MouseExit () {
		objectPrice[] ap = FindObjectsOfType (typeof(objectPrice)) as objectPrice[];
		foreach (objectPrice op in ap) {
			if (op.itemName == thisGOtext)
				op.gameObject.GetComponent<Image> ().color = Color.white;
		}
	}
}
