using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Player class
 * */

public class ShipController : MonoBehaviour {

     public float verticalInputAcceleration = 1;
     public float Rotation = 5.0f;
     public float velocityDrag = 0.5f;
     private Vector3 velocity;
     private float RotationVelocity;

    bool allowFire = true;
    public GameObject bullet;

    int health = 10;
    bool allowDamage = true;
    GameManager manager;
    public Text healthField;

    void Start()
    {
        GameObject ManagerObj = GameObject.Find("GameManager");
        manager = ManagerObj.GetComponent<GameManager>();
    }

    private void Update()
    {

         
         Vector3 acceleration = Input.GetAxis("Vertical") * verticalInputAcceleration * transform.up;
         velocity += acceleration * Time.deltaTime;

        //Stops from moving outside game field
        //Values need to be changed if field is changed
        if (Mathf.Abs(transform.position.x) > 48)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x,-48,48);
            transform.position = pos;
        }
        if (Mathf.Abs(transform.position.y) > 32)
        {
            Vector3 pos = transform.position;
            pos.y = Mathf.Clamp(pos.y, -32, 32);
            transform.position = pos;
        }

        
        RotationVelocity = -1 * Input.GetAxis("Horizontal") * Rotation;

        
        if (Input.GetKey(KeyCode.Space) && allowFire)
        {
            StartCoroutine(Fire());
        }

    }
 
     private void FixedUpdate()
     {
         
         velocity = velocity * (1 - Time.deltaTime * velocityDrag);
        velocity = Vector3.ClampMagnitude(velocity, 15.0f);
        if (Mathf.Abs(transform.position.x) > 34)
        {
            transform.position.Set(34,transform.position.y,0);
        }

        
        GetComponent<Rigidbody2D>().velocity = velocity;
         transform.Rotate(0, 0, RotationVelocity);
     }

    IEnumerator Fire()
    {
        allowFire = false;
        var bulletTemp = Instantiate(bullet, transform.position + transform.up, transform.rotation);
        bulletTemp.GetComponent<Rigidbody2D>().velocity = 25.0f * transform.up;
        Destroy(bulletTemp, 1.25f);
        yield return new WaitForSeconds(0.10f);
        allowFire = true;

    }

    IEnumerator TakeDamage()
    {
        allowDamage = false;
        GetComponent<SpriteRenderer>().color = new Color(0.9f,0.1f,0.1f);

        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255);
        allowDamage = true;
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if ((c.gameObject.tag == "Enemy" || c.gameObject.tag == "Asteroid") || c.gameObject.tag == "Explosion" && allowDamage)
        {
            health--;
            healthField.text = "Health: " + health.ToString();
            manager.playerTookDamage();
            StartCoroutine(TakeDamage());
        }
    }

    public int getHealth()
    {
        return health;
    }
    
}
