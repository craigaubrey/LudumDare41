using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameScript : MonoBehaviour {

    string userName = "";
    GameObject userNameBox;    
    //Controller
    GameObject controller;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        userNameBox = GameObject.Find("NameInput_Panel");
        controller = GameObject.Find("Controller");
        if (userName == null)
        {
            userNameBox.SetActive(false);
        }
    }

    public void SetUserName()
    {
        if (GameObject.Find("name_Text").GetComponent<Text>().text != "")
        {
            userName = GameObject.Find("name_Text").GetComponent<Text>().text;
            controller.GetComponent<MainMenuUIController>().ChangeUI(0);
        }
    }

    public string GetUsername() { return userName; }
}
