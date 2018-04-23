using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScript : MonoBehaviour {
    GameObject endGameUI;

    private void Start()
    {
        endGameUI = GameObject.Find("WinScreen_Parent");
        endGameUI.SetActive(false);
    }

    public void EndGame()
    {
        GetComponent<PhotonView>().RPC("RPC_EndGame", PhotonTargets.All);
    }

    [PunRPC]
    void RPC_EndGame()
    {
        GetComponent<GameStatusController>().paused = true;
        endGameUI.SetActive(true);
        int p1Pts, p2Pts;
        string winText = "";
        p1Pts = GetComponent<GameStatusController>().p1Points;
        p2Pts = GetComponent<GameStatusController>().p2Points;

        if (p1Pts > p2Pts)
            winText = GetComponent<GameStatusController>().p1NameTest;
        if(p1Pts < p2Pts)
            winText = GetComponent<GameStatusController>().p2NameTest;

        winText += " has won the game!";

        if (p1Pts == p2Pts)
            winText = "Draw!";

        GameObject.Find("Win_Txt").GetComponent<Text>().text = winText;
    }
}
