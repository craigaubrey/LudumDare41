using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStatusController : MonoBehaviour {
    public bool restartPause = true, paused = false;
    public float restartTimer;
    GameObject startGameGraphic;
    //Score
    public int p1Points = 0, p2Points = 0;
    public string p1NameTest = "John", p2NameTest = "Bob";

    private void Start()
    {
        startGameGraphic = GameObject.Find("StartRound_Img");
        startGameGraphic.SetActive(false);
    }

    private void Update()
    {
        GameObject.Find("Score_Txt").GetComponent<Text>().text = GetScoreText();

        StartRound();
    }

    public void GoalScored()
    {
        if (PhotonNetwork.isMasterClient)
            p1Points++;
        else
            p2Points++;

        GetComponent<PhotonView>().RPC("RPC_GoalScored", PhotonTargets.All, p1Points, p2Points);
    }

    bool canStartCoroutine = true;
    void StartRound()
    {
        if(restartPause && restartTimer > 0)
        {
            startGameGraphic.SetActive(true);
            restartTimer -= Time.deltaTime;
            GameObject.Find("StartRound_Txt").GetComponent<Text>().text = restartTimer.ToString("0.0");
            canStartCoroutine = true;
        }
        if(restartPause && restartTimer <= 0 && canStartCoroutine)
        {
            canStartCoroutine = false;
            StartCoroutine(StartGameTimer());
        }
    }

    public void StartGameInitializer()
    {
        GetComponent<PhotonView>().RPC("RPC_StartGameCountdown", PhotonTargets.All);
    }

    IEnumerator StartGameTimer()
    {
        GameObject.Find("StartRound_Txt").GetComponent<Text>().text = "START";
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
        restartTimer = 4;
    }

    ///
    ///Getters
    ///
    public string GetScoreText()
    {
        return p1NameTest + " " + p1Points + " - " + p2Points + " " + p2NameTest;
    }
}
