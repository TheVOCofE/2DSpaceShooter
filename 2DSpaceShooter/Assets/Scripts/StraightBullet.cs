using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "Asteroid")
        {
            Destroy(this.gameObject);
            Destroy(c.gameObject);

        }
    }//end on trigger enter 2d
}
