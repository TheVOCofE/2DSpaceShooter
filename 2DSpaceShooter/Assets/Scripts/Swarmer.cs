using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AI script implementing flocking techniques
 * */

public class Swarmer : MonoBehaviour
{
    public Vector3 velocity;
    public Vector3 acceleration = Vector3.zero;
    static float desiredSeparion = 2.5f;
    static float neighborDistance = 7;
    float maxForce = 2;
    float maxSpeed = 5;
    public GameManager swarm;
    GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        float angle = Random.Range(0, Mathf.PI * 2);
        velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
        if (swarm == null)
        {
            swarm = (GameManager)FindObjectOfType(typeof(GameManager));
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sep = 0.79f * Separate(swarm.GetSwarmers());
        Vector3 ali = 1.125f * Align(swarm.GetSwarmers());
        Vector3 coh = 1.125f * Cohesion(swarm.GetSwarmers());

        acceleration = Vector3.zero;
        acceleration += sep * 1.5f;
        acceleration += ali;
        acceleration += coh;


        float dt = Time.deltaTime;
        dt *= 5;

        Vector3 pos = transform.position;
        pos += velocity * dt + 0.5f * acceleration * dt * dt;
        velocity += acceleration * dt;
        acceleration = Vector3.zero;

        if (pos.x > 48) pos.x = -48;
        if (pos.x < -48) pos.x = 48;
        if (pos.y > 32) pos.y = -32;
        if (pos.y < -32) pos.y = 32;
        transform.position = pos;


        float angle = Mathf.Acos(Vector3.Dot(velocity.normalized, Vector3.up));

        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle * (velocity.x > 0 ? -1 : 1));
    }

    Vector3 Separate(List<GameObject> boids)
    {
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < desiredSeparion))
            {
               
                Vector3 diff = transform.position - other.transform.position;
                diff.Normalize();
                diff /= d;
                steer += diff;
                count++;
            }
        }
        if (count > 0)
        {
            steer /= count;
        }

        if (steer.magnitude > 0)
        {
            steer.Normalize();
            steer *= maxSpeed;
            steer -= velocity;
            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }
        return steer;
    }

    Vector3 Align(List<GameObject> boids)
    {
       
        Vector3 sum = Vector3.zero;
        Vector3 steer = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighborDistance))
            {
                               
                sum += ((Swarmer)other.GetComponent(typeof(Swarmer))).velocity;
                count++;
            }
        }

        if (count > 0)
        {
            
            sum.Normalize();
            sum *= maxSpeed;
            steer = sum - velocity;
            if (steer.magnitude > maxForce)
            {
                steer.Normalize();
                steer *= maxForce;
            }
        }

        return steer;
    }

    Vector3 Cohesion(List<GameObject> boids)
    {
        
        Vector3 sum = Vector3.zero;
        int count = 0;

        foreach (GameObject other in boids)
        {
            if (other == gameObject) continue;

            float d = Vector3.Distance(transform.position, other.transform.position);
            if ((d > 0) && (d < neighborDistance))
            {
                    
                sum += ((Swarmer)other.GetComponent(typeof(Swarmer))).transform.position;
                count++;
            }
        }

        if (count > 0)
        {
            Vector3 averagePos = Vector3.zero;
            
            sum = sum / count;
            return Seek(sum);
        }

        return Vector3.zero;
    }



    Vector3 Seek(Vector3 target)
    {
        Vector3 desired = target - transform.position;
        desired.Normalize();
        desired /= maxSpeed;

        Vector3 steer = desired - velocity;
        if (steer.magnitude > maxForce)
        {
            steer.Normalize();
            steer *= maxForce;
        }

        return steer;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
            swarm.OnSwarmDestroy(gameObject);
        }
        else if (c.gameObject.tag == "Asteroid")
        {
            Destroy(c.gameObject);
        }
    }
}

