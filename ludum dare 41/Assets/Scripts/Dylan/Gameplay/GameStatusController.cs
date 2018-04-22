using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour {

    //Score
    int p1Points = 0, p2Points = 0;
    string p1NameTest = "John", p2NameTest = "Bob";


    private void Start()
    {
        GameObject.Find("Score_Txt").GetComponent<Text>().text = GetScoreText();
    }

    public string GetScoreText()
    {
        return p1NameTest + " " + p1Points + " - " + p2Points + " " + p2NameTest;
    }
}
