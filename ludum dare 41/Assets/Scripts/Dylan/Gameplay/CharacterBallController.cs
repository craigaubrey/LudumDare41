using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBallController : MonoBehaviour {
    BallScript ballScript;


    public void BallPickup()
    {
        
        GetComponent<PhotonView>().RPC("RPC_BallPickup", PhotonTargets.All);
    }

    [PunRPC]
    void RPC_BallPickup()
    {
        ballScript = GameObject.FindWithTag("ball").GetComponent<BallScript>();
        ballScript.carrier = gameObject;
        ballScript.gameObject.transform.SetParent(transform);

        ballScript.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        ballScript.gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }
}
