using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {
    public float meleePower;
    public bool attacking;

    void Start()
    {
        attacking = false;
    }

    public void DoAttack()
    {
        GetComponent<PhotonView>().RPC("RPC_DoAttack", PhotonTargets.Others);
    }

    [PunRPC]
    void RPC_DoAttack()
    {
        attacking = true;
        StartCoroutine(AttackingCoroutine());
        GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator AttackingCoroutine()
    {
        yield return new WaitForSeconds(0.1f);

        attacking = false;
        GetComponent<Collider2D>().enabled = false;
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (attacking && other.gameObject != transform.parent.gameObject)
        {
            GameObject.FindWithTag("ball").GetComponent<BallScript>().HitDropBall();
            print("Attacked SUccessfully");
            if(transform.parent.localRotation.y == 0)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 60);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 100);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 60);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 100);
            }
        }
    }


}
