using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using PGL;

public class importScript : MonoBehaviour
{
    // This stores all the class stuff like all the word lookup tables and such
    // Reverse the string and index ID references - so we can search by string and get the closest id match
    public IDictionary<int, string> LookupTable { get; private set; } = new Dictionary<int, string>();
    public IDictionary<int, List<int>> edges { get; private set; } = new Dictionary<int, List<int>>();
    public GraphStructure ref_graph = new GraphStructure();

    public TextAsset edges_file;
    // public TextAsset nodes_file;

    private Point DefaultPoint = new Point(0f, 0f, 0f);

    void Awake()
    {

        Graph_import inputgraph = JsonUtility.FromJson<Graph_import>(edges_file.text);

        int unhadled_edges = 0;
        foreach (Vertex vertex in inputgraph.vertices)
        {
            try {
                Node node = new Node(vertex.name, vertex.id, vertex.link, DefaultPoint);
                ref_graph.AddNode(node);
                edges.Add(vertex.id, vertex.link.ToList());
                LookupTable.Add(vertex.id, vertex.name);
            }
            catch {
                unhadled_edges++;
            }
        }
    }
    

    /// <summary>
    /// These are all the helper classes needed to import the data into the the matrices
    /// </summary>
    [System.Serializable]
    public class Graph_import
    {
        public Vertex[] vertices;
    }

    [System.Serializable]
    public class Vertex
    {
        public string name;
        public int id;
        public int[] link;
    }
}
