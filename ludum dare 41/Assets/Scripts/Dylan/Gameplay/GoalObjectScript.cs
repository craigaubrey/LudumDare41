using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectScript : MonoBehaviour {
    [SerializeField]
    float goalUpForce;
    [SerializeField]
    float goalHorizForce;

    bool canScore = true;

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
            transform.GetChild(0).GetComponent<ParticleSystem>().startColor = Color.red;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "character" && collision.gameObject == GameObject.FindWithTag("ball").GetComponent<BallScript>().carrier)
        {
            if (gameObject.GetComponent<PhotonView>().isMine && collision.gameObject.GetComponent<PhotonView>().isMine)
                print("Own goal collision");
            else if(!gameObject.GetComponent<PhotonView>().isMine && collision.gameObject.GetComponent<PhotonView>().isMine && canScore)
            {
                canScore = false;
                
                //For UI Updates and such
                GameObject.Find("Controller").GetComponent<GameStatusController>().GoalScored();
                //TO reset play
                GetComponent<PhotonView>().RPC("RPC_ApplyForceAfterGoal", PhotonTargets.All);
                GetComponent<PhotonView>().RPC("RPC_PausePlayAndStartCoroutine", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    void RPC_ApplyForceAfterGoal()
    {
        print("apply force");
        Rigidbody2D rb = GameObject.Find("Controller").GetComponent<CharacterContainer>().myCharacter.GetComponent<Rigidbody2D>();
        
        if(rb.gameObject.transform.position.x > transform.position.x)
        {
            rb.AddForce(Vector2.up * goalUpForce);
            rb.AddForce(Vector2.right * goalHorizForce);
        }
        else
        {
            rb.AddForce(Vector2.up * goalUpForce);
            rb.AddForce(Vector2.right * goalHorizForce);
        }

    }

    [PunRPC]
    void RPC_PausePlayAndStartCoroutine()
    {
        GameObject.Find("Controller").GetComponent<GameStatusController>().restartPause = true;
        StartCoroutine(WaitBeforeRestartProcess());
    }

    IEnumerator WaitBeforeRestartProcess()
    {
        yield return new WaitForSeconds(2f);

        GameObject.FindWithTag("ball").GetComponent<BallScript>().RestartPlay();
        GameObject.Find("Controller").GetComponent<GameplayController>().RestartPlay();
        canScore = true;
    }

}
