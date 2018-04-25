using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayUIController : MonoBehaviour {
    GameObject canvas;
    List<GameObject> UIParents = new List<GameObject>();

    void Awake()
    {
        //Starting setup for main menu
        SetupUI();
    }


    void SetupUI()
    {
        //Get canvas
        canvas = GameObject.Find("SecondaryCanvas");
        
        //Gather all UI elements
        foreach (Transform t in canvas.transform)
        {
            UIParents.Add(t.gameObject);
            //
            t.localScale = new Vector3(1, 1, 1);
            t.gameObject.SetActive(false);
        }
    }

    public void ChangeUI(int childToDisplay)
    {
        //Set the declared index active, and others inactive
        int counter = 0;
        foreach (GameObject g in UIParents)
        {
            if (counter == childToDisplay)
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
