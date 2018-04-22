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
            transform.position = carrier.transform.position;
    }
}
