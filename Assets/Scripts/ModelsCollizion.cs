using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ModelsCollizion : MonoBehaviour {

    public bool isRack = false;

	public bool collision;

    public bool onRightWall;
    public bool inSlot1;
    public bool inSlot2;
    public bool inSlotTotal;
	public bool isObjectCollisionWithPolka = false;
	public bool isObjectCollisionWithBigPolka = false;
	public bool isPolkaBusy = false;
	Collider2D polka, bigPolka, perekladina;
    public float slot1X {
        get {
            if (slot1Collider) {
                return slot1Collider.gameObject.transform.position.x;
            }
            else return 0;
        }
        private set { }

    }
    public float slot2X {
        get
        {
            if (slot2Collider) {
                return slot2Collider.gameObject.transform.position.x;
            }
            else return 0;
        }
        private set { }
    }
    
    public float slotTotalX {
        get {
            if (!onRightWall) {
                return slot1X;
            }
            else {
                return slot2X;
            }
        }
        private set { }
    }

    public Collider2D slot1Collider;
    public Collider2D slot2Collider;

    public List<Collider2D> plankCollisions;
    public List<Collider2D> modelCollisions;
	Collider2D leftTruePlank, rightTruePlank;

    public bool isRed;

    void OnTriggerEnter2D(Collider2D col) {
		if (col.name.Contains ("model")) {
			modelCollisions.Add (col);
			collision = true;
		}
		if (col.name.Contains ("plank")) {
			if (!isRack) {
				if (col.gameObject.GetComponent<planksControl>().collidingWithVeryBigPolkas > 0) {
					if (col.gameObject.name == "plank1") {
						leftTruePlank = col;
					} else if (col.gameObject.name == "plank2") {
						rightTruePlank = col;
					}
				}
				plankCollisions.Add (col);
				RecalculateSlots ();
			}
		}
		if (col.gameObject.tag == "Polka" && !col.gameObject.GetComponent<ModelsCollizion>().isPolkaBusy) {
			polka = col;
			isObjectCollisionWithPolka = true;
		}
		if (col.gameObject.tag == "BigPolka" && !col.gameObject.GetComponent<ModelsCollizion>().isPolkaBusy) {
			bigPolka = col;
			isObjectCollisionWithBigPolka = true;
		}
        
    } 

	void OnTriggerExit2D(Collider2D col) {
        if (col.name.Contains("model")) {
            if (modelCollisions.Contains(col)) {
                modelCollisions.Remove(col);
                if (modelCollisions.Count == 0) {
                    collision = false;
                }
            }
        }
        if (col.name.Contains("plank")) {
            if (!isRack) {
				if (col.gameObject.GetComponent<planksControl>().collidingWithVeryBigPolkas > 0) {
					if (col.gameObject.name == "plank1") {
						leftTruePlank = null;
					} else if (col.gameObject.name == "plank2") {
						rightTruePlank = null;
					}
				}
                if (plankCollisions.Contains(col)) {
                    plankCollisions.Remove(col);
                    RecalculateSlots();
                }
            }
        }
		if (col.gameObject.tag == "Polka") {
			polka.GetComponent<ModelsCollizion> ().isPolkaBusy = false;
			polka = null;
			isObjectCollisionWithPolka = false;
		}
		if (col.gameObject.tag == "BigPolka") {
			bigPolka.GetComponent<ModelsCollizion> ().isPolkaBusy = false;
			bigPolka = null;
			isObjectCollisionWithBigPolka = false;
		}
    }

	public void isMouseOver() {
		//this.gameObject.GetComponent<Image> ().color = Color.green;
		foreach(Transform child in GetComponentsInChildren<Transform>())
		{
			if (child.name != "Close")
				child.gameObject.GetComponent<Image> ().color = Color.green;
		}
	}
	public void isMouseExit() {
		foreach(Transform child in GetComponentsInChildren<Transform>())
		{
			if (child.name != "Close")
				child.gameObject.GetComponent<Image> ().color = Color.white;
		}
	}

	void Update () {
		if (gameObject.tag == "Perekladina") {
			if (isObjectCollisionWithPolka) {
				isRed = false;
			} else {
				isRed = true;
			}
				if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2)) {
					if (isObjectCollisionWithPolka) {
						gameObject.transform.SetParent (polka.transform.GetChild (1).transform);
						gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
						polka.gameObject.GetComponent<ModelsCollizion> ().isPolkaBusy = true;
					}
				}
			}
			if (gameObject.tag == "BigPerekladina") {
				if (isObjectCollisionWithBigPolka) {
					isRed = false;
				} else {
					isRed = true;
				}
				if (Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1) || Input.GetMouseButtonUp (2)) {
					if (isObjectCollisionWithBigPolka) {
						gameObject.transform.SetParent (bigPolka.transform.GetChild (1).transform);
						gameObject.transform.localPosition = new Vector3 (0f, 0f, 0f);
						bigPolka.gameObject.GetComponent<ModelsCollizion> ().isPolkaBusy = true;
					}
				}
			}
		}

	void OnDestroy () {
		Planer planer = GameObject.Find("Planer").GetComponent<Planer>();
		if (!onRightWall) {
			planer.calculatePlanks ("left");
		} else {
			planer.calculatePlanks ("right");
		}
	}

    private void RecalculateSlots() {
        if (plankCollisions.Count > 2) {
			if (leftTruePlank != null && rightTruePlank != null) {
				isRed = false;
				inSlot1 = true;
				inSlot2 = true;
				inSlotTotal = true;
				slot1Collider = leftTruePlank;
				slot2Collider = rightTruePlank;
			} else {
				isRed = true;
				inSlot1 = false;
				inSlot2 = false;
				inSlotTotal = false;
				slot1Collider = null;
				slot2Collider = null;
			}
        } else
		if (plankCollisions.Count == 2) {
			//Debug.Log (Mathf.Abs(Mathf.Abs(plankCollisions[0].gameObject.transform.position.x - plankCollisions[1].gameObject.transform.position.x) - gameObject.GetComponent<PolygonCollider2D>().bounds.size.x));
            if (Mathf.Abs(Mathf.Abs(plankCollisions[0].gameObject.transform.position.x - plankCollisions[1].gameObject.transform.position.x) - gameObject.GetComponent<PolygonCollider2D>().bounds.size.x) <= 4) {
                if (plankCollisions[0].gameObject.transform.position.x > plankCollisions[1].gameObject.transform.position.x) {
                    slot1Collider = plankCollisions[1];
                    slot2Collider = plankCollisions[0];
                }
                else {
                    slot1Collider = plankCollisions[0];
                    slot2Collider = plankCollisions[1];
                }
                isRed = false;
                inSlot1 = true;
                inSlot2 = true;
                inSlotTotal = true;
            }
            else {
				isRed = false;
                inSlot1 = false;
                inSlot2 = false;
                inSlotTotal = false;
                slot1Collider = null;
                slot2Collider = null;
            }
        }
        else if (plankCollisions.Count == 1) {
            if (plankCollisions[0].bounds.center.x < gameObject.GetComponent<PolygonCollider2D>().bounds.center.x) {
                slot1Collider = plankCollisions[0];
                slot2Collider = null;
                inSlot1 = true;
                inSlot2 = false;
                inSlotTotal = false;
                isRed = false;
            }
            else {
                slot1Collider = null;
                slot2Collider = plankCollisions[0];
                inSlot1 = false;
                inSlot2 = true;
                inSlotTotal = false;
                isRed = false;
            }
        }
        else {
            isRed = false;
            inSlot1 = false;
            inSlot2 = false;
            inSlotTotal = false;
            slot1Collider = null;
            slot2Collider = null;
        }
    }
}
