using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{

    [SerializeField]
    float gameDuration;

    float gameTimer;

    // Use this for initialization
    void Start()
    {
        gameTimer = gameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<GameStatusController>().paused && !GetComponent<GameStatusController>().restartPause)
        {
            if(gameTimer > 0)
                gameTimer -= Time.deltaTime;
            GameObject.Find("Timer_Txt").GetComponent<Text>().text = gameTimer.ToString("0");

            if (gameTimer <= 0)
            {
                //End Game
                GetComponent<GameEndScript>().EndGame();
            }
        }
    }


    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(gameTimer);
        }

        else
        {
            gameTimer = (float)stream.ReceiveNext();
        }
    }
}
