//The purpose of this script is to move the enemy along the path by following the waypoints that have been placed on the scene
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [HideInInspector]
    public GameObject[] waypoints;
    private int currentWaypoint = 0;
    private float lastWaypointSwitchTime;
    public float speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        lastWaypointSwitchTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 startPosition = waypoints [currentWaypoint].transform.position; //the current position of the enemy
        Vector3 endPosition = waypoints [currentWaypoint + 1].transform.position; // the target position of the enemy

        float pathLength = Vector3.Distance (startPosition, endPosition); //calculate the distance between the two points
        float totalTimeForPath = pathLength / speed; //calculate the time taken to travel between the points
        float currentTimeOnPath = Time.time - lastWaypointSwitchTime;
        gameObject.transform.position = Vector2.Lerp (startPosition, endPosition, currentTimeOnPath / totalTimeForPath);

        if (gameObject.transform.position.Equals(endPosition))  //When the enemy reaches it's target
        {
            if (currentWaypoint < waypoints.Length - 2) //if the enemy has not reached the final waypoint 
            {
                currentWaypoint++; //increase the current position
                lastWaypointSwitchTime = Time.time; //
                RotateIntoMoveDirection();//rotate the enemy to the correct direction 
            }
            else
            { //if the enemy has reached it's final point
                Destroy(gameObject);  //destroy the enemy
                AudioSource audioSource = gameObject.GetComponent<AudioSource>();
                AudioSource.PlayClipAtPoint(audioSource.clip, transform.position); //play the sound
                GameManagerBehaviour gameManager = GameObject.Find("GameManager").GetComponent<GameManagerBehaviour>();
                gameManager.Health -= 1; //decrease the player's health by 1.
            }
        }
    }
    public float DistanceToGoal() ////calculates the distance to the final goal so the monsters can target the closest enemy
    {
        float distance = Vector2.Distance(gameObject.transform.position, waypoints[currentWaypoint + 1].transform.position);
        for (int i = currentWaypoint + 1; i < waypoints.Length - 1; i++)
        {
            Vector3 startPosition = waypoints[i].transform.position;
            Vector3 endPosition = waypoints[i + 1].transform.position;
            distance += Vector2.Distance(startPosition, endPosition);
        }
        return distance;
    }


    private void RotateIntoMoveDirection() //rotates the enemy so it will always face the direction of it's movement.
    {
        Vector3 newStartPosition = waypoints [currentWaypoint].transform.position;
        Vector3 newEndPosition = waypoints [currentWaypoint + 1].transform.position;
        Vector3 newDirection = (newEndPosition - newStartPosition);

        float x = newDirection.x;
        float y = newDirection.y;
        float rotationAngle = Mathf.Atan2 (y, x) * 180 / Mathf.PI;

        GameObject sprite = gameObject.transform.Find("Sprite").gameObject;
        sprite.transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
    }
}
