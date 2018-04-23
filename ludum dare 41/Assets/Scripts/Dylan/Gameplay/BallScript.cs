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
            transform.position = carrier.transform.position;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }

        
    }

    public void RestartPlay()
    {
        GetComponent<PhotonView>().RPC("RPC_DropBall", PhotonTargets.All);
        GetComponent<PhotonView>().RPC("RPC_ResetBall", PhotonTargets.All);
    }

    public void HitDropBall()
    {
        GetComponent<PhotonView>().RPC("RPC_DropBall", PhotonTargets.All);
    }

    ///
    ///RPCS
    ///
    [PunRPC]
    void RPC_ResetBall()
    {
        transform.position = new Vector2(0, 4.5f);
    }

    [PunRPC]
    void RPC_DropBall()
    {
        bool dropped = false;
        if (carrier != null)
            dropped = true;
        carrier = null;
        gameObject.transform.parent = null;
        
        StartCoroutine(WaitToResetBallComps());

        //REset move speed
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
