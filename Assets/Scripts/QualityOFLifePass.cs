using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityOFLifePass : MonoBehaviour
{
    // Reference to the navigator text
    public TextMeshProUGUI NavigationQueues;

    // Start is called before the first frame update
    void Start()
    {
        // set the navigation text to be rightclick navigation | selection
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            NavigationQueues.text = "Left hold + Drag to rotate | Left hold + Scroll to zoom";
        }
        else {
            NavigationQueues.text = "Left click for navigation | Right click on nodes to select";
        }
    }
}
