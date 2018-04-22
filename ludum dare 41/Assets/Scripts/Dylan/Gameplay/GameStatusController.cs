﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour {
    public bool restartPause = true, paused = false;
    GameObject startGameGraphic;
    //Score
    public int p1Points = 0, p2Points = 0;
    string p1NameTest = "John", p2NameTest = "Bob";

    private void Start()
    {
        startGameGraphic = GameObject.Find("StartRound_Img");
        startGameGraphic.SetActive(false);
    }

    private void Update()
    {
        GameObject.Find("Score_Txt").GetComponent<Text>().text = GetScoreText();
    }

    public void GoalScored()
    {
        if (PhotonNetwork.isMasterClient)
            p1Points++;
        else
            p2Points++;

        GetComponent<PhotonView>().RPC("RPC_GoalScored", PhotonTargets.All, p1Points, p2Points);
    }

    public void StartGameInitializer()
    {
        GetComponent<PhotonView>().RPC("RPC_StartGameCountdown", PhotonTargets.All);
    }

    IEnumerator StartGameTimer()
    {
        yield return new WaitForSeconds(2);
        startGameGraphic.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        startGameGraphic.SetActive(false);
        restartPause = false;
    }

    /// <summary>
    /// RPCS
    /// </summary>
    /// <returns></returns>
    [PunRPC]
    void RPC_GoalScored(int p1Pts, int p2Pts)
    {
        print("Getting score");
        p1Points = p1Pts;
        p2Points = p2Pts;
    }

    [PunRPC]
    void RPC_StartGameCountdown() {
        StartCoroutine(StartGameTimer());
    }

    ///
    ///Getters
    ///
    public string GetScoreText()
    {
        return p1NameTest + " " + p1Points + " - " + p2Points + " " + p2NameTest;
    }
}
