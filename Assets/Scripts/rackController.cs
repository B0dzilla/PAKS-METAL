using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rackController : MonoBehaviour {
    public int slotCount = 3;
    public Image topPart;
    public Image bottomPart;

    public GameObject closeButton;
    public GameObject editButton;

    public List<GameObject> components;
    public bool inEditMode = false;
    private int[] slotsComponentIndex;
    public float[] slotYPosition;

    private void Start() {
        slotsComponentIndex = new int[slotCount];
        for (int i = 0; i < slotsComponentIndex.Length; i++) {
            slotsComponentIndex[i] = -1;
        }
    }
    public void showButton(GameObject button) {
        if (!inEditMode) {
            button.SetActive(true);
        }
    }

    public void hideButton(GameObject button) {
        if (!inEditMode) {
            button.SetActive(false);
        }
    }

    public void hideButtons() {
        closeButton.SetActive(false);
        editButton.SetActive(false);
    }

    public void toggleEditMode() {
        if (inEditMode) {
            inEditMode = false;
            foreach (GameObject component in components) {
                component.GetComponent<rackComponentController>().inEditMod = false;
            }
        }
        else {
            inEditMode = true;
            foreach (GameObject component in components) {
                component.GetComponent<rackComponentController>().inEditMod = true;
            }
        }
    }

    public int GetSlotFromYPosition(GameObject component, out float yPositionOfSlot) {
        RectTransform componentRectTransform = component.GetComponent<RectTransform>();
        float minDelta = Mathf.Abs(componentRectTransform.localPosition.y - slotYPosition[0]);
        int slotIndex = 0;
        for (int i = 1; i < slotYPosition.Length; i++) {
            float delta = Mathf.Abs(componentRectTransform.localPosition.y - slotYPosition[i]);
            if (delta < minDelta) {
                minDelta = delta;
                slotIndex = i;
            }
        }
        int takesSlots = component.GetComponent<rackComponentController>().takesSlots;
        if (slotIndex + takesSlots > slotCount){
            slotIndex = slotCount - takesSlots;
        }
        yPositionOfSlot = slotYPosition[slotIndex];
        return slotIndex;
    }

    public bool CheckIfSlotAvailable(GameObject component, int slot) {
        bool slotAvailable = true;
        int index = components.IndexOf(component);
        int takesSlots = component.GetComponent<rackComponentController>().takesSlots;
        for (int i = 0; i < takesSlots; i++) {
            if (slotsComponentIndex[i + slot] != index && slotsComponentIndex[i + slot] != -1) {
                slotAvailable = false;
                break;
            }
        }
        return slotAvailable;
    }

    public void addComponent(GameObject component, int slot) {
        int takesSlots = component.GetComponent<rackComponentController>().takesSlots;
        component.GetComponent<rackComponentController>().rack = this;
        components.Add(component);
        int index = components.Count - 1;
        for (int i = 0; i < takesSlots; i++) {
            slotsComponentIndex[i + slot] = index;
        }
        FixOrder();
    }
    public void removeComponent(GameObject component) {
        int index = components.IndexOf(component);
        for (int i = 0; i < slotsComponentIndex.Length; i++) {
            if (slotsComponentIndex[i] == index) {
                slotsComponentIndex[i] = -1;
            }
            else if (slotsComponentIndex[i] > index) {
                slotsComponentIndex[i]--;
            }
        }
        components.Remove(component);
        Destroy(component);
        FixOrder();
    }

    public void changeComponentSlot(GameObject component, int newSlot) {
        int takesSlots = component.GetComponent<rackComponentController>().takesSlots;
        int index = components.IndexOf(component);
        for (int i = 0; i < slotsComponentIndex.Length; i++) {
            if (slotsComponentIndex[i] == index) {
                slotsComponentIndex[i] = -1;
            }
        }
        for (int i = 0; i < takesSlots; i++) {
            slotsComponentIndex[i + newSlot] = index;
        }
        FixOrder();
    }

    public void FixOrder() {
        for (int i = slotsComponentIndex.Length - 1; i >= 0; i--) {
            if (slotsComponentIndex[i] != -1) {
                components[slotsComponentIndex[i]].transform.SetAsLastSibling();
            }
        }
        topPart.transform.SetAsLastSibling();
        closeButton.transform.SetAsLastSibling();
        editButton.transform.SetAsLastSibling();
    }
}
