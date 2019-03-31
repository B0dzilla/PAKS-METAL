using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RackEditController : MonoBehaviour {

    public GameObject rackSource;
    public GameObject rackWindow;

    private GameObject rackToShow;

    private GameObject dragComponent;
    private int slotToPlace;

    private bool createAndDrag = false;
    private bool placeAvailable = false;
    private Vector3 startingPosition;
    
    void OnEnable() {
        rackToShow = Instantiate(rackSource, rackWindow.transform);
        RectTransform rackRectTransform = rackToShow.GetComponent<RectTransform>();
        rackRectTransform.localScale = new Vector3(1.5f, 1.5f, 1);
        rackRectTransform.localPosition = new Vector3(-90, -80, rackRectTransform.localPosition.z);
        rackRectTransform.localRotation = Quaternion.Euler(new Vector3(0, 0, -4.67f));
        rackToShow.GetComponent<rackController>().hideButtons();
        rackToShow.GetComponent<rackController>().toggleEditMode();
        rackToShow.GetComponent<rackController>().topPart.GetComponent<Image>().raycastTarget = false;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onComponentBeginDragAndCreate(GameObject component) {
        dragComponent = Instantiate(component, rackToShow.transform);
        dragComponent.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, -4.67f));
        createAndDrag = true;
        slotToPlace = -1;
        rackToShow.GetComponent<rackController>().topPart.transform.SetAsLastSibling();
    }
    public void onComponentBeginDrag(GameObject component) {
        if (component.GetComponent<rackComponentController>().inEditMod) {
            dragComponent = component;
            createAndDrag = false;
            startingPosition = dragComponent.GetComponent<RectTransform>().localPosition;
            dragComponent.transform.SetAsLastSibling();
            rackToShow.GetComponent<rackController>().topPart.transform.SetAsLastSibling();
        }
    }

    public void onComponentDrag() {
        if (dragComponent != null) {
            placeAvailable = true;
            RectTransform componentRectTransform = dragComponent.GetComponent<RectTransform>();
            componentRectTransform.position = Input.mousePosition;
            float yPositionToSet;
            rackController rack = rackToShow.GetComponent<rackController>();
            slotToPlace = rack.GetSlotFromYPosition(dragComponent, out yPositionToSet);
            componentRectTransform.anchoredPosition = new Vector3(0, yPositionToSet, 0);
            placeAvailable = rack.CheckIfSlotAvailable(dragComponent, slotToPlace);
            
            if (placeAvailable) {
                dragComponent.GetComponent<Image>().color = Color.green;
            }
            else {
                dragComponent.GetComponent<Image>().color = Color.red;
            }
        }
    }

    public void onComponentEndDrag() {
        if (dragComponent != null) {
            dragComponent.GetComponent<Image>().color = Color.white;
            if (!placeAvailable) {
                if (createAndDrag) {
                    Destroy(dragComponent);
                }
                else {
                    dragComponent.GetComponent<RectTransform>().localPosition = startingPosition;
                }
            }
            else {
                if (createAndDrag) {
                    rackToShow.GetComponent<rackController>().addComponent(dragComponent, slotToPlace);
                }
                else {
                    rackToShow.GetComponent<rackController>().changeComponentSlot(dragComponent, slotToPlace);
                }
            }
            dragComponent = null;
        }
    }

    public void closeRackEditor() {
        Destroy(rackToShow);
        gameObject.SetActive(false);
    }
    public void acceptChanges() {
        rackToShow.GetComponent<rackController>().toggleEditMode();
        rackToShow.GetComponent<rackController>().topPart.GetComponent<Image>().raycastTarget = true;
        rackToShow.transform.parent = rackSource.transform.parent;
        RectTransform rackToShowRectTransform = rackToShow.GetComponent<RectTransform>();
        RectTransform rackSourceRectTransform = rackSource.GetComponent<RectTransform>();
        rackToShowRectTransform.localScale = rackSourceRectTransform.localScale;
        rackToShowRectTransform.localPosition = rackSourceRectTransform.localPosition;
        rackToShowRectTransform.localRotation = rackSourceRectTransform.localRotation;
        Destroy(rackSource);
        gameObject.SetActive(false);
    }
}
