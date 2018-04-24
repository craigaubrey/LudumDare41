using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UsernameScript : MonoBehaviour {

    string userName = "";
    GameObject userNameBox;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        userNameBox = GameObject.Find("NameInput_Panel");
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
            userNameBox.SetActive(false);
        }
    }

    public string GetUsername() { return userName; }
}
