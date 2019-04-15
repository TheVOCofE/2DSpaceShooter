using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Camera follows player 
 * Stops at edges of background sprite
 * Values need to be adjusted with the camera area/the size of the game field 
 * */
public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    float minPositionx = -30.0f; 
    float maxPositionx = 30.0f; 
    float minPositiony = -17.0f; 
    float maxPositiony = 17.0f;   

    void Update()
    {
        Vector3 pos = player.transform.position;
        pos.z += transform.position.z;
        pos.x = Mathf.Clamp(pos.x,minPositionx,maxPositionx);
        pos.y = Mathf.Clamp(pos.y, minPositiony, maxPositiony);
        transform.position = pos;
        
       
    }
}
