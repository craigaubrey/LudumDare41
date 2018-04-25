using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour {
    public int level;
    [SerializeField]
    public List<string> sceneList;

    //Controller
    GameObject controller, sceneDataObj;

    private void Start()
    {
        controller = GameObject.Find("Controller");
        sceneDataObj = GameObject.Find("SceneDataController_Obj");
    }

    // Use this for initialization
    public void FindOnlineMatch_Click () {
        PhotonNetwork.ConnectUsingSettings("v1.0");
	}
	
    //Join Lobby
	void OnConnectedToMaster()
    {
        //Randomize a name
        string name = "playaaa" + Random.Range(1, 1000000).ToString();
        PhotonNetwork.player.name = name;
        PhotonNetwork.JoinRandomRoom();
    }

    //Failed ot join room, set up room
    void OnPhotonRandomJoinFailed()
    {
        controller.GetComponent<MainMenuUIController>().ChangeUI(2);
        RoomOptions options = new RoomOptions() { isVisible = true, MaxPlayers = 2 };
        int randomName = Random.Range(0, 100);

        PhotonNetwork.CreateRoom(randomName.ToString(), options, TypedLobby.Default);
        level = Random.Range(1, sceneList.Count+1);
    }

    //Joined a room(called when created room too)
    void OnJoinedRoom()
    {
        print("joined " + PhotonNetwork.room.name);
    }

    //a player has joined this room, move to scene
    void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            GetComponent<PhotonView>().RPC("RPC_StartGame", PhotonTargets.All, level);
        }
    }

    //Cancel finding a game, return to menu
    public void CancelGameFind()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        controller.GetComponent<MainMenuUIController>().ChangeUI(0);
    }

    /// <summary>
    /// RPCS
    /// </summary>
    /// //Move all players to game scene
    [PunRPC]
    public void RPC_StartGame(int level)
    {
        sceneDataObj.GetComponent<InterSceneController>().SetOnlineStatus(true);
        PhotonNetwork.LoadLevel(level);
    }
}
