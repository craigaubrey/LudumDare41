using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformMoveScript : MonoBehaviour {

    float moveSpeed = 4;
    [SerializeField]
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
        Scene s = SceneManager.GetActiveScene();
        if(s.name == "DylanTest")
            MovePlatform();
        if (s.name == "Mechlevel")
            MovePlatformMech();
    }

    void MovePlatform()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypoint], moveSpeed * Time.deltaTime);

        if(transform.position.x == wayPoints[currentWaypoint].x && transform.position.y == wayPoints[currentWaypoint].y)
        {
            if (currentWaypoint == 1)
                currentWaypoint = 2;
            else if (currentWaypoint == 2)
                currentWaypoint = 1;
        }
    }

    void MovePlatformMech()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[currentWaypoint], moveSpeed * Time.deltaTime);

            if (currentWaypoint == 1 && transform.position.y >= wayPoints[1].y)
                currentWaypoint = 2;
            else if (currentWaypoint == 2 && transform.position.y <= wayPoints[2].y)
                currentWaypoint = 1;
    }



}
