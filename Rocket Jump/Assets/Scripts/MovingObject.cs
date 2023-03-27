using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour {
    public Transform[] destinations;
    public Transform movingObject;

    [Header("Settings")]
    public bool moveAround;
    public float speed;

    public bool randomDestination;
    public bool includeInterval;
    public int interval;

    private int currentDestinationID;
    private bool moveBack;
    private float time = 0; //Used with delay

    private void Update() {
        if(time <= 0) {
            movingObject.localPosition = Vector2.MoveTowards(movingObject.localPosition, destinations[currentDestinationID].localPosition, speed * Time.deltaTime);
        }

        time -= Time.deltaTime;

        if(ObjectIsNearDestination()) {
            float.TryParse(destinations[currentDestinationID].name, out time);

            if(randomDestination) {
                if(includeInterval) {
                    if(interval > currentDestinationID) {
                        currentDestinationID = Random.Range(interval, destinations.Length);
                    } else {
                        currentDestinationID = Random.Range(0, interval);
                    }

                } else {
                    currentDestinationID = Random.Range(0, destinations.Length);
                }
            } else {
                if(!moveBack) {
                    currentDestinationID++;
                } else {
                    currentDestinationID--;
                }

                if(currentDestinationID >= destinations.Length) {
                    if(moveAround) {
                        currentDestinationID = 0;
                    } else {
                        currentDestinationID--;
                        moveBack = true;
                    }
                }
                if(currentDestinationID < 0) {
                    moveBack = false;
                    currentDestinationID = 0;
                }
            }
        }
    }

    public void LoadTrap(float posX, float posY, int targetID, bool moveBack) {
        movingObject.transform.localPosition = new Vector2(posX, posY);

        currentDestinationID = targetID;
        this.moveBack = moveBack;
    }

    public void SaveTrap(out float posX, out float posY, out int targetID, out bool moveBack) {
        posX = movingObject.transform.localPosition.x;
        posY = movingObject.transform.localPosition.y;

        targetID = currentDestinationID;
        moveBack = this.moveBack;
    }

    //Return true if the object is near the destination
    private bool ObjectIsNearDestination() {
        if(Vector2.Distance(movingObject.localPosition, destinations[currentDestinationID].localPosition) < 0.05f) {
            return true;
        }
        return false;
    }

    //Draw gizmos with object's path
    private void OnDrawGizmos() {
        if(!randomDestination) {
            for(int i = 0; i < destinations.Length - 1; i++) {
                Gizmos.DrawLine(destinations[i].position, destinations[i + 1].position);
            }
            if(moveAround) {
                Gizmos.DrawLine(destinations[destinations.Length - 1].position, destinations[0].position);
            }
        }
    }
}