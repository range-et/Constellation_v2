using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGL;

public class Node_object : MonoBehaviour
{
    // Hold the id reference
    public int id;
    // Check if it has been drawn or not
    public bool has_been_drawn = false;
    // Hold a reference to the parent that it was drawn from
    public Node parentDrawingNode;
    // Hold a reference if the edge has been drawn from this node or not
    public bool edgeHasBeenDrawn = false;
}
