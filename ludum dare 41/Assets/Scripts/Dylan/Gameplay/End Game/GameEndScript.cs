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

    public void EndGame(bool disconnected)
    {
        GetComponent<PhotonView>().RPC("RPC_EndGame", PhotonTargets.All, disconnected);
    }

    [PunRPC]
    void RPC_EndGame(bool disconnected)
    {
        string winText = "";
        GetComponent<GameStatusController>().paused = true;
        endGameUI.SetActive(true);
        if (!disconnected)
        {
            int p1Pts, p2Pts;
            p1Pts = GetComponent<GameStatusController>().p1Points;
            p2Pts = GetComponent<GameStatusController>().p2Points;

            if (p1Pts > p2Pts)
                winText = GetComponent<GameStatusController>().p1NameTest;
            if (p1Pts < p2Pts)
                winText = GetComponent<GameStatusController>().p2NameTest;

            winText += " has won the game!";

            if (p1Pts == p2Pts)
                winText = "Draw!";

            
        }

        else
            winText = "Lost connection to other player";

        GameObject.Find("Win_Txt").GetComponent<Text>().text = winText;
        PhotonNetwork.LeaveRoom();
    }
}
