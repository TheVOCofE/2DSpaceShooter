using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAsteroids : MonoBehaviour {

    public float spawnDelay;
    public float spawnDist;
    public float asteroidSpeed;

    public GameObject asteroid1;
    public GameObject asteroid2;
    public GameObject asteroid3;

    bool allowSpawn = true;

    // Update is called once per frame
    void Update () {

        if (allowSpawn) {
            StartCoroutine(spawnAsteroid());
        }//end if

        
    }//end update

    IEnumerator spawnAsteroid() {
        allowSpawn = false;
        int ySpawn = Random.Range(-10, 11);
        int xSpawn = Random.Range(-10, 11);
        Vector3 spawnDir = new Vector3(xSpawn, ySpawn, 0.0f);
        spawnDir.Normalize();
        spawnDir *= spawnDist;
        int asteroidSize = Random.Range(0, 3);
        Quaternion randomRot = Quaternion.Euler(0.0f, 0.0f, Random.Range(0, 360));

        switch (asteroidSize)
        {
            case 1:
                var asteroidTemp1 = Instantiate(asteroid1, transform.position + spawnDir, randomRot);
                asteroidTemp1.GetComponent<Rigidbody2D>().velocity = asteroidSpeed * -spawnDir.normalized;
                Destroy(asteroidTemp1, 15.0f);
                break;
            case 2:
                var asteroidTemp2 = Instantiate(asteroid2, transform.position + spawnDir, randomRot);
                asteroidTemp2.GetComponent<Rigidbody2D>().velocity = asteroidSpeed * -spawnDir.normalized;
                Destroy(asteroidTemp2, 15.0f);
                break;
            default:
                var asteroidTemp3 = Instantiate(asteroid3, transform.position + spawnDir, randomRot);
                asteroidTemp3.GetComponent<Rigidbody2D>().velocity = asteroidSpeed * -spawnDir.normalized;
                Destroy(asteroidTemp3, 15.0f);
                break;
        }//end switch
        yield return new WaitForSeconds(spawnDelay);
        allowSpawn = true;
    }

    
}//end spawnAsteroids