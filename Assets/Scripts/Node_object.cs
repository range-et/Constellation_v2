using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PGL;
using TMPro;

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
    // Name of the node 
    public string Name;

    public Color HoverColour;
    public Color DefaultColour;

    public TextMeshProUGUI PositionText;


    private void OnMouseEnter()
    {
        foreach (Transform physicalObject in transform)
        {
            if (physicalObject.tag == "PhysicalRenderer")
            {
                Material renderer = physicalObject.GetComponent<MeshRenderer>().material;
                renderer.SetColor("_EmissionColor", HoverColour);
            }
        }
    }

    private void OnMouseExit(){
        foreach (Transform physicalObject in transform)
        {
            if (physicalObject.tag == "PhysicalRenderer")
            {
                Material renderer = physicalObject.GetComponent<MeshRenderer>().material;
                renderer.SetColor("_EmissionColor", DefaultColour);
            }
        }
    }
}
