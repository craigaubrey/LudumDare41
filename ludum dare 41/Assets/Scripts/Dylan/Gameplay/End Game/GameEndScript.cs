using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScript : MonoBehaviour {

    private void Start()
    {
    }

    public void EndGame(bool disconnected)
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GetComponent<PhotonView>().RPC("RPC_EndGame", PhotonTargets.All, disconnected);
        else
            OfflineEndGame();
    }

    void OfflineEndGame()
    {
        string winText = "";
        GetComponent<GameStatusController>().paused = true;
        GameObject.Find("Controller").GetComponent<GameplayUIController>().ChangeUI(4);

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


        GameObject.Find("Win_Txt").GetComponent<Text>().text = winText;
    }


    [PunRPC]
    void RPC_EndGame(bool disconnected)
    {
        string winText = "";
        GetComponent<GameStatusController>().paused = true;
        GameObject.Find("Controller").GetComponent<GameplayUIController>().ChangeUI(4);
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
