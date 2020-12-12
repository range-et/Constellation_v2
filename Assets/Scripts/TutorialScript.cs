using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    // Start is called before the first frame update
    // Reference to the tutorial screen
    public GameObject tutorialScreen;
    private static bool ShowTutorial = true;

    // Update is called once per frame
    void Update()
    {
        // Mainatin a reference to keep track if the tutorial is visible or not
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // if active turn it off
            if (ShowTutorial == false)
            {
                Show();
            }
            // if not active turn it on
            else
            {
                Hide();
            }
        }
    }

    private void Hide()
    {
        tutorialScreen.SetActive(false);
        Time.timeScale = 1f;
        ShowTutorial = false;
    }

    private void Show()
    {
        tutorialScreen.SetActive(true);
        Time.timeScale = 0f;
        ShowTutorial = true;
    }
}
