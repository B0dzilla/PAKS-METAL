using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Planer : MonoBehaviour {

	public Transform topPannelPosition;
	public GameObject selectedObj;
    public int selected;
    public pageControl pageController;
	public GameObject selector;
	public GameObject topPannel;
	public GameObject[] categoryPannel = new GameObject[8];
	public GameObject movedModel;

	public GameObject wall1transform, wall1;
    public GameObject wall2transform, wall2;
    public GameObject itemsOnWalls1, itemsOnWalls2;

    public GameObject walls;
	public GameObject textLength1, textLength2;
	public GameObject shadow1, shadow2;

    public GameObject Size;
    public float lenght1;
    public float lenght2;
    public bool twoWalls;
    Vector3 dif;
	bool placeAvail = false;

	public float distance = 0f;

	public GameObject RightPannel;
	public GameObject btn;
	public GameObject RightPannelElements;

	public GameObject slotModel1, slotModel2;
	public GameObject topplank1, topplank2;
	public GameObject topplank1m, topplank1_5m;
	public int smallTopPlanks, bigTopPlanks, rightSmallTopPlanks, rightBigTopPlanks;

	public float smoothTime = 0.1F;

	float xVelocity1 = 0.0F, xVelocity = 0.0F, xVelocity2 = 0.0F, xVelocity3 = 0.0F;
	public Slider mashtabSlider;


    private List<GameObject> movedModelsTree;
    private float startingTreePosition;
    private Vector3 startingPosition;
    private GameObject left;
    private GameObject right;
    private float lastPosition;
    private float currentPosition;
    private GameObject movedPanel;

    bool dragAndCreate = false;

    // Use this for initialization
    void Start () {
		topPannel.transform.position = new Vector3 (topPannel.transform.position.x, 0.0f, topPannel.transform.position.z);
        if (!twoWalls) {
			shadow1.SetActive (true);
            lenght1 = Size.GetComponent<chooseSize>().slider1.value;
        }
        else {
			shadow2.SetActive (true);
            lenght1 = Size.GetComponent<chooseSize>().slider2.value;
        }
		RectTransform rt = wall1transform.GetComponent<RectTransform> ();
		rt.sizeDelta = new Vector2 (20*lenght1,484);
		textLength1.GetComponent<Text>().text = Mathf.Round(lenght1)/10 +" м.";
		RectTransform rt1 = topplank1.GetComponent<RectTransform> ();
		rt1.sizeDelta = new Vector2 (20*lenght1,rt1.sizeDelta.y);
        //wall1.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
        if (twoWalls) {
            wall2.SetActive(true);
			RectTransform rektshadow = shadow2.GetComponent<RectTransform>();
			rektshadow.sizeDelta = new Vector2 (0,wall1transform.GetComponent<RectTransform> ().rect.width);
            lenght2 = Size.GetComponent<chooseSize>().slider1.value;
            RectTransform rt2 = wall2transform.GetComponent<RectTransform>();
            rt2.sizeDelta = new Vector2(20 * lenght2, 484);
			textLength2.GetComponent<Text>().text = Mathf.Round(lenght2)/10 +" м.";
            RectTransform rt21 = topplank2.GetComponent<RectTransform>();
            rt21.sizeDelta = new Vector2(20 * lenght2, rt21.sizeDelta.y);
            //wall2.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
        showRightPannel ();
	}
	
	// Update is called once per frame
	void Update () {

		float wallAnimation = Mathf.SmoothDamp (walls.transform.localScale.z, 1.0f, ref xVelocity3, smoothTime);
		if (wallAnimation < 0.98f) walls.transform.localScale = new Vector3 (wallAnimation, wallAnimation, wallAnimation);
		
		if (selected != -1) {
			selector.SetActive (true);	
			float newPosition2 = Mathf.SmoothDamp (selector.transform.position.x, selectedObj.transform.position.x, ref xVelocity2, smoothTime);
			selector.transform.position = new Vector3 (newPosition2, selector.transform.position.y, selector.transform.position.z);
			float newPosition1 = Mathf.SmoothDamp (topPannel.transform.position.y, topPannelPosition.position.y, ref xVelocity, smoothTime);
			topPannel.transform.position = new Vector3 (topPannel.transform.position.x, newPosition1, topPannel.transform.position.z);
		} else {
			selector.SetActive (false);
			float newPosition = Mathf.SmoothDamp (topPannel.transform.position.y, 65.0f, ref xVelocity1, smoothTime);
			topPannel.transform.position = new Vector3 (topPannel.transform.position.x, newPosition, topPannel.transform.position.z);
		}

		float zoom = Input.GetAxis("Mouse ScrollWheel");
		if (walls.transform.localScale.x > 2.2f && zoom > 0f) {
			zoom = 0f;
		}
		if (walls.transform.localScale.x < 0.4f && zoom < 0f) {
			zoom = 0f;
		}
		if (zoom != 0) {
        walls.transform.localScale = new Vector3(walls.transform.localScale.x * (1 + zoom), walls.transform.localScale.y * (1 + zoom), walls.transform.localScale.z);
			mashtabSlider.value = walls.transform.localScale.x;
		}
        RightPannelElements.transform.position = new Vector3 (RightPannelElements.transform.position.x, topPannel.transform.position.y + 150.0f, RightPannelElements.transform.position.z);
		/* планка исчезает когда на неё ничего нет
		if (itemsOnWalls1.transform.childCount < 2) {
			topplank1.SetActive (false);
		}
		if (itemsOnWalls2.transform.childCount < 2) {
			topplank2.SetActive (false);
		}
		*/
	}

	public void ChangeMashtabSlider() {
		walls.transform.localScale = new Vector3 (mashtabSlider.value, mashtabSlider.value, walls.transform.localScale.z);
	}

	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
	}

	public void onSelectCategory (int categoryN){
		if (selected > -1) categoryPannel [selected].SetActive (false);
		selected = categoryN;
		categoryPannel [selected].SetActive (true);
	}

	public void moveSelector(GameObject select){
		selectedObj = select;
	}

	public void closeSelectPannel(){
		categoryPannel [selected].SetActive (false);
		selected = -1;
	}

	public void OnMouseEnter1 (GameObject target){
		
	}

	public void onBeginPlatformMove(){
		dif = Input.mousePosition - walls.transform.position;
	}

	public void onPlatformMove(){
        walls.transform.position = Input.mousePosition - dif;
    }


    public void onPanelBeginDrag(GameObject panel) {
        movedModelsTree = new List<GameObject>();
        startingTreePosition = panel.transform.localPosition.x;
        left = panel;
        right = panel;
        lastPosition = Input.mousePosition.x;
        currentPosition = Input.mousePosition.x;
        movedPanel = panel;
        addToModelsList(panel);
        foreach (GameObject model in movedModelsTree) {
            if (model.name.Contains("plank")) {
                if (model.transform.localPosition.x > right.transform.localPosition.x) {
                    right = model;
                }
                if (model.transform.localPosition.x < left.transform.localPosition.x) {
                    left = model;
                }
            }
        }
    }

    public void onPanelDrag() {
        currentPosition = Input.mousePosition.x;
        float delta = currentPosition - lastPosition;
        if (!movedPanel.GetComponent<planksControl>().onRightWall) {
            if (left.transform.localPosition.x + delta < 575 - lenght1 * 19) {
                delta = (575 - lenght1 * 19) - (left.transform.localPosition.x);
            }
            else if (right.transform.localPosition.x + delta > 555) {
                delta = 555 - (right.transform.localPosition.x);
            }
            foreach (GameObject model in movedModelsTree) {
                model.transform.localPosition = new Vector3(model.transform.localPosition.x + delta, model.transform.localPosition.y + delta * 0.123f, model.transform.localPosition.z);
            }
        }
        else {
            if (left.transform.localPosition.x + delta < 575) {
                delta = 575 - (left.transform.localPosition.x);
            }
            else if (right.transform.localPosition.x + delta > 565 + lenght2 * 16f) {
                delta = (565 + lenght2 * 16f) - (right.transform.localPosition.x);
            }
            foreach (GameObject model in movedModelsTree) {
                model.transform.localPosition = new Vector3(model.transform.localPosition.x + delta, model.transform.localPosition.y - delta * 0.262f, model.transform.localPosition.z);
            }
        }


        placeAvail = true;
        foreach (GameObject model in movedModelsTree) {
			if (model.gameObject.tag != "Perekladina" && model.gameObject.tag != "BigPerekladina") {
				if (model.name.Contains ("plank")) {
					if (model.GetComponent<planksControl> ().isColliding) {
						placeAvail = false;
						break;
					}
				} else if (model.name.Contains ("model")) {
					if (model.GetComponent<ModelsCollizion> ().collision || model.GetComponent<ModelsCollizion> ().isRed) {
						placeAvail = false;
						break;
					}
				}
			}
        }

        if (placeAvail) {
            foreach (GameObject model in movedModelsTree) {
                model.GetComponent<UnityEngine.UI.Image>().color = Color.green;
            }
        }
        else {
            foreach (GameObject model in movedModelsTree) {
                model.GetComponent<UnityEngine.UI.Image>().color = Color.red;
            }
        }
        lastPosition = currentPosition;
		if (!movedPanel.GetComponent<planksControl> ().onRightWall) {
			calculatePlanks ("left");
		} else {
			calculatePlanks ("right");
		}
    }

    public void onPanelEndDrag() {
        float delta = startingTreePosition - movedPanel.transform.localPosition.x;
        foreach (GameObject model in movedModelsTree) {
            model.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            if (!placeAvail) {
                if (!movedPanel.GetComponent<planksControl>().onRightWall) {
                    model.transform.localPosition = new Vector3(model.transform.localPosition.x + delta, model.transform.localPosition.y + delta * 0.123f, model.transform.localPosition.z);
                }
                else {
                    model.transform.localPosition = new Vector3(model.transform.localPosition.x + delta, model.transform.localPosition.y - delta * 0.262f, model.transform.localPosition.z);
                }
            }
            if (model.name.Contains("plank")) {
                model.GetComponent<planksControl>().isMoving = false;
            }
        }
    }

    public void addToModelsList(GameObject model) {
        if (!movedModelsTree.Contains(model)) {
            movedModelsTree.Add(model);
            if (model.name.Contains("plank")) {
                planksControl currentPlankControl = model.GetComponent<planksControl>();
                currentPlankControl.isMoving = true;
                for (int i = 0; i < currentPlankControl.collisions.Count; i++) {
                    addToModelsList(currentPlankControl.collisions[i].gameObject);
                }
            }
            else if (model.name.Contains("model")) {
                addToModelsList(model.GetComponent<ModelsCollizion>().slot1Collider.gameObject);
                addToModelsList(model.GetComponent<ModelsCollizion>().slot2Collider.gameObject);
            }
        }
    }


    public void onModelBeginDrag(GameObject model) {
        if (model.GetComponent<ModelsCollizion>().isRack && model.GetComponent<rackController>().inEditMode) {
            return;
        }
        movedModel = model;
        movedModel.transform.SetParent(GameObject.Find("Walls").transform);
        movedModel.transform.SetParent(GameObject.Find("ItemsOnWall2").transform);
        dragAndCreate = false;
        startingPosition = movedModel.transform.localPosition;
    }

	public void onModelBeginDragWithCreate(GameObject model){
		movedModel =  Instantiate (model as GameObject, GameObject.Find("Walls").transform);
		movedModel.transform.SetParent (GameObject.Find("ItemsOnWall2").transform);
        dragAndCreate = true;
	}

    public void onModelDrag() {
        if (movedModel == null) {
            return;
        }
        movedModel.GetComponent<RectTransform>().position = Input.mousePosition;
        ModelsCollizion movedModelCollizion = movedModel.GetComponent<ModelsCollizion>();
		if (movedModel.gameObject.tag != "Perekladina" && movedModel.gameObject.tag != "BigPerekladina") {
			placeAvail = true;
		}

		if (!movedModelCollizion.isRack) {
			if (!movedModelCollizion.onRightWall) {

				if (!movedModelCollizion.inSlotTotal) {
					if (movedModel.transform.localPosition.x < 575 - lenght1 * 19) {
						movedModel.transform.localPosition = new Vector3 (575 - lenght1 * 19, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
					}
					if (movedModel.transform.localPosition.x > 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ())) {
						if (movedModel.transform.localPosition.x > 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) / 2) {
							if (!twoWalls) {
								movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
							} else {
								if (movedModel.gameObject.tag != "Perekladina" && movedModel.gameObject.tag != "BigPerekladina") {
									movedModelCollizion.onRightWall = true;
									movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
									movedModel.GetComponent<RectTransform> ().rotation = Quaternion.Euler (new Vector3 (0, 38.2f, 0));
									movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
								} else if (movedModel.gameObject.tag == "Perekladina") {
									movedModelCollizion.onRightWall = true;
									movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
									movedModel.GetComponent<RectTransform> ().localRotation = Quaternion.Euler (new Vector3 (0, 38.2f, -12.045f));
									movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
								} else if (movedModel.gameObject.tag == "BigPerekladina") {
									movedModelCollizion.onRightWall = true;
									movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (0.5923035f, 0.5807422f, 0.586951f);
									movedModel.GetComponent<RectTransform> ().localRotation = Quaternion.Euler (new Vector3 (0.7690001f, 186.116f, 14.836f));
									movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
								}
							}
						} else {
							movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
						}
					}
				} else {
					if (dragAndCreate) {
						if (Mathf.Abs (movedModel.transform.position.x - movedModelCollizion.slotTotalX) < 50) {
							movedModel.transform.position = new Vector3 (movedModelCollizion.slotTotalX, movedModel.transform.position.y, movedModel.transform.position.z);
						}
					} else {
						movedModel.transform.position = new Vector3 (movedModelCollizion.slotTotalX, movedModel.transform.position.y, movedModel.transform.position.z);
					}
				}

			} else {
				if (!movedModelCollizion.inSlotTotal) {
						if (movedModel.transform.localPosition.x > 565 + lenght2 * 16f) {
							movedModel.transform.localPosition = new Vector3 (565 + lenght2 * 16f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
						}
						if (movedModel.transform.localPosition.x < 560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f) {
							if (movedModel.transform.localPosition.x < 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) / 2) {
								movedModelCollizion.onRightWall = false;
								movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
								movedModel.GetComponent<RectTransform> ().rotation = Quaternion.Euler (new Vector3 (0, 0, -4.67f));
								movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
							} else {
								movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
							}
						}
				} else {
					if (dragAndCreate) {
						if (Mathf.Abs (movedModel.transform.position.x - movedModelCollizion.slotTotalX) < 50) {
							movedModel.transform.position = new Vector3 (movedModelCollizion.slotTotalX, movedModel.transform.position.y, movedModel.transform.position.z);
						}
					} else {
						movedModel.transform.position = new Vector3 (movedModelCollizion.slotTotalX, movedModel.transform.position.y, movedModel.transform.position.z);
					}
				}
			}

			if (!movedModelCollizion.onRightWall) {
					if (movedModel.transform.localPosition.y > movedModel.transform.localPosition.x * 0.123 + 310) {
						movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f + 310f, movedModel.transform.localPosition.z);
					}
					if (movedModel.transform.localPosition.y < movedModel.transform.localPosition.x * 0.123) {
						movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f, movedModel.transform.localPosition.z);
					}
			} else {
				if (movedModel.transform.localPosition.y > 527.904f - movedModel.transform.localPosition.x * 0.262f) {
					movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, 527.904f - movedModel.transform.localPosition.x * 0.262f, movedModel.transform.localPosition.z);
				}
				if (movedModel.transform.localPosition.y < 217.904f - movedModel.transform.localPosition.x * 0.262f) {
					movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, 217.904f - movedModel.transform.localPosition.x * 0.262f, movedModel.transform.localPosition.z);
				}
			}
			bool snapSuccessful = true;
			Vector3 lastPosition = movedModel.transform.localPosition;
			if (!movedModelCollizion.onRightWall) {
				if (movedModelCollizion.inSlot1 && !movedModelCollizion.inSlot2) {
					if (Mathf.Abs (movedModel.transform.position.x - movedModelCollizion.slot1X) < 30) {
						movedModel.transform.position = new Vector2 (movedModelCollizion.slot1X, movedModel.transform.position.y);
						if (movedModel.transform.localPosition.x > 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ())) {
							placeAvail = false;
							movedModel.transform.localPosition = lastPosition;
						}
					} else {
						snapSuccessful = false;
					}
				} else if (movedModelCollizion.inSlot2 && !movedModelCollizion.inSlot1) {
					if (Mathf.Abs (movedModel.transform.position.x - (movedModelCollizion.slot2X - movedModelCollizion.GetComponent<PolygonCollider2D> ().bounds.size.x)) < 30) {
						movedModel.transform.position = new Vector2 (movedModelCollizion.slot2X - movedModelCollizion.GetComponent<PolygonCollider2D> ().bounds.size.x, movedModel.transform.position.y);
						if (movedModel.transform.localPosition.x < 575 - lenght1 * 19) {
							placeAvail = false;
							movedModel.transform.localPosition = lastPosition;
						}
					} else {
						snapSuccessful = false;
					}
				}
			} else {
				if (movedModelCollizion.inSlot1 && !movedModelCollizion.inSlot2) {
					if (Mathf.Abs (movedModel.transform.position.x - (movedModelCollizion.slot1X + movedModelCollizion.GetComponent<PolygonCollider2D> ().bounds.size.x)) < 30) {
						movedModel.transform.position = new Vector2 (movedModelCollizion.slot1X + movedModelCollizion.GetComponent<PolygonCollider2D> ().bounds.size.x, movedModel.transform.position.y);
						if (movedModel.transform.localPosition.x > 565 + lenght2 * 16f) {
							placeAvail = false;
							movedModel.transform.localPosition = lastPosition;
						}
					} else {
						snapSuccessful = false;
					}
				} else if (movedModelCollizion.inSlot2 && !movedModelCollizion.inSlot1) {
					if (Mathf.Abs (movedModel.transform.position.x - movedModelCollizion.slot2X) < 30) {
						movedModel.transform.position = new Vector2 (movedModelCollizion.slot2X, movedModel.transform.position.y);
						if (movedModel.transform.localPosition.x < 560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f) {
							placeAvail = false;
							movedModel.transform.localPosition = lastPosition;
						}
					} else {
						snapSuccessful = false;
					}
				}
			}
			if (movedModelCollizion.isRed || movedModelCollizion.collision || ((movedModelCollizion.inSlot2 || movedModelCollizion.inSlot1) && !snapSuccessful)) {
				placeAvail = false;
				movedModel.transform.localPosition = lastPosition;
				if (movedModel.gameObject.tag == "Perekladina" && movedModelCollizion.isObjectCollisionWithPolka == true && !movedModelCollizion.GetComponent<ModelsCollizion> ().isPolkaBusy) {
					placeAvail = true;
				}
				if (movedModel.gameObject.tag == "BigPerekladina" && movedModelCollizion.isObjectCollisionWithBigPolka == true && !movedModelCollizion.GetComponent<ModelsCollizion> ().isPolkaBusy) {
					placeAvail = true;
				}
				if (movedModel.gameObject.tag == "Perekladina" && movedModelCollizion.GetComponent<ModelsCollizion> ().isPolkaBusy) {
					placeAvail = false;
				}
				if (movedModel.gameObject.tag == "BigPerekladina" && movedModelCollizion.GetComponent<ModelsCollizion> ().isPolkaBusy) {
					placeAvail = false;
				}
			}

			if (placeAvail) {
				movedModel.GetComponent<UnityEngine.UI.Image> ().color = Color.green;
			} else {
				movedModel.GetComponent<UnityEngine.UI.Image> ().color = Color.red;
			}
		} else {
				if (!movedModelCollizion.onRightWall) {
					if (movedModel.transform.localPosition.x < 575 - lenght1 * 19) {
						movedModel.transform.localPosition = new Vector3 (575 - lenght1 * 19, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
					}
					if (movedModel.transform.localPosition.x > 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ())) {
						if (movedModel.transform.localPosition.x > 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) / 2) {
							if (!twoWalls) {
								movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
							} else {
								movedModelCollizion.onRightWall = true;
								movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
								movedModel.GetComponent<RectTransform> ().rotation = Quaternion.Euler (new Vector3 (0, 0, -2.72f));
								movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
							}
						} else {
							movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
						}
					}

					movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f - 50, movedModel.transform.localPosition.z);
				} else {
					if (movedModel.transform.localPosition.x > 565 + lenght2 * 16f) {
						movedModel.transform.localPosition = new Vector3 (565 + lenght2 * 16f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
					}
					if (movedModel.transform.localPosition.x < 560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f) {
						if (movedModel.transform.localPosition.x < 560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) / 2) {
							movedModelCollizion.onRightWall = false;
							movedModel.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
							movedModel.GetComponent<RectTransform> ().rotation = Quaternion.Euler (new Vector3 (0, 0, -4.67f));
							movedModel.transform.localPosition = new Vector3 (560 - polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 1.05f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
						} else {
							movedModel.transform.localPosition = new Vector3 (560 + polygonCollider2DWidth (movedModel.GetComponent<PolygonCollider2D> ()) * 0.9f, movedModel.transform.localPosition.y, movedModel.transform.localPosition.z);
						}
					}

					movedModel.transform.localPosition = new Vector3 (movedModel.transform.localPosition.x, 167.904f - movedModel.transform.localPosition.x * 0.262f, movedModel.transform.localPosition.z);

				}

				if (movedModelCollizion.collision && movedModel.gameObject.tag != "Perekladina" && movedModel.gameObject.tag != "BigPerekladina") {
					placeAvail = false;
				}

				foreach (UnityEngine.UI.Image im in movedModel.GetComponentsInChildren<UnityEngine.UI.Image>()) {
					if (placeAvail) {
						im.color = Color.green;
					} else {
						im.color = Color.red;
					}
				}
		}

        
	}

    public float polygonCollider2DWidth(PolygonCollider2D polygon) {
        if (polygon.points.Length > 0) {
            float minX = polygon.points[0].x;
            float maxX = polygon.points[0].x;
            for (int i = 1; i < polygon.points.Length; i++) {
                if (polygon.points[i].x > maxX) {
                    maxX = polygon.points[i].x;
                }
                if (polygon.points[i].x < minX) {
                    minX = polygon.points[i].x;
                }
            }
            return maxX - minX;
        }
        else {
            return 0;
        }
    }

	public void onModelEndDrag(){
        if (movedModel == null) {
            return;
        }

        if (!movedModel.GetComponent<ModelsCollizion>().isRack) {
            movedModel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else {
            foreach (UnityEngine.UI.Image im in movedModel.GetComponentsInChildren<UnityEngine.UI.Image>()) {
                im.color = Color.white;
            }
        }
        if (!placeAvail) {
            if (dragAndCreate) {
                Destroy(movedModel);
            }
            else {
                movedModel.transform.localPosition = startingPosition;
            }
		}
		else {
            if (!movedModel.GetComponent<ModelsCollizion>().onRightWall) {
                movedModel.transform.SetParent(GameObject.Find("ItemsOnWall1").transform);
            }
            else {
                movedModel.transform.SetParent(GameObject.Find("ItemsOnWall2").transform);
            }
            placeAvail = false;
            if (!movedModel.GetComponent<ModelsCollizion>().isRack) {
                if (!movedModel.GetComponent<ModelsCollizion>().inSlotTotal) {
                    createSlot(420.0f - movedModel.transform.localPosition.y);
                }
                else {
                    AdjustHeight(420.0f - movedModel.transform.localPosition.y);
                }
            }
            else {
                if (dragAndCreate) {
                    Edit(movedModel.transform.GetChild(0).gameObject);
                }
            }
			//calculatePlanks ();
		}

		movedModel = null;
	}
		
	public void onPereklEndDrag(){
		if (movedModel == null) {
			return;
		}

		if (!movedModel.GetComponent<ModelsCollizion>().isRack) {
			movedModel.GetComponent<UnityEngine.UI.Image>().color = Color.white;
		}
		else {
			foreach (UnityEngine.UI.Image im in movedModel.GetComponentsInChildren<UnityEngine.UI.Image>()) {
				im.color = Color.white;
			}
		}
		if (!placeAvail) {
			if (dragAndCreate) {
				Destroy(movedModel);
			}
			else {
				movedModel.transform.localPosition = startingPosition;
			}
		}
		else {
			if (!movedModel.GetComponent<ModelsCollizion>().onRightWall) {
				movedModel.transform.SetParent(GameObject.Find("ItemsOnWall1").transform);
			}
			else {
				movedModel.transform.SetParent(GameObject.Find("ItemsOnWall2").transform);
			}
			placeAvail = false;
			if (!movedModel.GetComponent<ModelsCollizion>().isRack) {
				if (!movedModel.GetComponent<ModelsCollizion>().inSlotTotal) {
					createSlot(420.0f - movedModel.transform.localPosition.y);
				}
				else {
					AdjustHeight(420.0f - movedModel.transform.localPosition.y);
				}
			}
			else {
				if (dragAndCreate) {
					Edit(movedModel.transform.GetChild(0).gameObject);
				}
			}
		}

		movedModel = null;
	}

	public void calculatePlanks(string swall) {
		
		float minX = 407f, maxX = -498f;
		if (swall == "left") {
			GameObject edgeTopPlank = null;
			foreach (RectTransform child in itemsOnWalls1.GetComponentsInChildren<RectTransform>()) { //вычситываем крайние стойки
				if (child.name.Contains ("plank")) {
					if (child.name == "plank1")
					if (child.localPosition.x < minX) {
						minX = child.localPosition.x;
					}
					if (child.name == "plank2")
					if (child.localPosition.x > maxX) {
						maxX = child.localPosition.x;
					}
					if (child.name == "edgeTopplank1m" || child.name == "edgeTopplank1_5m")
						edgeTopPlank = child.gameObject;
				}
			}
			distance = maxX - minX; //высчитываем дистанцию между крайними стойками

			if (distance < 100f) { //если дистанция меньше минимальной полки то стойки уничтожаются
				if (edgeTopPlank != null) {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}
			} else if (distance >= 100f && distance < 180f) { //спавнится маленькая стойка
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1_5m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+3f, 0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+3f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				if (edgeTopPlank.transform.localPosition.x > 390f) {
					edgeTopPlank.transform.localPosition = new Vector2 (390f, 0f);
				}

			} else if (distance >= 180f && distance < 228f) { //спавниться большая стойка
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 62f, 0f);
					edgeTopPlank.GetComponent<RectTransform> ().sizeDelta = new Vector3(distance+45f,edgeTopPlank.GetComponent<RectTransform>().sizeDelta.y,1f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 62f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				if (edgeTopPlank.transform.localPosition.x > 390f) {
					edgeTopPlank.transform.localPosition = new Vector2 (390f, 0f);
				}

			} else if (distance >= 228f && distance <= 280f) { //тоже большая стойка, просто немного смещена
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 107f, 0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 107f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				if (edgeTopPlank.transform.localPosition.x > 390f) {
					edgeTopPlank.transform.localPosition = new Vector2 (390f, 0f);
				}

			} else if (distance > 280f && distance <= 350f) { //2 метровые стойки, одна обрезается
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1_5m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+3f, 0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+3f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 202f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (202f, 0f);
				//}

				smallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_left").transform);
				topplank_small.transform.localPosition = new Vector2 (maxX-165f, -0.5f);
				topplank_small.transform.SetParent (GameObject.Find ("edgeTopplank1m").transform);

			} else if (distance > 350f && distance <= 370f) { //2 метровые стойки
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1_5m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX, 0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX, 0f);
						foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
							if (child.gameObject.name.Contains ("topplank")) {
								Destroy (child.gameObject);
							}
						}
						smallTopPlanks = 0;
						bigTopPlanks = 0;
				}

				if (edgeTopPlank.transform.localPosition.x > 202f) {
					edgeTopPlank.transform.localPosition = new Vector2 (202f, 0f);
				}
					
				smallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_left").transform);
				topplank_small.transform.localPosition = new Vector2 (((edgeTopPlank.transform.localPosition.x) + 190), -0.5f);
				topplank_small.transform.SetParent (GameObject.Find ("edgeTopplank1m").transform);

			} else if (distance > 370f && distance <= 440f) { //одна полтора метровая и одна метровая обрезанная
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+104f, 0f);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+104f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 214f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (214f, 0f);
				//}
				smallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_left").transform);
				topplank_small.transform.localPosition = new Vector2 (maxX-165f, -0.5f);
				topplank_small.transform.SetParent (GameObject.Find ("edgeTopplank1_5m").transform);

			} else if (distance > 440f && distance <= 468f) { //одна полтора метровая и одна метровая
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, 0f);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, 0f);
						foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
							if (child.gameObject.name.Contains ("topplank")) {
								Destroy (child.gameObject);
							}
						}
						smallTopPlanks = 0;
						bigTopPlanks = 0;
				}

				if (edgeTopPlank.transform.localPosition.x > 214f) {
					edgeTopPlank.transform.localPosition = new Vector2 (214f, 0f);
				}
				smallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_left").transform);
				topplank_small.transform.localPosition = new Vector2 ((edgeTopPlank.transform.localPosition.x + 186f), -0.5f);
				topplank_small.transform.SetParent (GameObject.Find ("edgeTopplank1_5m").transform);

			} else if (distance > 468f && distance <= 530f) { //2 полторы метровые обрезанные
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "edgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, 0f);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 214f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (214f, 0f);
				//}
				smallTopPlanks = 1;
				GameObject topplank_big = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
				topplank_big.transform.SetParent (GameObject.Find ("topplank_left").transform);
				topplank_big.transform.localPosition = new Vector2 (maxX-165f, -0.5f);
				topplank_big.transform.SetParent (GameObject.Find ("edgeTopplank1_5m").transform);

			} else if (distance > 530) { //если дистанция больше большой стойки то спавнить указанное кол-во больших стоек
				float bigPlanks = Mathf.Round ((distance) / 280); //нужное кол-во больших планок
				float smallPlanks;
				if ((distance - (bigPlanks*280)) > 30f) { //280 - длина большой планки
					smallPlanks = 1;
				} else {
					smallPlanks = 0;
				}
				if (edgeTopPlank != null) {
					if (edgeTopPlank.name == "edgeTopplank1m") {
						Destroy (edgeTopPlank);
						edgeTopPlank = null;
					}
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_left").transform);
					edgeTopPlank.gameObject.name = "edgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 116f, 0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 116f, 0f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					smallTopPlanks = 0;
					bigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 110f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (110f, 0f);
				//}

				if (bigTopPlanks == 0) {	
					bigTopPlanks = (int)bigPlanks;
					for (int bp = 1; bp < bigPlanks; bp++) {
						GameObject topplank_big = Instantiate (topplank1_5m as GameObject, itemsOnWalls1.transform);
						topplank_big.transform.SetParent (GameObject.Find ("topplank_left").transform);
						topplank_big.transform.localPosition = new Vector2 (((minX + 116f) + 280f * bp), -0.5f*bp);
						topplank_big.transform.SetParent (GameObject.Find ("edgeTopplank1_5m").transform);
					}
				}
				if ((int)smallPlanks != smallTopPlanks) { //и, если нужно одну маленькую
					smallTopPlanks = 1;
					GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
					topplank_small.transform.SetParent (GameObject.Find ("topplank_left").transform);
					topplank_small.transform.localPosition = new Vector2 (maxX-165f, -0.5f*bigPlanks);
					topplank_small.transform.SetParent (GameObject.Find ("edgeTopplank1_5m").transform);
				}
			}
		}

//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

		if (swall == "right") {
			minX = 573f;
			maxX = -500f;
			GameObject edgeTopPlank = null;
			foreach (RectTransform child in itemsOnWalls2.GetComponentsInChildren<RectTransform>()) { //вычситываем крайние стойки
				if (child.name.Contains ("plank")) {
					if (child.name == "plank2")
					if (child.localPosition.x < minX) {
						minX = child.localPosition.x;
					}
					if (child.name == "plank1")
					if (child.localPosition.x > maxX) {
						maxX = child.localPosition.x+100f;
					}
					if (child.name == "rightEdgeTopplank1m" || child.name == "rightEdgeTopplank1_5m")
						edgeTopPlank = child.gameObject;
				}
			}
			float distance = maxX - minX; //высчитываем дистанцию между крайними стойками
			if (distance < 100f) { //если дистанция меньше минимальной полки то стойки уничтожаются
				if (edgeTopPlank != null) {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}
			} else if (distance >= 100f && distance < 253f) { //спавнится маленькая стойка
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "rightEdgeTopplank1_5m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1m as GameObject, itemsOnWalls2.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX, -3f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0f,0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX, -3f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}
				//Debug.Log (edgeTopPlank.transform.localPosition.x);
				if (edgeTopPlank.transform.localPosition.x > 900f) {
					edgeTopPlank.transform.localPosition = new Vector2 (900f, 0f);
				}

			} else if (distance >= 253f && distance < 345f) { //спавниться большая стойка
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "rightEdgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (maxX - 238f, -3f);
					edgeTopPlank.GetComponent<RectTransform> ().sizeDelta = new Vector3(distance-38f,edgeTopPlank.GetComponent<RectTransform>().sizeDelta.y,1f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0f,0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (maxX - 238f, -3f);
					edgeTopPlank.GetComponent<RectTransform> ().sizeDelta = new Vector3(distance-38f,edgeTopPlank.GetComponent<RectTransform>().sizeDelta.y,1f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 390f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (390f, 0f);
				//}

			} else if (distance > 345f && distance <= 420f) { //2 метровые стойки, одна обрезается
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "rightEdgeTopplank1_5m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX, -3f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0f,0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX, -3f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 202f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (202f, 0f);
				//}

				rightSmallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls1.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_right").transform);
				topplank_small.transform.localPosition = new Vector2 (maxX-210f, -3f);
				topplank_small.transform.SetParent (GameObject.Find ("rightEdgeTopplank1m").transform);
				topplank_small.transform.localRotation = new Quaternion (0f,0f,0f,0f);

				if (topplank_small.transform.localPosition.x > 198f) {
					topplank_small.transform.localPosition = new Vector2 (198f, 0f);
				}

			} else if (distance > 420f && distance <= 500f) { //одна полтора метровая и одна метровая обрезанная
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "rightEdgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+104f, -3f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0f,0f);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+104f, -3f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 214f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (214f, 0f);
				//}
				rightSmallTopPlanks = 1;
				GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls2.transform);
				topplank_small.transform.SetParent (GameObject.Find ("topplank_right").transform);
				topplank_small.transform.localPosition = new Vector2 (maxX-180f, -3f);
				topplank_small.transform.SetParent (GameObject.Find ("rightEdgeTopplank1_5m").transform);
				topplank_small.transform.localRotation = new Quaternion (0f,0f,0f,0f);
				if (topplank_small.transform.localPosition.x > 193f) {
					topplank_small.transform.localPosition = new Vector2 (190f, 0f);
				}

			} else if (distance > 500f && distance <= 560f) { //2 полторы метровые обрезанные
				if (edgeTopPlank != null)
				if (edgeTopPlank.name == "rightEdgeTopplank1m") {
					Destroy (edgeTopPlank);
					edgeTopPlank = null;
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, -3f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0f,0f);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX+102f, -3f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 214f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (214f, 0f);
				//}
				rightSmallTopPlanks = 1;
				GameObject topplank_big = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
				topplank_big.transform.SetParent (GameObject.Find ("topplank_right").transform);
				topplank_big.transform.localPosition = new Vector2 (maxX-165f, -3f);
				topplank_big.transform.SetParent (GameObject.Find ("rightEdgeTopplank1_5m").transform);
				topplank_big.transform.localRotation = new Quaternion (0f,0f,0f,0f);

			} else if (distance > 560) { //если дистанция больше большой стойки то спавнить указанное кол-во больших стоек
				float bigPlanks = Mathf.Round ((distance) / 280); //нужное кол-во больших планок
				float smallPlanks;
				if ((distance - (bigPlanks*280)) > 30f) { //280 - длина большой планки
					smallPlanks = 1;
				} else {
					smallPlanks = 0;
				}
				if (edgeTopPlank != null) {
					if (edgeTopPlank.name == "rightEdgeTopplank1m") {
						Destroy (edgeTopPlank);
						edgeTopPlank = null;
					}
				}

				if (edgeTopPlank == null) {	
					edgeTopPlank = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
					edgeTopPlank.transform.SetParent (GameObject.Find ("topplank_right").transform);
					edgeTopPlank.gameObject.name = "rightEdgeTopplank1_5m";
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 116f, 0f);
					edgeTopPlank.transform.localRotation = new Quaternion (0f,0f,0.3f,0f);
					//edgeTopPlank.GetComponent<RectTransform> ().localPosition = new Vector2 (minX,edgeTopPlank.transform.localPosition.y);
				} else {
					edgeTopPlank.transform.localPosition = new Vector2 (minX + 116f, -3f);
					foreach (RectTransform child in edgeTopPlank.GetComponentsInChildren<RectTransform>()) {
						if (child.gameObject.name.Contains ("topplank")) {
							Destroy (child.gameObject);
						}
					}
					rightSmallTopPlanks = 0;
					rightBigTopPlanks = 0;
				}

				//if (edgeTopPlank.transform.localPosition.x > 110f) {
				//	edgeTopPlank.transform.localPosition = new Vector2 (110f, 0f);
				//}

				if (rightBigTopPlanks == 0) {	
					rightBigTopPlanks = (int)bigPlanks;
					for (int bp = 1; bp < bigPlanks; bp++) {
						GameObject topplank_big = Instantiate (topplank1_5m as GameObject, itemsOnWalls2.transform);
						topplank_big.transform.SetParent (GameObject.Find ("topplank_right").transform);
						topplank_big.transform.localPosition = new Vector2 (((minX + 116f) + 280f * bp), -3f);
						topplank_big.transform.SetParent (GameObject.Find ("rightEdgeTopplank1_5m").transform);
						topplank_big.transform.localRotation = new Quaternion (0f,0f,0f,0f);
					}
				}
				if ((int)smallPlanks != rightSmallTopPlanks) { //и, если нужно одну маленькую
					rightSmallTopPlanks = 1;
					GameObject topplank_small = Instantiate (topplank1m as GameObject, itemsOnWalls2.transform);
					topplank_small.transform.SetParent (GameObject.Find ("topplank_right").transform);
					topplank_small.transform.localPosition = new Vector2 (maxX-165f, -3f);
					topplank_small.transform.SetParent (GameObject.Find ("rightEdgeTopplank1_5m").transform);
					topplank_small.transform.localRotation = new Quaternion (0f,0f,0f,0f);
				}
			}
		}
		//Debug.Log ("minimal x = "+minX+" maximal x = "+maxX+" distance = "+ (maxX-minX));
	}

	public void destroy (GameObject target){
		Destroy (target.transform.parent.gameObject);
	}

	public void showCloseBtn (GameObject btn){
		btn.SetActive (true);
	}

	public void hideCloseBtn (GameObject btn){
		btn.SetActive (false);
    }
    public void Edit(GameObject btn) {
        pageController.openRackEditor(btn.transform.parent.gameObject);
    }

    public void showRightPannel(){
		
		btn.transform.Rotate(new Vector3(0.0f, 0.0f, 180.0f));

		if (RightPannel.transform.position.x == Screen.width-Screen.width/4f) {
			RightPannel.transform.position = new Vector2 (Screen.width-40f, RightPannel.transform.position.y);
		} else 
			RightPannel.transform.position = new Vector2 (Screen.width-Screen.width/4f, RightPannel.transform.position.y);
	}
		

	public void createSlot(float height) {
        ModelsCollizion movedModelCollizion = movedModel.GetComponent<ModelsCollizion>();
		if (movedModel.gameObject.tag != "Perekladina" && movedModel.gameObject.tag != "BigPerekladina") {
			if (!movedModelCollizion.onRightWall) {
				GameObject plank1;
				if (!movedModelCollizion.inSlot1) {
					plank1 = Instantiate (slotModel1, GameObject.Find ("Walls").transform);
					plank1.gameObject.name = "plank1";
					plank1.transform.localScale = wall1.transform.localScale;
					plank1.transform.SetParent (GameObject.Find ("ItemsOnWall1").transform);
					plank1.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (height, 335.0f));
					plank1.transform.position = new Vector2 (movedModel.GetComponent<PolygonCollider2D> ().bounds.center.x - movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x, movedModel.transform.position.y);
					plank1.transform.localPosition = new Vector2 (plank1.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f + 335.0f);
					plank1.transform.SetAsFirstSibling ();
					BoxCollider2D plank1Collider = plank1.GetComponent<BoxCollider2D> ();
					RectTransform plank1Rect = plank1.GetComponent<RectTransform> ();
					plank1Collider.offset = new Vector2 (0, plank1Collider.offset.y);
					plank1Collider.size = new Vector2 (plank1Rect.sizeDelta.x + 4, plank1Collider.size.y);
				} else {
					plank1 = movedModelCollizion.slot1Collider.gameObject;
					float plank1MaxHeight = height;
					List<Collider2D> plank1Collisions = plank1.GetComponent<planksControl> ().collisions;
					foreach (Collider2D collision in plank1Collisions) {
						if (plank1MaxHeight < 420.0f - collision.transform.localPosition.y) {
							plank1MaxHeight = 420.0f - collision.transform.localPosition.y;
						}
					}
					if (plank1MaxHeight >= height) {

						plank1.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (plank1MaxHeight, 335.0f));
						plank1.transform.localPosition = new Vector2 (plank1.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f + 335.0f);
					}
				}
				GameObject plank2;
				if (!movedModelCollizion.inSlot2) {
					plank2 = Instantiate (slotModel2, GameObject.Find ("Walls").transform);
					plank2.gameObject.name = "plank2";
					plank2.transform.localScale = wall1.transform.localScale;
					plank2.transform.SetParent (GameObject.Find ("ItemsOnWall1").transform);
					plank2.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (height, 335.0f));
					plank2.transform.position = new Vector2 (movedModel.GetComponent<PolygonCollider2D> ().bounds.center.x + movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x, plank1.transform.position.y + 0.246f * movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x);
					plank2.transform.SetAsFirstSibling ();
					BoxCollider2D plank2Collider = plank2.GetComponent<BoxCollider2D> ();
					RectTransform plank2Rect = plank2.GetComponent<RectTransform> ();
					plank2Collider.offset = new Vector2 (0, plank2Collider.offset.y);
					plank2Collider.size = new Vector2 (plank2Rect.sizeDelta.x + 4, plank2Collider.size.y);
				} else {
					plank2 = movedModelCollizion.slot2Collider.gameObject;
					float plank2MaxHeight = height;
					List<Collider2D> plank2Collisions = plank2.GetComponent<planksControl> ().collisions;
					foreach (Collider2D collision in plank2Collisions) {
						if (plank2MaxHeight < 420.0f - collision.transform.localPosition.y) {
							plank2MaxHeight = 420.0f - collision.transform.localPosition.y;
						}
					}
					if (plank2MaxHeight >= height) {

						plank2.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (plank2MaxHeight, 335.0f));
						plank2.transform.position = new Vector2 (plank2.transform.position.x, plank1.transform.position.y + 0.246f * movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x);
					}
				}

				if (!topplank1.active) {
					topplank1.SetActive (true);
				}
				calculatePlanks ("left");
			} else {
				GameObject plank1;
				if (!movedModelCollizion.inSlot2) {
					plank1 = Instantiate (slotModel1, GameObject.Find ("Walls").transform);
					plank1.gameObject.name = "plank1";
					plank1.transform.localScale = wall2.transform.localScale;
					plank1.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
					plank1.GetComponent<planksControl> ().onRightWall = true;
					plank1.transform.SetParent (GameObject.Find ("ItemsOnWall2").transform);
					plank1.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (height * 0.92f, 335.0f));
					plank1.transform.position = new Vector2 (movedModel.GetComponent<PolygonCollider2D> ().bounds.center.x + movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x, movedModel.transform.position.y);
					plank1.transform.localPosition = new Vector2 (plank1.transform.localPosition.x, 217.904f - movedModel.transform.localPosition.x * 0.262f + 335.0f);
					plank1.transform.SetAsFirstSibling ();
					BoxCollider2D plank1Collider = plank1.GetComponent<BoxCollider2D> ();
					RectTransform plank1Rect = plank1.GetComponent<RectTransform> ();
					plank1Collider.offset = new Vector2 (0, plank1Collider.offset.y);
					plank1Collider.size = new Vector2 (plank1Rect.sizeDelta.x + 4, plank1Collider.size.y);
				} else {
					plank1 = movedModelCollizion.slot2Collider.gameObject;
					float plank1MaxHeight = height;
					List<Collider2D> plank1Collisions = plank1.GetComponent<planksControl> ().collisions;
					foreach (Collider2D collision in plank1Collisions) {
						if (plank1MaxHeight < 420.0f - collision.transform.localPosition.y) {
							plank1MaxHeight = 420.0f - collision.transform.localPosition.y;
						}
					}
					if (plank1MaxHeight >= height) {

						plank1.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (plank1MaxHeight * 0.92f, 335.0f));
						plank1.transform.localPosition = new Vector2 (plank1.transform.localPosition.x, 217.904f - movedModel.transform.localPosition.x * 0.262f + 335.0f);
					}
				}
				GameObject plank2;
				if (!movedModelCollizion.inSlot1) {
					plank2 = Instantiate (slotModel2, GameObject.Find ("Walls").transform);
					plank2.gameObject.name = "plank2";
					plank2.transform.localScale = wall2.transform.localScale;
					plank2.GetComponent<RectTransform> ().localScale = new Vector3 (-1, 1, 1);
					plank2.GetComponent<planksControl> ().onRightWall = true;
					plank2.transform.SetParent (GameObject.Find ("ItemsOnWall2").transform);
					plank2.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (height * 0.92f, 335.0f));
					plank2.transform.position = new Vector2 (movedModel.GetComponent<PolygonCollider2D> ().bounds.center.x - movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x, plank1.transform.position.y + 0.524f * movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x);
					plank2.transform.SetAsFirstSibling ();
					BoxCollider2D plank2Collider = plank2.GetComponent<BoxCollider2D> ();
					RectTransform plank2Rect = plank2.GetComponent<RectTransform> ();
					plank2Collider.offset = new Vector2 (0, plank2Collider.offset.y);
					plank2Collider.size = new Vector2 (plank2Rect.sizeDelta.x + 4, plank2Collider.size.y);
				} else {
					plank2 = movedModelCollizion.slot1Collider.gameObject;
					float plank2MaxHeight = height;
					List<Collider2D> plank2Collisions = plank2.GetComponent<planksControl> ().collisions;
					foreach (Collider2D collision in plank2Collisions) {
						if (plank2MaxHeight < 420.0f - collision.transform.localPosition.y) {
							plank2MaxHeight = 420.0f - collision.transform.localPosition.y;
						}
					}
					if (plank2MaxHeight >= height) {

						plank2.transform.GetComponent<RectTransform> ().sizeDelta = new Vector2 (plank1.transform.GetComponent<RectTransform> ().sizeDelta.x, Mathf.Min (plank2MaxHeight * 0.92f, 335.0f));
						plank2.transform.position = new Vector2 (plank2.transform.position.x, plank1.transform.position.y + 0.524f * movedModel.GetComponent<PolygonCollider2D> ().bounds.extents.x);
					}
				}

				if (!topplank2.active) {
					topplank2.SetActive (true);
				}
				calculatePlanks ("right");
			}
		}
	}

    public void AdjustHeight(float height) {
        ModelsCollizion movedModelCollizion = movedModel.GetComponent<ModelsCollizion>();
        if (!movedModelCollizion.onRightWall) {
            GameObject plank1 = movedModelCollizion.slot1Collider.gameObject;
            GameObject plank2 = movedModelCollizion.slot2Collider.gameObject;
            float plank1MaxHeight = height;
            float plank2MaxHeight = height;
            List<Collider2D> plank1Collisions = plank1.GetComponent<planksControl>().collisions;
            List<Collider2D> plank2Collisions = plank2.GetComponent<planksControl>().collisions;
            foreach (Collider2D collision in plank1Collisions) {
                if (plank1MaxHeight < 420.0f - collision.transform.localPosition.y) {
                    plank1MaxHeight = 420.0f - collision.transform.localPosition.y;
                }
            }
            foreach (Collider2D collision in plank2Collisions) {
                if (plank2MaxHeight < 420.0f - collision.transform.localPosition.y) {
                    plank2MaxHeight = 420.0f - collision.transform.localPosition.y;
                }
            }
            if (plank1MaxHeight >= height) {

                plank1.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(plank1.transform.GetComponent<RectTransform>().sizeDelta.x, Mathf.Min(plank1MaxHeight, 335.0f));
                
                plank1.transform.localPosition = new Vector2(plank1.transform.localPosition.x, movedModel.transform.localPosition.x * 0.123f + 335.0f);
            }
            if (plank2MaxHeight >= height) {
                plank2.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(plank1.transform.GetComponent<RectTransform>().sizeDelta.x, Mathf.Min(plank2MaxHeight, 335.0f));
                plank2.transform.position = new Vector2(plank2.transform.position.x, plank1.transform.position.y + 0.246f * movedModel.GetComponent<PolygonCollider2D>().bounds.extents.x);
            }
        }
        else {
            GameObject plank1 = movedModelCollizion.slot2Collider.gameObject;
            GameObject plank2 = movedModelCollizion.slot1Collider.gameObject;
            float plank1MaxHeight = height;
            float plank2MaxHeight = height;
            List<Collider2D> plank1Collisions = plank1.GetComponent<planksControl>().collisions;
            List<Collider2D> plank2Collisions = plank2.GetComponent<planksControl>().collisions;
            foreach (Collider2D collision in plank1Collisions) {
                if (plank1MaxHeight < 420.0f - collision.transform.localPosition.y) {
                    plank1MaxHeight = 420.0f - collision.transform.localPosition.y;
                }
            }
            foreach (Collider2D collision in plank2Collisions) {
                if (plank2MaxHeight < 420.0f - collision.transform.localPosition.y) {
                    plank2MaxHeight = 420.0f - collision.transform.localPosition.y;
                }
            }
            if (plank1MaxHeight >= height) {

                plank1.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(plank1.transform.GetComponent<RectTransform>().sizeDelta.x, Mathf.Min(plank1MaxHeight*0.92f, 335.0f));
                plank1.transform.localPosition = new Vector2(plank1.transform.localPosition.x, 217.904f - movedModel.transform.localPosition.x * 0.262f + 335.0f);
            }
            if (plank2MaxHeight >= height) {
                plank2.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(plank1.transform.GetComponent<RectTransform>().sizeDelta.x, Mathf.Min(plank2MaxHeight * 0.92f, 335.0f));
                plank2.transform.position = new Vector2(plank2.transform.position.x, plank1.transform.position.y + 0.524f * movedModel.GetComponent<PolygonCollider2D>().bounds.extents.x);
            }
        }
    }
		

}
