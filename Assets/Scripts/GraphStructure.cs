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
            // for some reason returing a list of Ids didnt work ... my guess is that cause the reference 
            // to the classes were broken as we were passing them around - so we got left with empty nodes
            // Too lazy to rewrite the class method 
            List<Node> path = new List<Node>();

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

            // back trace the path
            path.Add(end_n);

            // Node previous = path[path.Count - 1];

            // && path[path.Count - 1] != null
            // (previous.id != start_n.id) && 
            // the argument is that the first node cannot be explored from anything - hence is the start node
            while (path[path.Count - 1].exploredFrom != null)
            {
                // Get the last item and append that guy's parent to the path node
                Node node = path[path.Count - 1];
                path.Add(node.exploredFrom);
                pathIDs.Add(node.id);
                // previous = node;
            }

            // Add the start node back in the list
            pathIDs.Add(start_n.id);

            // reverse the path list
            pathIDs.Reverse();

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


