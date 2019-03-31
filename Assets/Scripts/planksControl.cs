using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class planksControl : MonoBehaviour {
    
    public bool isMoving = false;
    public List<Collider2D> collisions;
    public List<Collider2D> movingCollisions;
    public bool isColliding = false;
    public bool onRightWall = false;
	public GameObject plank_min, plank_med, plank_max;
	GameObject spawnedPlank;
	bool isCreated = false;
	int spawningPlankHeight = 1;
	int thisPlankHeight = 1;
	//public int collidingWithPolkas = 0;
	public int collidingWithVeryBigPolkas = 0;


	void OnTriggerEnter2D(Collider2D col) {
		if (!col.gameObject.name.Contains("model_stellazh") && !col.gameObject.name.Contains("opplank")) {
            if (!isMoving) {
                if (col.gameObject.name.Contains("model")) {
                    collisions.Add(col);
					if (col.gameObject.tag == "VeryBigPolka") {
						collidingWithVeryBigPolkas += 1;
					}
					//if (col.gameObject.tag == "Polka") {
					//	collidingWithPolkas += 1;
					//}
                }
            }
            else {
                movingCollisions.Add(col);
                if (movingCollisions.Count > 1) {
                    isColliding = true;
                }
            }
        }
    }

	void OnTriggerExit2D(Collider2D col) {
        if (!col.gameObject.name.Contains("model_stellazh")) {
            if (collisions.Contains(col)) {
                collisions.Remove(col);
				if (col.gameObject.tag == "VeryBigPolka") {
					collidingWithVeryBigPolkas -= 1;
				}
				//if (col.gameObject.tag == "Polka") {
				//	collidingWithPolkas -= 1;
				//}
                if (collisions.Count == 0) {
                    Destroy(this.gameObject);
                }
            }
            else if (movingCollisions.Contains(col)) {
                movingCollisions.Remove(col);
                if (movingCollisions.Count == 0) {
                    isColliding = false;
                }
            }
        }
	}

	void Update() {
		if (gameObject.GetComponent<RectTransform> ().sizeDelta.y <= 154f) {
			thisPlankHeight = 1;
			if (spawnedPlank == null) {
				spawnedPlank = Instantiate (plank_min,gameObject.transform);
				spawnedPlank.transform.localPosition = new Vector2 (0f, 0f);
				spawnedPlank.GetComponent<RectTransform>().sizeDelta = new Vector3 (0f,0f,0f);
				spawnedPlank.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
			}
		} else if (gameObject.GetComponent<RectTransform>().sizeDelta.y > 154f && gameObject.GetComponent<RectTransform>().sizeDelta.y <= 250f) {
			thisPlankHeight = 2;
			if (spawnedPlank == null) {
				spawnedPlank = Instantiate (plank_med,gameObject.transform);
				spawnedPlank.transform.localPosition = new Vector2 (0f, 0f);
				spawnedPlank.GetComponent<RectTransform>().sizeDelta = new Vector3 (0f,0f,0f);
				spawnedPlank.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
			}
		} else if (gameObject.GetComponent<RectTransform>().sizeDelta.y > 250f) {
			thisPlankHeight = 3;
			if (spawnedPlank == null) {
				spawnedPlank = Instantiate (plank_max,gameObject.transform);
				spawnedPlank.transform.localPosition = new Vector2 (0f, 0f);
				spawnedPlank.GetComponent<RectTransform>().sizeDelta = new Vector3 (0f,0f,0f);
				spawnedPlank.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
			}
		}
		if (!isCreated) {
			isCreated = true;
			spawningPlankHeight = thisPlankHeight;
		}

		if (thisPlankHeight != spawningPlankHeight) {
			Destroy (spawnedPlank);
			spawnedPlank = null;
			spawningPlankHeight = thisPlankHeight;
		}
	}

}
