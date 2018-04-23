using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMoveScript : MonoBehaviour {

    float moveSpeed = 4;
    int currentWaypoint;

    [SerializeField]
    Vector2[] wayPoints;

    private void Start()
    {
        transform.position = wayPoints[0];
        currentWaypoint = 1;
    }

    private void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        print("moving");
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypoint], moveSpeed * Time.deltaTime);

        if(transform.position.x == wayPoints[currentWaypoint].x)
        {
            if (currentWaypoint == 1)
                currentWaypoint = 2;
            else if (currentWaypoint == 2)
                currentWaypoint = 1;
        }
    }




}
