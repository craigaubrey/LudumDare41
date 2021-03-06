﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBallController : MonoBehaviour {
    BallScript ballScript;


    public void BallPickup()
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GetComponent<PhotonView>().RPC("RPC_BallPickup", PhotonTargets.All);
        else
            OfflineBallPickup();
        GetComponent<CharacterMovementScript>().movementSpeed = GetComponent<CharacterMovementScript>().carrySpeed;
    }

    [PunRPC]
    void RPC_BallPickup()
    {
        ballScript = GameObject.FindWithTag("ball").GetComponent<BallScript>();
        ballScript.carrier = gameObject;
        ballScript.gameObject.transform.SetParent(transform);

        ballScript.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        ballScript.gameObject.GetComponent<CircleCollider2D>().enabled = false;

        
        GameObject.Find("Controller").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Ball Pickup"));
    }

    void OfflineBallPickup()
    {
        ballScript = GameObject.FindWithTag("ball").GetComponent<BallScript>();
        ballScript.carrier = gameObject;
        ballScript.gameObject.transform.SetParent(transform);

        ballScript.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        ballScript.gameObject.GetComponent<CircleCollider2D>().enabled = false;


        GameObject.Find("Controller").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Ball Pickup"));
    }
}
