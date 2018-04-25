using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {
    public GameObject carrier;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(carrier == null && collision.gameObject.tag == "character")
        {
            collision.gameObject.GetComponent<CharacterBallController>().BallPickup();
        }
    }

    private void Update()
    {
        if (carrier != null)
        {
            transform.position = new Vector2(carrier.transform.position.x +0.05f, carrier.transform.position.y -0.04f);
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }

        
    }

    public void RestartPlay()
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
        {
            GetComponent<PhotonView>().RPC("RPC_DropBall", PhotonTargets.All);
            GetComponent<PhotonView>().RPC("RPC_ResetBall", PhotonTargets.All);
        }
        else
        {
            RPC_DropBall();
            RPC_ResetBall();
        }
        
    }

    public void HitDropBall()
    {
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GetComponent<PhotonView>().RPC("RPC_DropBall", PhotonTargets.All);
        else
            OfflineDropBall();
    }

    void OfflineDropBall()
    {
        bool dropped = false;
        if (carrier != null)
        {
            dropped = true;
            carrier.GetComponent<CharacterMovementScript>().movementSpeed = carrier.GetComponent<CharacterMovementScript>().runSpeed;
        }
            carrier = null;
        gameObject.transform.parent = null;

        StartCoroutine(WaitToResetBallComps());

     
        if (dropped)
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100);
    }

    ///
    ///RPCS
    ///
    [PunRPC]
    void RPC_ResetBall()
    {
        transform.position = new Vector2(0, 4.5f);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    [PunRPC]
    void RPC_DropBall()
    {
        bool dropped = false;
        if (carrier != null)
        {
            dropped = true;
            carrier.GetComponent<CharacterMovementScript>().movementSpeed = carrier.GetComponent<CharacterMovementScript>().runSpeed;
        }
        carrier = null;
        gameObject.transform.parent = null;
        
        StartCoroutine(WaitToResetBallComps());

        //REset move speed
        if (GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == true)
            GameObject.Find("Controller").GetComponent<CharacterContainer>().myCharacter.GetComponent<CharacterMovementScript>().movementSpeed = GameObject.Find("Controller").GetComponent<CharacterContainer>().myCharacter.GetComponent<CharacterMovementScript>().runSpeed;

        if (dropped)
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * 100);
    }

    IEnumerator WaitToResetBallComps()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
}
