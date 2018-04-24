using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadController : MonoBehaviour
{
    //Player character prefab
    GameObject mainPlayer, goalObject, ballObj;

    //devmodCOntroller
    DevModeController dmCon;

    //On awake
    private void Awake()
    {
        //Get prefabs
        mainPlayer = Resources.Load<GameObject>("MainPlayer");
        goalObject = Resources.Load<GameObject>("Goal_Obj");
        ballObj = Resources.Load<GameObject>("Ball_Obj");

        //Get other instances
        dmCon = GameObject.Find("Controller").GetComponent<DevModeController>();

        //add scene load
        InitGame();

    }

    //Spawn player on scene load
    void InitGame()
    {

        if (!dmCon.devMode)
        {
            SpawnPlayer();
            SpawnGoals();
            if (PhotonNetwork.isMasterClient)
                SpawnBall();
            GetComponent<GameStatusController>().StartGameInitializer();
            GetUsername();

        }
        else if (dmCon.devMode)
            SetUpDevGame();

        
    }

    //Add instance to network
    Vector2 p1StartPos = new Vector2(-5.5f, -4);
    Vector2 p2StartPos = new Vector2(5.5f, -4);

    Quaternion p1Rot = new Quaternion(0, 0, 0, 0);
    Quaternion p2Rot = new Quaternion(0, 180, 0, 0);
    void SpawnPlayer()
    {
        print("shite");
        if (PhotonNetwork.isMasterClient)
            GetComponent<CharacterContainer>().myCharacter = PhotonNetwork.Instantiate(mainPlayer.name, p1StartPos, p1Rot, 0);
        else
            GetComponent<CharacterContainer>().myCharacter = PhotonNetwork.Instantiate(mainPlayer.name, p2StartPos, p2Rot, 0);
    }


    Vector2 p1GoalPos = new Vector2(-8, -3);
    Vector2 p2GoalPos = new Vector2(8, -3);
    //add goals to network
    void SpawnGoals()
    {
        print("Spawn Goals");
        if (PhotonNetwork.isMasterClient)
        {
            print("Host goals");
            GameObject g = PhotonNetwork.Instantiate(goalObject.name, p1GoalPos, goalObject.transform.rotation, 0);
            g.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            print("Other goals");
            GameObject g = PhotonNetwork.Instantiate(goalObject.name, p2GoalPos, goalObject.transform.rotation, 0);
            g.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    void SpawnBall()
    {
        GameObject g = PhotonNetwork.Instantiate(ballObj.name, new Vector3(0,4.5f,0), ballObj.transform.rotation, 0);
        g.name = "Ball_Obj";
    }

    void GetUsername()
    {
        string name = GameObject.Find("UsernameHolder_Obj").GetComponent<UsernameScript>().GetUsername();
        if (PhotonNetwork.isMasterClient)
            GetComponent<GameStatusController>().p1NameTest = name;
        else
            GetComponent<GameStatusController>().p2NameTest = name;

        GetComponent<PhotonView>().RPC("RPC_GiveOtherUserName", PhotonTargets.OthersBuffered, name);
    }

    [PunRPC]
    void RPC_GiveOtherUserName(string name)
    {
        if (!PhotonNetwork.isMasterClient)
            GetComponent<GameStatusController>().p1NameTest = name;
        else
            GetComponent<GameStatusController>().p2NameTest = name;
    }

    void SetUpDevGame()
    {
        GameObject g = GameObject.Instantiate(mainPlayer, mainPlayer.transform.position, Quaternion.identity);
        g.GetComponent<CharacterMovementScript>().testChar = false;
        g = GameObject.Instantiate(mainPlayer, mainPlayer.transform.position, Quaternion.identity);
        g.GetComponent<CharacterMovementScript>().testChar = true;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        LoadMainMenu();
    }

    public void LoadMainMenu()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
    }
}
