using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

/*
 * Decision making AI
 * Handles game state, score, resetting game, etc
 * */

public class GameManager : MonoBehaviour {

    int score = 0;
    int enemyCounter;
    int init_spawn_count = 4;
    //Enemies
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    float spawnDelay = 15.0f;
    float timeSincePlayerDamage = 0;
    bool allowSpawn = true;
    bool isBossSpawned = false;
    GameObject player;
    public Text scoreField;
   
    //Flocking enemy, kind of unique
    public GameObject swarmer;
    List<GameObject> swarmers = new List<GameObject>();

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyCounter = init_spawn_count;

        int test = 0;
        Debug.Log("Test: " + (int)(test + 0.5f)/(test + 0.5f));

        for (int i = 0; i < init_spawn_count; i++)
        {
            int ySpawn = Random.Range(-16, 16);
            int xSpawn = Random.Range(-29, 29);
            //Need to update for extra enemies
            int spawnEnemyNum = Random.Range(0,5);
            
            if (Mathf.Abs(ySpawn - player.transform.position.y) < 5)
            {
                ySpawn -= 10 * (int)((ySpawn+0.5f) / Mathf.Abs(ySpawn+0.5f));
            }
            if (Mathf.Abs(xSpawn - player.transform.position.x) < 5)
            {
                xSpawn -= 10 * (int)((xSpawn + 0.5f) / Mathf.Abs(xSpawn + 0.5f));
            }


            if (spawnEnemyNum == 1)
            {
                GameObject enemy = Instantiate(enemy1);
                enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
            }
            else if (spawnEnemyNum == 2)
            {
                GameObject enemy = Instantiate(enemy2);
                enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
            }
            else if (spawnEnemyNum == 3)
            {
                GameObject enemy = Instantiate(enemy3);
                enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    swarmers.Add(Instantiate(swarmer, new Vector3(xSpawn, ySpawn, 0), transform.rotation));
                    swarmer.SetActive(true);
                    swarmer.GetComponent<Swarmer>().swarm = this;
                }
                enemyCounter += 2;
            }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (!isBossSpawned)
        {
            float enemyCounterTimeModifier = Mathf.Clamp(enemyCounter, 0, 20) / 4.0f;
            float playerDamageTimeModifier = -Mathf.Clamp(timeSincePlayerDamage, 0, 60) / 12.0f;
            float scoreTimeModifier = -Mathf.Clamp(score, 0, 500) / 125.0f;
            spawnDelay = 4.0f + enemyCounterTimeModifier + scoreTimeModifier;
            spawnDelay = Mathf.Clamp(spawnDelay, 1.0f, 15.0f);
            spawnDelay += playerDamageTimeModifier;
            spawnDelay = Mathf.Clamp(spawnDelay, 1.0f, 15.0f);
            playerDamageTimeModifier += Time.deltaTime;
            if (allowSpawn)
            {
                StartCoroutine(spawnTimer());
            }
        }
	}

    public void OnEnemyDestroy(int points)
    {
        score += points;
        enemyCounter--;
        scoreField.text = "Score: " + score;
    }

    public void OnSwarmDestroy(GameObject deadSwarmer)
    {
        score += 5;
        enemyCounter--;
        scoreField.text = "Score: " + score;
        swarmers.Remove(deadSwarmer);
    }

   public void playerTookDamage()
    {
        timeSincePlayerDamage = 0;
        if (player.GetComponent<ShipController>().getHealth() <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator spawnTimer()
    {
        allowSpawn = false;
        Debug.Log(spawnDelay);
        int ySpawn = Random.Range(-16, 16);
        int xSpawn = Random.Range(-29, 29);

        if (Mathf.Abs(ySpawn - player.transform.position.y) < 5)
        {
            ySpawn -= 10 * (int)((ySpawn + 0.5f) / Mathf.Abs(ySpawn + 0.5f));
        }
        if (Mathf.Abs(xSpawn - player.transform.position.x) < 5)
        {
            xSpawn -= 10 * (int)((xSpawn + 0.5f) / Mathf.Abs(xSpawn + 0.5f));
        }
        //Need to update for extra enemies
        int spawnEnemyNum = Random.Range(0, 5);
        enemyCounter++;
        if (spawnEnemyNum == 1)
            {
                GameObject enemy = Instantiate(enemy1);
                enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
            }
            else if (spawnEnemyNum == 2)
            {
                GameObject enemy = Instantiate(enemy2);
                enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
            }
        else if (spawnEnemyNum == 3)
        {
            GameObject enemy = Instantiate(enemy3);
            enemy.transform.position = new Vector3(xSpawn, ySpawn, 0);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                swarmers.Add(Instantiate(swarmer, Vector3.zero, transform.rotation));
                swarmer.SetActive(true);
                swarmer.GetComponent<Swarmer>().swarm = this;
            }
            enemyCounter += 2;
            }
        yield return new WaitForSeconds(spawnDelay);
        allowSpawn = true;
    }

    public List<GameObject> GetSwarmers()
    {
        return swarmers;
    }
}
