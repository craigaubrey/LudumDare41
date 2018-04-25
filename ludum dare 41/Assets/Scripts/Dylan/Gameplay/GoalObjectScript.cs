using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalObjectScript : MonoBehaviour {

    bool canScore = true;

    //For offline Mode
    int offlinePlayer;

    public void SetOfflinePlayer(int playerNum) { offlinePlayer = playerNum; }
    public int GetOfflinePlayer() { return offlinePlayer; }

    private void Awake()
    {
        //Set color on other player client
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
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
            //For Online Play
            else if(!gameObject.GetComponent<PhotonView>().isMine && collision.gameObject.GetComponent<PhotonView>().isMine && canScore)
            {
                canScore = false;
                //For UI Updates and such
                GameObject.Find("Controller").GetComponent<GameStatusController>().GoalScored(0);
                //TO reset play
                GetComponent<PhotonView>().RPC("RPC_ApplyForceAfterGoal", PhotonTargets.All, transform.position.x);
                GetComponent<PhotonView>().RPC("RPC_PausePlayAndStartCoroutine", PhotonTargets.All);
            }
            else if (offlinePlayer != collision.gameObject.GetComponent<CharacterMainController>().GetOfflinePlayer() && canScore && GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == false)
            {
                canScore = false;
                int scorer = collision.gameObject.GetComponent<CharacterMainController>().GetOfflinePlayer();
                

                //For UI Updates and such
                GameObject.Find("Controller").GetComponent<GameStatusController>().GoalScored(scorer);
                //TO reset play
                RPC_ApplyForceAfterGoal(transform.position.x);
                RPC_PausePlayAndStartCoroutine();
            }
        }
    }

    [PunRPC]
    void RPC_ApplyForceAfterGoal(float xPosOfGoal)
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GameObject.Find("Controller").GetComponent<CharacterContainer>().myCharacter.GetComponent<CharacterMovementScript>().ApplyForceAfterGoal(xPosOfGoal);
        else
        {
            GameObject[] chars = GameObject.FindGameObjectsWithTag("character");
            foreach (GameObject g in chars)
                g.GetComponent<CharacterMovementScript>().ApplyForceAfterGoal(xPosOfGoal);
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
