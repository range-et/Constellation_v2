using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using PGL;
using System;


public class InteractionManager : MonoBehaviour
{
    // How far apart are all the points (The epsilon value)
    [Range(1.0f, 10.0f)]
    public float epsilon;

    // The classic system random as the start point 
    private System.Random random = new System.Random();

    // what is the name of the current node
    public string currentNodeText;

    // Prefab for the node component that has to be drawn
    [SerializeField] GameObject Node_Prefab;

    // Path mode boolean
    private bool pathMode = false;

    //All the relevant imports that we will be using 
    private GraphStructure graphStructure;

    //The colors for on path and off path
    public Color defaultColour;
    public Color pathColour;

    // References to all the butoons that we are using
    public GameObject RebaseText;
    public GameObject SearchText;
    public GameObject SuggestionOBJ_1;
    public GameObject SuggestionOBJ_2;

    TextMeshProUGUI suggestionList_1;
    TextMeshProUGUI suggestionList_2;

    // The camera Object reference
    public GameObject cameraPointer;

    // Start is called before the first frame update
    void Start()
    {
        // Create the graph structure which refers to the nodes that we have
        var import_script_asset = FindObjectOfType<importScript>();
        graphStructure = import_script_asset.ref_graph;

        // Create a reference to the suggestion list
        suggestionList_1 = SuggestionOBJ_1.GetComponent<TextMeshProUGUI>();
        suggestionList_2 = SuggestionOBJ_2.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // When we rebase clicking and searching 

        // Cast a ray from the camera through the mouse point to the object
        // When we hit the object - return that 
        // Current base is that object
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f)){
                RebaseCameraToTransform(hit.transform);
            }
        }
    }

    // This guys is the actuall method linked to the UI button
    public void Search(){
        string searchstring = RebaseText.GetComponent<TMP_InputField>().text;
        if (searchstring != null || searchstring != " "){
            graphStructure.ResetAll();
            // Rebase the camera the main node
            RebaseCameraToTransform(transform);
            // Search node - resets all the things 
            SearchNode(searchstring);
            // Lastly delete all the line connecting everything 
            var lineRenderer = transform.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
        else {
            suggestionList_1.text = "YOU MUST ENTER SOMETHING";
        }
    }


    // On void search
    // Find the point rebase to it
    // Draw out the context
    private void SearchNode(string SearchString) {
        // Searching means we're out of path mode
        pathMode = false;
        // loop through all the stuff in the graph thing and then find the node and 
        // Name combination which has the highest score
        float matchScore = Mathf.Infinity;
        int RebaseID = -1;
        Dictionary<string, int> SugesstionDict = new Dictionary<string, int>();

        foreach (Node node in graphStructure.nodes.Values){
            int levRatio = LevenshteinDist.Compute(SearchString, node.name);
            if (matchScore >= levRatio) {
                matchScore = levRatio;
                RebaseID = node.id;
                SugesstionDict.Add(node.name, levRatio);
            }
        }

        var sorted = from entry in SugesstionDict orderby entry.Value ascending select entry;
        var sortedDict = sorted.ToDictionary(pair => pair.Key, pair => pair.Value);

        string suggestion = sortedDict.Keys.Aggregate((i, j) => i + System.Environment.NewLine + j);
        suggestionList_1.text = suggestion;

        // For that node
        // Draw out the context
        DrawNeighbours(RebaseID, new Vector3(0f, 0f, 0f), graphStructure);
    }

    public void FindPath()
    {
        string searchstring = RebaseText.GetComponent<TMP_InputField>().text;
        string destinationString = SearchText.GetComponent<TMP_InputField>().text;

        // This if statement checks if any blanks were entered or not
        if (destinationString != null && destinationString != " " && searchstring != null && searchstring != " ")
        {
            // Chose to write the code twice cause there was some error in creating the function
            // (And twice is okay in my book lol)
            // match the suggestion list for the startnode
            float matchScore = Mathf.Infinity;
            int RebaseID = -1;
            Dictionary<string, int> SugesstionDict = new Dictionary<string, int>();

            foreach (Node node in graphStructure.nodes.Values)
            {
                int levRatio = LevenshteinDist.Compute(searchstring, node.name);
                if (matchScore >= levRatio)
                {
                    matchScore = levRatio;
                    RebaseID = node.id;
                    SugesstionDict.Add(node.name, levRatio);
                }
            }

            var sorted = from entry in SugesstionDict orderby entry.Value ascending select entry;
            var sortedDict = sorted.ToDictionary(pair => pair.Key, pair => pair.Value);

            string suggestion_1 = sortedDict.Keys.Aggregate((i, j) => i + System.Environment.NewLine + j);
            suggestionList_1.text = suggestion_1;

            // match the suggestion list for the endnode
            matchScore = Mathf.Infinity;
            int SearchID = -1;
            Dictionary<string, int> SugesstionDict_2 = new Dictionary<string, int>();

            foreach (Node node in graphStructure.nodes.Values)
            {
                int levRatio = LevenshteinDist.Compute(destinationString, node.name);
                if (matchScore >= levRatio)
                {
                    matchScore = levRatio;
                    SearchID = node.id;
                    SugesstionDict_2.Add(node.name, levRatio);
                }
            }

            var sorted_2 = from entry in SugesstionDict_2 orderby entry.Value ascending select entry;
            var sortedDict_2 = sorted_2.ToDictionary(pair => pair.Key, pair => pair.Value);

            string suggestion_2 = sortedDict_2.Keys.Aggregate((i, j) => i + System.Environment.NewLine + j);
            suggestionList_2.text = suggestion_2;

            // now the last if condition
            // if neither one of those two nodes are indexed -1 - then draw the path
            if (RebaseID != -1 && SearchID != -1){
                // Clear all the stuff i.e. delete all the chidlren
                DeleteAllChildren();
                // Draw out the path
                findAndDrawPath(RebaseID, SearchID);

                // Draw all the edges and colour out all the stuff
                // draw all the connection lines
                // DrawAllEdges();
                // ColourAllNodes();
            }
            else
            {
                if (RebaseID == -1)
                {
                    suggestionList_1.text = "Could'nt find any matches, try again";
                }
                else if (SearchID == -1)
                {
                    suggestionList_2.text = "Could'nt find any matches, try again";
                }
            }
        }
        else {
            suggestionList_1.text = "Please Enter Something!!";
            suggestionList_2.text = "Please Enter Something!!";
        }

    }

    private void DeleteAllChildren() {
        foreach (Transform child in transform){
            Destroy(child.gameObject);
        }
    }
    
    private void DrawNeighbours(int IDNUM, Vector3 pos, GraphStructure graph)
    {
        // Drawing out the context method
        // figure out what is in the proximity of the current object
        // Draw those out 
        // Using the method to create things around a sphere
        // Figure out what is in the proximity of those objects and draw them out - but 
        // its a direct vector projection and only like one object - this happens as a try and accept 
        // loop GraphStructure graph

        // Clear away everythng that is in the current table
        // if the path mode is not on!!
        if (pathMode == false){
            DeleteAllChildren();
        }

        // values for this node
        Node this_node = graph.nodes[IDNUM];
        string text = this_node.name;
        int[] edge = this_node.connected_nodes;
        // instantiate one object in the middle of the thing 
        // This will lead to Z fighting - todo fix Z Fighting
        // Proximal fix with not drawn
        if (this_node.hasBeenDrawn == false)
        {
            GameObject center = Instantiate(Node_Prefab, pos, Quaternion.identity, transform);
            
            // Set all the node objects values
            Node_object node_obj = center.GetComponent<Node_object>();
            node_obj.id = IDNUM;
            node_obj.has_been_drawn = true;
            this_node.positionPoint = new Point(0f, 0f, 0f);

            // Set all the text objects values
            TextMeshPro tpm = center.GetComponentInChildren<TextMeshPro>();
            tpm.SetText(text);
        }

        // Use the central method create out all the other points of instantiation
        List<Vector3> VertPoints = HelperFunctions.return_sphere_cordinates(edge.Length, ((float)Math.Log(edge.Length)+epsilon) * epsilon);

        // instantiate nodes at all of them
        for (int i = 0; i < edge.Length; i++){
            // store a boolean value if the proximal (degree 2 edge has been drawn or not)
            bool proximalNodeDrawn = false;

            Vector3 vert = VertPoints[i];
            int nodeOBJ_IDNUM = edge[i];

            Node degree_1_node = graph.nodes[nodeOBJ_IDNUM];
            // This is the begining of the loop to format all the degree 1 nodes
            if (degree_1_node.hasBeenDrawn != true)
            {
                // this instanciates the node
                GameObject nodeOBJ = Instantiate(Node_Prefab, vert + pos, Quaternion.identity, transform);
                nodeOBJ.transform.SetParent(transform, true);

                // this gets the node values to set the variables and then we use them
                int[] nodeOBJ_edge = degree_1_node.connected_nodes;
                string nodeOBJ_text = degree_1_node.name;

                // set all the reference values in the graph
                degree_1_node.hasBeenDrawn = true;
                degree_1_node.positionPoint = new Point(vert + pos);

                // this is to physically set the values
                Node_object nodeOBJ_ref = nodeOBJ.GetComponent<Node_object>();
                nodeOBJ_ref.id = nodeOBJ_IDNUM;
                nodeOBJ_ref.has_been_drawn = true;
                nodeOBJ_ref.parentDrawingNode = graph.nodes[IDNUM];

                // this sets all the text values
                TextMeshPro nodeOBJ_ref_TPM = nodeOBJ.GetComponentInChildren<TextMeshPro>();
                nodeOBJ_ref_TPM.SetText(nodeOBJ_text);

                // for each edge find one projective point for posterty sake
                // this loop processes the 2nd degree nodes
                for (int j = 0; j < nodeOBJ_edge.Length; j++)
                {
                    Node degree_2_node = graph.nodes[nodeOBJ_edge[j]];

                    if ((proximalNodeDrawn == false) && (degree_2_node.hasBeenDrawn == false))
                    {
                        GameObject nodeOBJ_d2 = Instantiate(Node_Prefab, 2*vert + pos, Quaternion.identity);
                        nodeOBJ_d2.transform.SetParent(transform, true);

                        string nodeOBJ_d2_text = degree_2_node.name;

                        // set all the reference values in the graph
                        degree_2_node.hasBeenDrawn = true;
                        degree_2_node.positionPoint = new Point(2 * vert + pos);

                        // this is to physically set the values
                        Node_object nodeOBJ_d2_ref = nodeOBJ_d2.GetComponent<Node_object>();
                        nodeOBJ_d2_ref.id = degree_2_node.id;
                        nodeOBJ_d2_ref.has_been_drawn = true;
                        nodeOBJ_d2_ref.parentDrawingNode = graph.nodes[nodeOBJ_IDNUM];

                        // Set the final text values
                        TextMeshPro nodeOBJ_d2_ref_TPM = nodeOBJ_d2.GetComponentInChildren<TextMeshPro>();
                        nodeOBJ_d2_ref_TPM.SetText(nodeOBJ_d2_text);

                        proximalNodeDrawn = true;
                    }
                }
                // loop for degree 2 ends here
            }
            // for loop for degree one nodes ends here
        }
        // Draw out all the edges and colour nodes 
        DrawAllEdges();
        ColourAllNodes();
    }

    private void ColourAllNodes(){
        // loop through each node 
        // in each node get the one that is tagged "PhysicalRenderer"
        // get the renderer object.
        // if on the path color it differently 
        // if not on the path - colour it something 
        // else color it something else
        foreach (Transform node in transform){
            Node_object node_ref = node.GetComponent<Node_object>();
            
            // The mouthful of an if statement - gets the associated node and then changes based on the property
            if (graphStructure.nodes[node_ref.id].fallsOnPath){
                foreach (Transform physicalObject in node){
                    if (physicalObject.tag == "PhysicalRenderer"){
                        MeshRenderer renderer = physicalObject.GetComponent<MeshRenderer>();
                        renderer.material.color = pathColour;
                    }
                }
            }
            else {
                foreach (Transform physicalObject in node){
                    if (physicalObject.tag == "PhysicalRenderer"){
                        MeshRenderer renderer = physicalObject.GetComponent<MeshRenderer>();
                        renderer.material.color = defaultColour;
                    }
                }
            }
        }
    }

    private void DrawAllEdges()
    {
        // Go through all the points that we have drawn so far
        // find out whether or not those nodes fall on the main path
        // if they dont then find out who their parent node was
        // draw a path to that guy

        foreach (Transform subObject in transform)
        {
            // get the Node_obj class - holds all the information that we need
            Node_object node = subObject.GetComponent<Node_object>();
            
            // Writing out that mouthful of an if statement
            // get the ID associated with the node and then find out what node it is, then if it falls on path or not
            if (graphStructure.nodes[node.id].fallsOnPath != true && node.parentDrawingNode != null && node.edgeHasBeenDrawn == false){
                LineRenderer line = subObject.GetComponent<LineRenderer>();
                // start from the nodes position
                line.SetPosition(0, graphStructure.nodes[node.id].positionPoint.ToVector());
                // end at the end position
                line.SetPosition(1, node.parentDrawingNode.positionPoint.ToVector());
                node.edgeHasBeenDrawn = true;
            }
        }
    }

    private void findAndDrawPath(int StartID, int EndID)
    {
        // reset everything 
        graphStructure.ResetAll();


        var lineRenderer = transform.GetComponent<LineRenderer>();
        // set the line renders size to be zero - essentially deleting the line
        lineRenderer.positionCount = 0;

        // When we search 
        // turn path mode on - this ensures that we arent deleting things as we draw them
        pathMode = true;
        // compute a path
        List<int> Path = graphStructure.BFS(graphStructure, graphStructure.nodes[StartID], graphStructure.nodes[EndID]);

        // Clear out whatever is on screen
        foreach (Transform node in transform){
            Destroy(node.gameObject);
        }

        // reset all the has been drawn stuff
        graphStructure.ResetHasBeenDrawn();

        // set the line renderer path to be the same length as the Path that we have
        lineRenderer.positionCount = Path.Count;

        // Set all the nodes in the path to be on the path object
        foreach (int NodeID in Path){
            graphStructure.nodes[NodeID].fallsOnPath = true;
        }

        // Reccursively draw out that path
        for (int i = 0; i < Path.Count; i++){
            DrawNeighbours(Path[i], graphStructure.nodes[Path[i]].positionPoint.ToVector(), graphStructure);
            lineRenderer.SetPosition(i, graphStructure.nodes[Path[i]].positionPoint.ToVector());
        }
    }

    private void RebaseCameraToTransform(Transform transform) {
        cameraPointer.transform.position = transform.position;
    }
}
