using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Spawns chasing bullets
 * */
public class Turret : MonoBehaviour {

   GameObject target;
    public GameObject bullet;
    Vector3 orientation = Vector3.up;
    GameManager manager;


    bool allowFire = true;

    // Use this for initialization
    void Start ()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        GameObject ManagerObj = GameObject.Find("GameManager");
        manager = ManagerObj.GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 dir = Vector3.zero;
        dir = target.transform.position - transform.position;
        dir = dir / dir.magnitude;
        Vector3 angle = new Vector3(0, 0, -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
        transform.eulerAngles = angle;

        if (Mathf.Abs(target.transform.position.magnitude - transform.position.magnitude) < 15 && allowFire)
        {
           StartCoroutine(Fire());
        }
	}

    IEnumerator Fire()
    {
        allowFire = false;
        var bulletTemp = Instantiate(bullet, transform.position, transform.rotation);
        Destroy(bulletTemp, 2.0f);
        yield return new WaitForSeconds(3.0f);
        allowFire = true;

    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
            GameObject ManagerObj = GameObject.Find("GameManager");
            manager = ManagerObj.GetComponent<GameManager>();
            manager.OnEnemyDestroy(10);
        }
        else if (c.gameObject.tag == "Asteroid")
        {
            Destroy(c.gameObject);
        }
    }
}
