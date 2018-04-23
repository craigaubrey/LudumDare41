using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerHandler : MonoBehaviour {
    public int level;
    [SerializeField]
    public List<string> sceneList;

	// Use this for initialization
	public void FindOnlineMatch () { 
        PhotonNetwork.ConnectUsingSettings("v1.0");
	}
	
    //Join Lobby
	void OnConnectedToMaster()
    {
        print("Joined Lobby");
        //Randomize a name
        string name = "playaaa" + Random.Range(1, 1000000).ToString();
        PhotonNetwork.player.name = name;
        PhotonNetwork.JoinRandomRoom();
    }

    //Failed ot join room, set up room
    void OnPhotonRandomJoinFailed()
    {
        print("Failed to join random room");

        RoomOptions options = new RoomOptions() { isVisible = true, MaxPlayers = 2 };
        int randomName = Random.Range(0, 100);

        PhotonNetwork.CreateRoom(randomName.ToString(), options, TypedLobby.Default);
        level = Random.Range(1, sceneList.Count+1);
    }

    //Joined a room
    void OnJoinedRoom()
    {
        print("joined " + PhotonNetwork.room.name);
        GameObject.Find("Text").GetComponent<Text>().text = "In game " + PhotonNetwork.room.name;
    }

    //a player has joined this room, move to scene
    void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        if (PhotonNetwork.isMasterClient)
        {
            GetComponent<PhotonView>().RPC("RPC_StartGame", PhotonTargets.All, level);
        }
    }

    /// <summary>
    /// RPCS
    /// </summary>
    /// //Move all players to game scene
    [PunRPC]
    public void RPC_StartGame(int level)
    {
        PhotonNetwork.LoadLevel(level);
    }
}
