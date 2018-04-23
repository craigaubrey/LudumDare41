using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMainController : MonoBehaviour {


	// Use this for initialization
	void Awake () {
        GetComponent<PhotonView>().RPC("SetCharacterSpriteColour", PhotonTargets.All);
        print("jobby");
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
