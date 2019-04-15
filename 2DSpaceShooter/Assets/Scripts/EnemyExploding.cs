using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * AI script utilizing collision detection algorithm to avoid player bullets
 * */

public class EnemyExploding : MonoBehaviour
{

    GameObject target;
    GameObject player;
    public GameObject explosion;

    public GameManager manager;
    public GameObject avoidDestination;
    public float speed;
    float baseSpeed;
    public float maxAngularSpeed;
    Vector3 orientation = Vector3.up;
    RaycastHit2D hit;
    public Vector3 spawnPoint;
    public Quaternion spawnOrientation;
    bool launched;
    bool whileHitR;
    bool whileHitL;

    // Use this for initialization
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        player = target;
        baseSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        Vector3 dir = Vector3.zero;
        Vector3 rayDirF = transform.up;
        Vector3 rayDirR = transform.up + 0.25f * transform.right;
        Vector3 rayDirL = transform.up + 0.25f * -transform.right;
        rayDirR.Normalize();
        rayDirL.Normalize();
        if (hit = Physics2D.Raycast(transform.position, rayDirF, 15.00f))
        {
            if (hit.transform.tag == "Asteroid")
            {
                //Debug.Log(hit.transform.tag);
                dir = hit.point + 20.0f * hit.normal - (Vector2)transform.position;
                avoidDestination.transform.position = dir;
                StartCoroutine(returnOnTarget());
            }
            else
            {
                whileHitR = false;
                whileHitL = false;
                dir = target.transform.position - transform.position;
            }
        }
        else if ((hit = Physics2D.Raycast(transform.position, rayDirR, 7.5f)))
        {
            if (hit.transform.tag == "Asteroid")
            {
                dir = hit.point + 20.0f * hit.normal - (Vector2)transform.position;
                avoidDestination.transform.position = dir;
                StartCoroutine(returnOnTarget());
            }
            else
            {
                whileHitR = false;
                whileHitL = false;
                dir = target.transform.position - transform.position;
            }
        }
        else if ((hit = Physics2D.Raycast(transform.position, rayDirL, 7.5f)))
        {
            if (hit.transform.tag == "Asteroid")
            {
                dir = hit.point + 20.0f * hit.normal - (Vector2)transform.position;
                avoidDestination.transform.position = dir;
                StartCoroutine(returnOnTarget());
            }
            else
            {
                whileHitR = false;
                whileHitL = false;
                dir = target.transform.position - transform.position;
            }
        }
        else //Add dir = target here
        {
            whileHitR = false;
            whileHitL = false;
            dir = target.transform.position - transform.position;
        }


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

    IEnumerator returnOnTarget()
    {
        target = avoidDestination;
        speed = 2.0f * baseSpeed;
        yield return new WaitForSeconds(1.0f);
        speed = baseSpeed;
        target = player;

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerBullet" || c.gameObject.tag == "Asteroid" || c.gameObject.tag == "Explosion")
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
            GameObject ManagerObj = GameObject.Find("GameManager");
            manager = ManagerObj.GetComponent<GameManager>();
            manager.OnEnemyDestroy(10);
        }
        
    }


}
