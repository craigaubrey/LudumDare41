using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {
    Vector2 p1StartPos = new Vector2(-5.5f, 2);
    Vector2 p2StartPos = new Vector2(5.5f, 2);

    int pauseCounter = 0;

    GameObject pauseMenu, mainPauseUI, waitingPauseUI;

    private void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu_Parent");
        mainPauseUI = pauseMenu.transform.GetChild(0).gameObject;
        waitingPauseUI = pauseMenu.transform.GetChild(1).gameObject;

        pauseMenu.SetActive(false);
        mainPauseUI.SetActive(false);
        waitingPauseUI.SetActive(false);
    }

    private void Update()
    {
        CheckPause();
    }

    public void RestartPlay()
    {
        GetComponent<PhotonView>().RPC("RPC_RestartPlayerPos", PhotonTargets.All);
        GetComponent<PhotonView>().RPC("RPC_WaitForGameStart", PhotonTargets.All);
    }

    void CheckPause()
    {
        if (Input.GetButtonDown("Pause") && !GetComponent<GameStatusController>().paused && !GetComponent<GameStatusController>().restartPause)
        {
            GetComponent<PhotonView>().RPC("RPC_PauseGame", PhotonTargets.All);
            print("ok i pause");
        }

    }

    public void ReadyToUnpause()
    {
        mainPauseUI.SetActive(false);
        waitingPauseUI.SetActive(true);

        GetComponent<PhotonView>().RPC("RPC_UnpauseCounter", PhotonTargets.All);
    }

    [PunRPC]
    void RPC_RestartPlayerPos()
    {
        if (PhotonNetwork.isMasterClient)
            GetComponent<CharacterContainer>().myCharacter.transform.position = p1StartPos;
        else
            GetComponent<CharacterContainer>().myCharacter.transform.position = p2StartPos;
    }

    [PunRPC]
    void RPC_WaitForGameStart()
    {
        GetComponent<GameStatusController>().restartPause = true;

        GetComponent<GameStatusController>().StartGameInitializer();
    }

    [PunRPC]
    void RPC_PauseGame()
    {
        Time.timeScale = 0;
        GetComponent<GameStatusController>().paused = true;
        pauseCounter = 2;

        pauseMenu.SetActive(true);
        mainPauseUI.SetActive(true);
        waitingPauseUI.SetActive(false);

    }

    [PunRPC]
    void RPC_UnpauseCounter()
    {
        pauseCounter--;

        if(pauseCounter <= 0)
        {
            GetComponent<PhotonView>().RPC("RPC_ResumeGame", PhotonTargets.All);
        }
    }

    [PunRPC]
    void RPC_ResumeGame()
    {
        Time.timeScale = 1;
        GetComponent<GameStatusController>().paused = false;

        pauseMenu.SetActive(false);
    }
}
