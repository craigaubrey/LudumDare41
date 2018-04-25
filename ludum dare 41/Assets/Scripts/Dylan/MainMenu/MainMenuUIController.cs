using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour {
    GameObject canvas, sceneDataObj;
    List<GameObject> UIParents = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
        //Starting setup for main menu
        SetupUI();
	}

    void SetupUI()
    {
        //Get canvas
        canvas = GameObject.Find("Canvas");
        sceneDataObj = GameObject.Find("SceneDataController_Obj");
        //Gather all UI elements
        foreach(Transform t in canvas.transform)
        {
            UIParents.Add(t.gameObject);
            //
            t.localScale = new Vector3(1, 1, 1);
            t.gameObject.SetActive(false);
        }

        if (sceneDataObj.GetComponent<UsernameScript>().GetUsername() == "")
            ChangeUI(1);
        else
            ChangeUI(0);
    }
	
    //Change ui currently displayed
	public void ChangeUI(int childToShow)
    {
        //Set the declared index active, and others inactive
        int counter = 0;
        foreach(GameObject g in UIParents)
        {
            if (counter == childToShow)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
            counter++;
        }
    }
}
