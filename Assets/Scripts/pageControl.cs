using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pageControl : MonoBehaviour {

	public GameObject active;
	public GameObject chooseWall;
	public GameObject chooseSize, wall2;
	public GameObject interfaceLayout;
	public GameObject planer;
    public GameObject rackEditor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void openChooseWall(){
		active.SetActive (false);
		chooseWall.SetActive (true);
		active = chooseWall;
	}

	public void openChooseSize(int WallCount){
		active.SetActive (false);
		chooseSize.SetActive (true);
		active = chooseSize;
		if (WallCount == 1)
			wall2.SetActive (false);
	}

	public void SetActiveInterfaceLayout(bool active){
		interfaceLayout.SetActive (active);	
	}

	public void openPlaner(int walls) {
        bool twoWalls = false;
        if (active.GetComponent<chooseSize>().slider2.IsActive()) {
            twoWalls = true;
        }
        active.SetActive (false);
		planer.SetActive (true);
		active = planer;
        if (twoWalls) {
            planer.GetComponent<Planer>().twoWalls = true;
        }
	}
    public void openRackEditor(GameObject rack) {
        RackEditController controller = rackEditor.GetComponent<RackEditController>();
        controller.rackSource = rack;
        rackEditor.SetActive(true);
    }
}
