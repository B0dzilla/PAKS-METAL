using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class objectPrice : MonoBehaviour {
	public bool isValueNeeded = false;
	public string id = "0";
	public string value = "0";
	public string itemName = "";
	public int amount = 0;
	public float price = 0.0f;
	private bool isCreated = false;
	string url = "http://test.paksmet.ru/LoadPrice.php";
	string url1 = "http://test.paksmet.ru/LoadPrice_additional.php";
	PriceController pc;
	
	void Awake () {
		Debug.Log ("Заспавнен объект: "+gameObject.name);

		pc = FindObjectOfType<PriceController> ();
		if (isValueNeeded) {
			StartCoroutine (GetPriceFromPhp (id, value, 1));
		} else {
			StartCoroutine (GetPriceFromPhp (id, 1));
		}
		foreach (objectPrice op in pc.allItems) {
			if (op.id == id && op.value == value) {
				op.amount += 1;
				isCreated = true;
			}
		}
		if (!isCreated) {
			pc.allItems.Add (this);
			amount += 1;
		}
	}

	void OnDestroy () {
		Debug.Log ("Удалён объект: "+gameObject.name);
		pc = GameObject.Find("Planer").GetComponent<PriceController>();
		if (isValueNeeded) {
			pc.StartCoroutine (GetPriceFromPhp (id, value, -1));
		} else {
			pc.StartCoroutine (GetPriceFromPhp (id, -1));
		}
		foreach (objectPrice op in pc.allItems) {
			if (op.itemName == itemName)
				op.amount -= 1;
			//if (op.amount == 0) {
			//	pc.allItems.Remove (op);
			//}
		}
		if (amount == 0) {
			pc.allItems.Remove (this);
		}
		pc.RefreshList ();
	}

	IEnumerator GetPriceFromPhp(string id, string value, int multiplier) {
		WWWForm form = new WWWForm ();
		form.AddField ("region_id", pc.selectedRegion);
		form.AddField ("product_id", id);
		form.AddField ("value_id", value);
		WWW www = new WWW (url,form);
		yield return www;
        Regex regex = new Regex(@"[\d]+");
        var match = regex.Match(www.text);
        var val = match.Value;
		price = Mathf.Ceil (Convert.ToSingle(val));
		pc.AddObjectToList (multiplier);
		yield return new WaitForSeconds(0.1f);
		pc.ChangeSumma ();
		//string value = data.Substring(data.IndexOf(index)+index.Length);
		//if(value.Contains("|"))value = value.Remove(value.IndexOf("|"));
		//return value;
	}

	IEnumerator GetPriceFromPhp(string id, int multiplier) {
		WWWForm form = new WWWForm ();
		form.AddField ("region_id", pc.selectedRegion);
		form.AddField ("product_id", id);
		WWW www = new WWW (url1,form);
		yield return www;
		Debug.Log (www.text);
		Regex regex = new Regex(@"[\d]+");
		var match = regex.Match(www.text);
		var val = match.Value;
		price = Mathf.Ceil (Convert.ToSingle(val));
		pc.AddObjectToList (multiplier);
		yield return new WaitForSeconds(0.1f);
		pc.ChangeSumma ();
		//Debug.Log ("Добавить к сумме: "+Convert.ToSingle(www.text) +" с множителем: ");
	}



		
}
