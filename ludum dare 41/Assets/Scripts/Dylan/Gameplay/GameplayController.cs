using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayController : MonoBehaviour {
    Vector2 p1StartPos = new Vector2(-5.5f, -4);
    Vector2 p2StartPos = new Vector2(5.5f, -4);


    int pauseCounter = 0;


    private void Awake()
    {

    }

    private void Update()
    {
        CheckPause();
    }

    public void RestartPlay()
    {
        ResetHP();
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
        {
            GetComponent<PhotonView>().RPC("RPC_RestartPlayerPos", PhotonTargets.All);
            GetComponent<PhotonView>().RPC("RPC_WaitForGameStart", PhotonTargets.All);
        }
        else
        {
            RPC_RestartPlayerPos();
            RPC_WaitForGameStart();
        }
    
    }

    void ResetHP()
    {
        GameObject[] chars = GameObject.FindGameObjectsWithTag("character");
        foreach(GameObject g in chars)
            g.GetComponent<CharacterHPScript>().UpdateHealth(2);
    }

    void CheckPause()
    {
        if (Input.GetButtonDown("Pause") && !GetComponent<GameStatusController>().paused && !GetComponent<GameStatusController>().restartPause)
        {
            if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
                GetComponent<PhotonView>().RPC("RPC_PauseGame", PhotonTargets.All);
            else
                RPC_PauseGame();
            print("ok i pause");
        }

    }

    public void ReadyToUnpause()
    {
        GameObject.Find("Controller").GetComponent<GameplayUIController>().ChangeUI(3);

        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GetComponent<PhotonView>().RPC("RPC_UnpauseCounter", PhotonTargets.All);
        else
            RPC_ResumeGame();
    }

    [PunRPC]
    void RPC_RestartPlayerPos()
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
        {
            GetComponent<CharacterContainer>().myCharacter.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            if (PhotonNetwork.isMasterClient)
                GetComponent<CharacterContainer>().myCharacter.transform.position = p1StartPos;
            else
                GetComponent<CharacterContainer>().myCharacter.transform.position = p2StartPos;
        }
        else
        {
            GameObject[] chars = GameObject.FindGameObjectsWithTag("character");
            foreach (GameObject g in chars)
                if (g.GetComponent<CharacterMainController>().GetOfflinePlayer() == 1)
                    g.transform.position = p1StartPos;
                else
                    g.transform.position = p2StartPos;
        }
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

        GameObject.Find("Controller").GetComponent<GameplayUIController>().ChangeUI(2);

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

        GameObject.Find("Controller").GetComponent<GameplayUIController>().ChangeUI(0);
    }
}
