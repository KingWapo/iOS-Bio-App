using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitClicked();
        }
	}

    public void IndexClicked()
    {
        Application.LoadLevel("Index");
    }

    public void IdentifyPlantsClicked()
    {
        Application.LoadLevel("Identify");
    }

    public void CreditsClicked()
    {

    }

    public void ExitClicked()
    {
        Application.Quit();
    }
}
