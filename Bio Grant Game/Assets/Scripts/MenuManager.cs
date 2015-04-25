using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        
        // Checks if Escape is hit to exit.
        // Unneccesary since it'll be an iPad app.
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitClicked();
        }
	}

    // Switch to Index Screen where the user
    // can look at the various plants.
    public void IndexClicked()
    {
        Application.LoadLevel("Index");
    }

    // Switch to Identify Screen where the
    // user can try to guess the plant
    // based off of information given to them.
    public void IdentifyPlantsClicked()
    {
        Application.LoadLevel("Identify");
    }

    // Switch to the Credits Screen to
    // view the credits for the application.
    public void CreditsClicked()
    {
        Application.LoadLevel("Credits");
    }

    // Switch to the Main Menu Screen
    public void BackClicked()
    {
        Application.LoadLevel("MainMenu");
    }

    // Exit the application.
    public void ExitClicked()
    {
        Application.Quit();
    }
}
