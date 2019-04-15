using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PathEnemy : MonoBehaviour
    {
        public Node targetNode;
        public Tilemap map;
        AStar2 aStarPathfinder = new AStar2();
        public enum State { MOVE, IDLE };
        State state;
        public Transform player;
        bool first =true;

        float maxForce = 2;
    public Rigidbody2D rb;

    // Use this for initialization
    void Awake()
        {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        map = GameObject.FindGameObjectWithTag("Map").GetComponent<Tilemap>();
    }

        // Update is called once per frame
        void Update()
        {
        if(Time.time > 1.0f && first)
        {
        state = State.IDLE;
        Debug.Log("INPLAYERMAPCOUNT: " + map.GetAllNodes().Count);
        aStarPathfinder.Init(map);
        player = GameObject.FindGameObjectWithTag("Player").transform;
            first = false; 
        }else if (!first)
        {
// handle mouse input

        state = State.MOVE;
                Vector3 pos = player.position;//Input.mousePosition;
                                              //pos = Camera.main.ScreenToWorldPoint(pos);
        float dist = 1000000.0f;
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Node"))
        {
           
            if(dist > Vector3.Distance(player.position, g.transform.position))
            {
                dist = Vector3.Distance(player.position, g.transform.position);
                targetNode = map.GetNode(Mathf.FloorToInt(g.transform.position.x), Mathf.FloorToInt(g.transform.position.y));
                //Debug.Log("Movingtotarget");
            }
        }
                //targetNode = map.GetNode(Mathf.FloorToInt(pos.x + 0.5f), Mathf.FloorToInt(pos.y + 0.5f));
                //Debug.Log("target Node " + targetNode.x + " " + targetNode.y);

                if (targetNode.tileType == Node.TileType.WALL)
                {
                    StartCoroutine(SearchPathAndMove(targetNode, map));
           
        }
                else
                {
                    state = State.IDLE;
                    Debug.Log("WALL - can't move to here");
                }

            
        }
      
        
        }

        IEnumerator SearchPathAndMove(Node target, Tilemap map)
        {
            // To do: Find out a start node. The start node is the node where the character stands
            // Node start = ?
            Debug.Log(map.GetAllNodes().Count);
        Transform tran = gameObject.transform;
            Node start = map.GetNode(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        //Debug.Log("start: " + start.x + " " + start.y);
        // To do: Get the shortest path between start and target using astar algorithm
        // List<Node> path = ?
        Debug.Log("start: " + start.x + " target: " + target.x);
            List<Node> path = aStarPathfinder.Search(start, target);


            foreach (Node n in path)
            {
                yield return new WaitForSeconds(0.0f);
            // To do: move the character using the position info in the shortest path
            // you need to use "yield return new WaitForSeconds(0.5f);" to make a delay between each movement
            // Refer to https://docs.unity3d.com/Manual/Coroutines.html
            float angle = Mathf.Acos(Vector3.Dot(rb.velocity.normalized, Vector3.up));

            transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * angle * (rb.velocity.x > 0 ? -1 : 1));
            rb.velocity = (new Vector2((targetNode.x - transform.position.x), (targetNode.y-transform.position.y)).normalized * maxForce);
            }

            // set state to IDLE in order to enable next movement
            state = State.IDLE;
        }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player" || c.gameObject.tag == "PlayerBullet")
        {
            Destroy(this.gameObject);
            
        }
    }
}
