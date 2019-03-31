using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rackComponentController : MonoBehaviour {
    public bool inEditMod = true;
    public rackController rack;
    public int takesSlots = 1;

    public void showButton(GameObject button) {
        if (inEditMod) {
            button.SetActive(true);
        }
    }

    public void hideButton(GameObject button) {
        if (inEditMod) {
            button.SetActive(false);
        }
    }

    public void destroyComponent() {
        rack.removeComponent(this.gameObject);
    }
}
