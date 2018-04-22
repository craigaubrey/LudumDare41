using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovementScript : Photon.MonoBehaviour {
    public float movementSpeed, jumpHeight, jumpSpeed;

    Vector3 selfPos;
    //RigidBody
    Rigidbody2D rb;

    private void Start()
    {
        //Get rigidbody
        rb = GetComponent<Rigidbody2D>();
        //testing
        GameObject.Find("Text").GetComponent<Text>().text = "Game: " + PhotonNetwork.room.name;
    }

    // Update is called once per frame
    void Update () {
        if (photonView.isMine)
        {
            //Left and right movement
            CheckMoveLeft();
            CheckMoveRight();
            CheckJump();
        }

	}

    //Moveing right
    void CheckMoveRight()
    {
        if (GetStickMovement() <= 1 && GetStickMovement() > 0.4f)
            transform.position = new Vector3(transform.position.x + GetSpeed(), transform.position.y, transform.position.z);
    }

    //Moving left
    void CheckMoveLeft()
    {
        if (GetStickMovement() >= -1 && GetStickMovement() < -0.4f)
            transform.position = new Vector3(transform.position.x - GetSpeed(), transform.position.y, transform.position.z);
    }

    //Jumping
    void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.AddForce(Vector2.up * jumpHeight);
        }
    }

    /// <summary>
    /// Network
    /// </summary>
    /// 
    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }

    void SmoothNetMovement()
    {
        transform.position = selfPos;
    }


    /// <summary>
    /// Getters
    /// </summary>
    bool Grounded()
    {
        if (rb.velocity.y > - 0.1f && rb.velocity.y < 0.1f)
            return true;
        else
            return false;
    }
    //Return the speed
    float GetSpeed()
    {
        if(Grounded())
            return movementSpeed * Time.deltaTime;
        else
            return jumpSpeed * Time.deltaTime;
    }
    //Return stick current input
    float GetStickMovement()
    {
        return Input.GetAxis("Horizontal");
    }


}
