using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovementScript : Photon.MonoBehaviour {
    public float movementSpeed, jumpHeight, jumpSpeed;
    public float attackCooldownTimer, attackCooldown;
    //testing tools
    public bool testChar;
    bool triggerDown = false;

    Vector3 selfPos;
    //RigidBody
    Rigidbody2D rb;
    DevModeController dmCon;
    //Melee Object
    MeleeScript melee;

    private void Start()
    {
        //Get rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Get instances
        dmCon = GameObject.Find("Controller").GetComponent<DevModeController>();
        melee = transform.GetChild(0).GetComponent<MeleeScript>();
    }

    // Update is called once per frame
    void Update () {
        if (photonView.isMine || dmCon.devMode && testChar)
        {
            //Left and right movement
            CheckMoveLeft();
            CheckMoveRight();
            CheckJump();
            CheckMelee();
            if (attackCooldownTimer > 0)
                attackCooldownTimer -= Time.deltaTime;
        }

	}

    //Moveing right
    void CheckMoveRight()
    {
        if (GetStickMovement() <= 1 && GetStickMovement() > 0.4f)
        {
            //Move right
            transform.position = new Vector3(transform.position.x + GetSpeed(), transform.position.y, transform.position.z);
            //Rotate to face    
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    //Moving left
    void CheckMoveLeft()
    {
        if (GetStickMovement() >= -1 && GetStickMovement() < -0.4f)
        {
            //Move left
            transform.position = new Vector3(transform.position.x - GetSpeed(), transform.position.y, transform.position.z);
            //Rotation
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
    }

    //Jumping
    void CheckJump()
    {
        if (Input.GetButtonDown("Jump") && Grounded())
        {
            rb.AddForce(Vector2.up * jumpHeight);
        }
    }

    //Melee attack input
    void CheckMelee()
    {
        //right triger down and check if player is able to attack
        if (Input.GetAxis("Right Trigger Melee Attack") < -0.4f && CanAttack())
        {
            print("attack");
            melee.DoAttack();
            //Trigger is down
            triggerDown = true;
            //Set cooldown
            attackCooldownTimer = attackCooldown;
        }

        //Recognize if the trigger is back up
        if (Input.GetAxis("Right Trigger Melee Attack") > -0.3f)
            triggerDown = false;
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
    //make sure the trigger isn't being held down & check cooldown timer (Set in inspector)
    bool CanAttack()
    {
        if (triggerDown)
            return false;
        if (attackCooldownTimer <= 0)
            return true;
        else
            return false;
    }

}
