using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour {
    public float meleePower;
    public bool attacking;
    public float hitCooldownTimer, hitCooldown;

    [SerializeField]
    float hitHorizForce;
    [SerializeField]
    float hitVertForce;
    [SerializeField]
    float deadHorizForce;
    [SerializeField]
    float deadVertForce;

    void Start()
    {
        attacking = false;
    }

    private void Update()
    {
        if (hitCooldownTimer > 0)
            hitCooldownTimer -= Time.deltaTime;
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

    //Hitting another player
    void OnCollisionStay2D(Collision2D other)
    {
        if (attacking && other.gameObject != transform.parent.gameObject && other.gameObject.tag == "character" && CanBeHit())
        {
            hitCooldownTimer = hitCooldown;
            GameObject.FindWithTag("ball").GetComponent<BallScript>().HitDropBall();
            //Update HP & RPC
            other.gameObject.GetComponent<CharacterHPScript>().UpdateHealth(other.gameObject.GetComponent<CharacterHPScript>().HP-=1);
            int hp = other.gameObject.GetComponent<CharacterHPScript>().HP;

            //Still alive, but vulnerable to death
            if (hp == 1)
            {
                //Visual representation - Push back
                if (transform.parent.localRotation.y == 0)
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * hitVertForce);
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * hitHorizForce);
                }
                else
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * hitVertForce);
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * hitHorizForce);
                }
            }
            if (hp <= 0)
            {
                //Visual representation - Push back
                if (transform.parent.localRotation.y == 0)
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * deadVertForce);
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * deadHorizForce);
                }
                else
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * deadVertForce);
                    other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * deadHorizForce);
                }
            }
            
        }
    }

    bool CanBeHit()
    {
        if (hitCooldownTimer <= 0)
            return true;
        else
            return false;
    }
}
