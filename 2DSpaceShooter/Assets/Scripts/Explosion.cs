using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public float expansionRate;
    public float lifeTime;

	// Use this for initialization
	void Start () {
        Destroy(this.gameObject, lifeTime);
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(expansionRate, expansionRate, 0.0f);
	}

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject.tag == "PlayerBullet" || c.gameObject.tag == "Asteroid"){
            Destroy(c.gameObject);
        }//end if
    }//end on trigger enter 2d
}