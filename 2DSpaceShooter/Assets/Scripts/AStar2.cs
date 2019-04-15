using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AStar2
    {
        public Tilemap map;

        List<Node> visited = new List<Node>();
        List<Node> unvisited = new List<Node>();

        Dictionary<Node, Node> predecessorDict = new Dictionary<Node, Node>();
        Dictionary<Node, float> distanceDict = new Dictionary<Node, float>();
        Dictionary<Node, float> actualDistanceDict = new Dictionary<Node, float>();

        public void Init(Tilemap tileMap)
        {
            map = tileMap;
        Debug.Log("map " + map.name);
            List<Node> nodes = map.GetAllNodes();
        Debug.Log("NODES " + map.GetAllNodes().Count);
            foreach (Node node in nodes)
            {
                distanceDict[node] = float.MaxValue;
                actualDistanceDict[node] = float.MaxValue;//
            }
        }

        public List<Node> Search(Node start, Node goal)
        {
            // 1. dist[s] = 0
            // 2. set all other distances to infinity
            List<Node> keys = new List<Node>(distanceDict.Keys);
            foreach (var key in keys)
            {
                distanceDict[key] = float.MaxValue;
                actualDistanceDict[key] = float.MaxValue;//
            }
            distanceDict[start] = 0;
            actualDistanceDict[start] = 0;//

            // 3. Initialize S(visited) and Q(unvisited)
            //    S, the set of visited nodes is initially empty
            //    Q, the queue initially conatains all nodes
            // To do: Initialize visited and unvisited
            visited.Clear();
            unvisited.Clear();
            foreach (Node n in map.GetAllNodes())
            {
                unvisited.Add(n);
            //Debug.Log("unvisited added" + n.x + n.y);
            }


            predecessorDict.Clear(); // to generate the result path

            while (unvisited.Count > 0)
            {
                // 4. select element of Q with the minimum distance
                // To do: Get a closest node from the unvisited list
                // Node u = ?
                Node u = GetClosestFromUnvisited();

                // Check if the node u is the goal.            
                if (u == goal) break;

                // 5. add u to list of S(visited)            
                // To do: add u to the visited list
                visited.Add(u);
                //unvisited.Remove(u);

                foreach (Node v in map.GetNeighbors(u))
                {
                    if (visited.Contains(v))
                        continue;

                    // 6. If new shortest path found then set new value of shortest path                
                    // To do: update distanceDict[v] 
                    // if dist[v] > actual dist[u] + w(u,v) + h(v,G) then 
                    //      dist[v] = actual dist[u] + w(u, v) + h(v, G)
                    if (distanceDict[v] > actualDistanceDict[u] + map.GetNeighborDistance(u, v) + map.GetEstimatedDistance(v, goal))
                    {
                        distanceDict[v] = actualDistanceDict[u] + map.GetNeighborDistance(u, v) + map.GetEstimatedDistance(v, goal);
                    }

                    // Update the actual distance only when it is smaller than the previous value
                    if (actualDistanceDict[v] > actualDistanceDict[u] + map.GetNeighborDistance(u, v))
                    {
                        actualDistanceDict[v] = actualDistanceDict[u] + map.GetNeighborDistance(u, v);

                        // update predecessorDict to build the result path
                        predecessorDict[v] = u;
                    }
                }
            }

            List<Node> path = new List<Node>();

            // Generate the shortest path
            path.Add(goal);
            Node p = predecessorDict[goal];

            while (p != start)
            {
                path.Add(p);
                p = predecessorDict[p];
            }

            path.Reverse();
            return path;
        }

        Node GetClosestFromUnvisited()
        {
            float shortest = float.MaxValue;
            Node shortestNode = null;
           // Debug.Log("dd:"+distanceDict.Count);
            foreach (var node in unvisited)
            {
                if (shortest > distanceDict[node])
                {
                    shortest = distanceDict[node];
                    shortestNode = node;
                }
            }

            unvisited.Remove(shortestNode);
            return shortestNode;
        }
    }
