using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMainController : MonoBehaviour {
    int offlinePlayer = 1;

    public void SetOfflinePlayer(int playerNum) { offlinePlayer = playerNum; }
    public int GetOfflinePlayer() { return offlinePlayer; }

	// Use this for initialization
	void start () {
           // GetComponent<PhotonView>().RPC("SetCharacterSpriteColour", PhotonTargets.All);
	}
	
    [PunRPC]
    void SetCharacterSpriteColour()
    {
        if (!GetComponent<PhotonView>().isMine)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
            print("crap");
        }
    }
}
