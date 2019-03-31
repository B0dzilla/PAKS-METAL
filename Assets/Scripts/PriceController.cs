using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
//using MySql.Data; 
//using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class PriceController : MonoBehaviour {
	public List<objectPrice> allItems = new List<objectPrice>();
	string url = "http://test.paksmet.ru/LoadPrice.php";
	string url1 = "http://test.paksmet.ru/LoadPrice.php";
	public GameObject content;
	public GameObject cameraa;
	public int selectedRegion;
	public float currentSumma = 0;
	public float displayedSumma = 0;
	public float timer = 0f;
	public GameObject displaySummaText;
	public GameObject summaField;

	/*IEnumerator updatePrice () {
		if (currentSumma > 0) {
			summaField.SetActive (true);
			if (Math.Round (displayedSumma, 0) != Math.Round (currentSumma, 0)) {
				displayedSumma += Time.deltaTime * (currentSumma - displayedSumma) * 2;
			} else {
				displayedSumma = currentSumma;
			}
			displayedSumma = Math.Round (displayedSumma, 2, MidpointRounding.AwayFromZero);
			displaySummaText.GetComponent<Text> ().text = Convert.ToString (displayedSumma) +" р.";
		}  else {
			summaField.SetActive (false);
		}
		yield return new WaitForSeconds(0.001f);
		StartCoroutine (updatePrice());
	}
	*/

	IEnumerator GetPriceFromPhp(string id, string value, int multiplier) {
		WWWForm form = new WWWForm ();
		form.AddField ("region_id", selectedRegion);
		form.AddField ("product_id", id);
		form.AddField ("value_id", value);
		WWW www = new WWW (url,form);
		yield return www;
		Regex regex = new Regex(@"[\d]+");
		var match = regex.Match(www.text);
		var val = match.Value;
		yield return new WaitForSeconds(0.1f);
		ChangeSumma ();
		//string value = data.Substring(data.IndexOf(index)+index.Length);
		//if(value.Contains("|"))value = value.Remove(value.IndexOf("|"));
		//return value;
	}

	IEnumerator GetPriceFromPhp(string id, int multiplier) {
		WWWForm form = new WWWForm ();
		form.AddField ("region_id", selectedRegion);
		form.AddField ("product_id", id);
		WWW www = new WWW (url1,form);
		yield return www;
		Debug.Log (www);
		Regex regex = new Regex(@"[\d]+");
		var match = regex.Match(www.text);
		var val = match.Value;
		yield return new WaitForSeconds(0.1f);
		ChangeSumma ();
		//string value = data.Substring(data.IndexOf(index)+index.Length);
		//if(value.Contains("|"))value = value.Remove(value.IndexOf("|"));
		//return value;
	}

	//void GetPriceFromDB () {
	//	MySql.Data.MySqlClient.MySqlConnection con = new MySqlConnection (mysqlparam);
	//	MySql.Data.MySqlClient.MySqlCommand cmd = new MySqlCommand ("SELECT product_price FROM j25_jshopping_products WHERE product_id = '645';");
	//	cmd.Connection = con;
	//	con.Open ();
	//	MySqlDataReader reader = cmd.ExecuteReader();

	//	while(reader.Read()) {
	//		Debug.Log (reader["product_price"].ToString());
	//	}
	//}

	// Use this for initialization

	public void AddObjectToList (int multiplier) {
		if (multiplier == 1) {
			for (int i = 0; i < allItems.Count; i++) {
				if (allItems [i].amount != 0) {
					Transform tmp = content.transform.GetChild (i);
					Color col = tmp.GetComponent<Image> ().color;
					col.a = 255;
					tmp.GetComponent<Image> ().color = col;
					tmp.transform.GetChild (1).GetComponent<Text> ().text = allItems [i].itemName;
					tmp.transform.GetChild (0).GetComponent<Text> ().text = "x" + allItems [i].amount.ToString ();
					tmp.transform.GetChild (2).GetComponent<Text> ().text = Convert.ToString (allItems [i].price * allItems [i].amount);
					//allItems [i].amount += 1;
				} else {
					allItems.Remove (allItems[i]);
					AddObjectToList (1);
					break;
				}
			}
		}
	}

	public void RefreshList () {
		for (int i = 0; i < allItems.Count+1; i++) {
			Transform tmp = content.transform.GetChild (i);
			Color col = tmp.GetComponent<Image> ().color;
			col.a = 0;
			tmp.GetComponent<Image> ().color = col;
			tmp.transform.GetChild(0).GetComponent<Text>().text = "";
			tmp.transform.GetChild(1).GetComponent<Text>().text = "";
			tmp.transform.GetChild(2).GetComponent<Text>().text = "";
		}
		AddObjectToList (1);
	}
	
	public void ChangeSumma () {
		//currentSumma = currentSumma + (Convert.ToSingle(price) * multiplier);
		currentSumma = 0f;
		for (int i = 0; i < allItems.Count; i++) {
			Transform tmp = content.transform.GetChild (i);
			float price = Convert.ToSingle(tmp.transform.GetChild (2).GetComponent<Text> ().text);
			currentSumma += price;
		}
		//currentSumma = Math.Round (currentSumma, 0, MidpointRounding.AwayFromZero);
	}

	void Awake () {
		summaField.SetActive (false);
		selectedRegion = cameraa.GetComponent<regionController> ().selectedRegion;
		//StartCoroutine (updatePrice());
	}

	void Update () {
		if (currentSumma > 0) {
			summaField.SetActive (true);
			if (displayedSumma != currentSumma) {
				timer += Time.deltaTime;
			}
			if (Math.Round (displayedSumma, 0) != Math.Round (currentSumma, 0)) {
				displayedSumma += Time.deltaTime * ((currentSumma - displayedSumma)) * 10;
				if (timer >= 2f) {
					displayedSumma = currentSumma;
					timer = 0f;
				}
			} else {
				displayedSumma = currentSumma;
			}
			displayedSumma = Mathf.Ceil (displayedSumma);
			displaySummaText.GetComponent<Text> ().text = Convert.ToString (displayedSumma) +" р.";
		}  else {
			summaField.SetActive (false);
		}
	}
}
