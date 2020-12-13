using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PGL
{
    public class GraphStructure
    {
        public IDictionary<int, Node> nodes;
        public GraphStructure(){
           this.nodes = new Dictionary<int, Node>();
        }

        public void AddNode(Node node) {
            this.nodes.Add(node.id, node);
        }

        public void ResetHasBeenExplored(){
            foreach (Node node in nodes.Values){
                node.hasBeenExplored = false;
            }
        }

        public void ResetHasBeenDrawn(){
            foreach (Node node in nodes.Values)
            {
                node.hasBeenDrawn = false;
            }
        }

        public void ResetFallsOnPath()
        {
            foreach (Node node in nodes.Values)
            {
                node.fallsOnPath = false;
            }
        }

        public void ResetAll() {
            ResetHasBeenExplored();
            ResetHasBeenDrawn();
            ResetFallsOnPath();
        }

        public List<int> BFS(GraphStructure graph, Node start_n, Node end_n)
        {
            // Create a new list and put all the nodes that led upto it
            List<int> pathIDs = new List<int>();

            // Reset all the nodes so that if there were any active nodes
            graph.ResetHasBeenExplored();

            // Execute the BFS algorithm
            // Create a FIFO queue to take all the elements
            Queue<Node> SearchQueue = new Queue<Node>();
            // Queue the first point
            SearchQueue.Enqueue(start_n);

            // the main while loop
            while (SearchQueue.Count > 0){
                // Search the queue 
                // get the first node
                Node searchcenter = SearchQueue.Dequeue();
                searchcenter.hasBeenExplored = true;

                // add all the connected node Id's to the code
                foreach (int Connected_Ids in searchcenter.connected_nodes){
                    // if not explored add it to the queue
                    if (graph.nodes[Connected_Ids].hasBeenExplored == false) {
                        graph.nodes[Connected_Ids].exploredFrom = searchcenter;
                        SearchQueue.Enqueue(graph.nodes[Connected_Ids]);
                    }
                }

                // if the current search node index is equal to the node then break out of it
                if (searchcenter == end_n){
                    break;
                }
            }

            // Lastly bactrace all the paths that we found
            // back trace the path
            pathIDs.Add(end_n.id);

            // so long as the first node isnt queued keep going
            while (pathIDs[pathIDs.Count - 1] != start_n.id)
            {
                // Get the last item and append that guy's parent to the path node
                Node node = graph.nodes[pathIDs[pathIDs.Count - 1]];
                pathIDs.Add(node.exploredFrom.id);
            }

            // Add the start node back in the list
            pathIDs.Add(start_n.id);

            // reverse the path list
            pathIDs.Reverse();

            // return the path list that we found
            return pathIDs;
        }


        public List<int> DFS(GraphStructure graph, Node start_n, Node end_n) {

            // Create a new list and put all the nodes that led upto it
            List<int> pathIDs = new List<int>();

            // Reset all the nodes so that if there were any active nodes
            graph.ResetHasBeenExplored();

            // Execute the DFS algorithm
            // Create a LIFO stack to take all the elements
            Stack<int> SearchQueue = new Stack<int>();
            // Queue the first point
            SearchQueue.Push(start_n.id);

            // while the stack is not empty
            while (SearchQueue.Count > 0) {
                // pop the first thing
                int SearchCenterID = SearchQueue.Pop();

                // if not visited
                if (graph.nodes[SearchCenterID].hasBeenExplored == false) {

                    // set that this has been explored
                    graph.nodes[SearchCenterID].hasBeenExplored = true;

                    // push all its neighbours 
                    foreach (int ConnectedID in graph.nodes[SearchCenterID].connected_nodes){
                        // set where it was explored from
                        graph.nodes[ConnectedID].exploredFrom = graph.nodes[SearchCenterID];
                        // Push the neighbour
                        SearchQueue.Push(ConnectedID);
                    }
                }

                // check if the things is the end point
                if (SearchCenterID == end_n.id)
                {
                    // if yes then break
                    break;
                }
            }


            pathIDs.Add(end_n.id);

            // so long as the first node isnt queued keep going
            while (pathIDs[pathIDs.Count - 1] != start_n.id)
            {
                // Get the last item and append that guy's parent to the path node
                Node node = graph.nodes[pathIDs[pathIDs.Count - 1]];
                pathIDs.Add(node.exploredFrom.id);
            }

            // Add the start node back in the list
            pathIDs.Add(start_n.id);

            // reverse the path list
            pathIDs.Reverse();

            // return the path list that we found
            return pathIDs;
        }
    }

    public class Node
    {
        public string name;
        public int id;
        public int[] connected_nodes;
        public bool hasBeenExplored;
        public bool hasBeenDrawn;
        public Point positionPoint;
        public Node exploredFrom;
        public bool fallsOnPath = false;


        public Node(string name, int id, int[] edges, Point positionPoint)
        {
            this.name = name;
            this.id = id;
            this.connected_nodes = edges;
            this.hasBeenExplored = false;
            this.hasBeenDrawn = false;
            this.positionPoint = positionPoint;
            this.exploredFrom = null;
        }
    }

    public class Point
    {
        public float x;
        public float y;
        public float z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 ToVector()
        {
            Vector3 ref_Vector = new Vector3(this.x, this.y, this.z);
            return ref_Vector;
        }

        public Point(Vector3 positionVector)
        {
            this.x = positionVector.x;
            this.y = positionVector.y;
            this.z = positionVector.z;
        }
    }
}


