using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMovementScript : Photon.MonoBehaviour {
    public float movementSpeed, jumpHeight, jumpSpeed, runSpeed, carrySpeed;
    public float attackCooldownTimer, attackCooldown;
    public bool canDoubleJump = true;

    [SerializeField]
    float goalUpForce;
    [SerializeField]
    float goalHorizForce;


    Animator animator;

    bool rightMove, leftMove;
    
    //testing tools
    public float testAxis;
    bool triggerDown = false;

    Vector3 selfPos;
    //RigidBody
    Rigidbody2D rb;
    DevModeController dmCon;
    //Melee Object
    MeleeScript melee;


    private void Start()
    {
        movementSpeed = runSpeed;
        //Get rigidbody
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Get instances
        dmCon = GameObject.Find("Controller").GetComponent<DevModeController>();
        melee = transform.GetChild(0).GetComponent<MeleeScript>();

        leftMove = false;
        rightMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        testAxis = GetStickMovement();
        //player check
        if (photonView.isMine || GameObject.Find("SceneDataController_Obj").GetComponent<InterSceneController>().GetOnlineStatus() == false)
        {
            //Pause check
            if (!dmCon.gameObject.GetComponent<GameStatusController>().restartPause && !dmCon.gameObject.GetComponent<GameStatusController>().paused)
            {
                //Left and right movement
                CheckMoveLeft();
                CheckMoveRight();
                CheckJump();
                CheckMelee();
                CheckDoubleJump();
                CheckMoveAnimation();
                if (attackCooldownTimer > 0)
                    attackCooldownTimer -= Time.deltaTime;
            }
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
            //set moving for naimation
            rightMove = true;
        }
        else
            rightMove = false;
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
            //set moving for animation
            leftMove = true;
        }
        else
            leftMove = false;
    }

    //Jumping
    void CheckJump()
    {
        if (Input.GetButtonDown("Jump" + GetComponent<CharacterMainController>().GetOfflinePlayer()))
        {
            if(Grounded())
                rb.AddForce(Vector2.up * jumpHeight);
            else if (canDoubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.1f);
                rb.AddForce(Vector2.up * jumpHeight);
                canDoubleJump = false;
            }
        }
    }

    //Melee attack input
    void CheckMelee()
    {
        //right triger down and check if player is able to attack
        if (Input.GetAxis("Right Trigger Melee Attack" + GetComponent<CharacterMainController>().GetOfflinePlayer()) < -0.4f && CanAttack())
        {
            print("attack");
            if (transform.rotation.y == 0)
                rb.AddForce(Vector2.right * 600);
            else
                rb.AddForce(Vector2.left * 600);

            melee.DoAttack();
            //Trigger is down
            triggerDown = true;
            //Set cooldown
            attackCooldownTimer = attackCooldown;
        }

        //Recognize if the trigger is back up
        if (Input.GetAxis("Right Trigger Melee Attack" + GetComponent<CharacterMainController>().GetOfflinePlayer()) > -0.3f)
            triggerDown = false;
    }

    void CheckDoubleJump()
    {
        if (!canDoubleJump)
        {
            if (rb.velocity.y == 0)
                canDoubleJump = true;
        }
    }

    void CheckMoveAnimation()
    {
        if(!leftMove && !rightMove)
        {
            animator.SetBool("Walking", false);
        }
        else
            animator.SetBool("Walking", true);
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

    public void ApplyForceAfterGoal(float goalXPos)
    {
        print("apply force");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        GameObject.Find("Controller").GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Goal"));
        print(goalXPos);

        if (transform.position.x > goalXPos)
        {
            rb.AddForce(Vector2.up * goalUpForce);
            rb.AddForce(Vector2.right * goalHorizForce);
        }
        else
        {
            rb.AddForce(Vector2.up * goalUpForce);
            rb.AddForce(Vector2.left * goalHorizForce);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform" && transform.position.y > collision.gameObject.transform.position.y)
            gameObject.transform.SetParent(collision.gameObject.transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MovingPlatform")
            gameObject.transform.SetParent(null);
    }

    /// <summary>
    /// Getters
    /// </summary>
    bool Grounded()
    {
        if (rb.velocity.y == 0)
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
        return Input.GetAxis("HorizontalStick" + GetComponent<CharacterMainController>().GetOfflinePlayer());
    }
    float GetVertStick()
    {
        return Input.GetAxis("VertStick");
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
