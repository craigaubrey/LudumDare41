using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectScript : MonoBehaviour {


    private void Awake()
    {
        //Set color on other player client
        GetComponent<PhotonView>().RPC("RPC_SetGoalRed", PhotonTargets.All);
    }

    //Setting up opposition goal as red
    [PunRPC]
    void RPC_SetGoalRed()
    {
        if(!GetComponent<PhotonView>().isMine)
            GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "character" && collision.gameObject == GameObject.FindWithTag("ball").GetComponent<BallScript>().carrier)
        {
            if (gameObject.GetComponent<PhotonView>().isMine && collision.gameObject.GetComponent<PhotonView>().isMine)
                print("Own goal collision");
            else
            {
                print("Goal");
            }
        }
    }

}
