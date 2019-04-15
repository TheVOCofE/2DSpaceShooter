using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AI script utilizing chasing algorithms
 * */

public class HomingBullet : MonoBehaviour {

   GameObject target;
    public float speed;
    public float maxAngularSpeed;

    Vector3 orientation = Vector3.up;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        Vector3 dir = Vector3.zero;
        dir = target.transform.position - transform.position;

        float dot = Vector3.Dot(dir, transform.up);
        float cos = dot / (dir.magnitude * transform.up.magnitude);

        float angle = Mathf.Acos(cos);
        float maxAngle = maxAngularSpeed * dt * Mathf.Deg2Rad;

        if (angle > maxAngle)
        {
            Vector3 normalOrientation = new Vector3(-transform.up.y, transform.up.x, 0);
            if (Vector3.Dot(dir, normalOrientation) >= 0)
            {
                dir.x = transform.up.x * Mathf.Cos(maxAngle) - transform.up.y * Mathf.Sin(maxAngle);
                dir.y = transform.up.x * Mathf.Sin(maxAngle) + transform.up.y * Mathf.Cos(maxAngle);
            }
            else
            {
                dir.x = transform.up.x * Mathf.Cos(-maxAngle) - transform.up.y * Mathf.Sin(-maxAngle);
                dir.y = transform.up.x * Mathf.Sin(-maxAngle) + transform.up.y * Mathf.Cos(-maxAngle);
            }
        }


        dir.Normalize();

        orientation = dir;
        UpdateOrientation();

        Vector3 pos = transform.position;
        Vector3 velocity = orientation * speed;

        pos = pos + velocity * Time.deltaTime;

        transform.position = pos;
    }

    void UpdateOrientation()
    {
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(orientation.x, orientation.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;
    }
}
