using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantInformation : MonoBehaviour
{

//Public variables

    // Name of the plant
    public string Name;
    
    // Latin name of the plant.
    public string LatinName;

    // If the plant is invasive or not.
    public bool IsInvasive;

    // A list of details about the plant.
    public List<string> Details;

    // If this is the correct plant
    // when playing the identifying
    // game.
    public bool IsCorrect;

    // A reference to its parent.
    public GameObject PlantList;

    // A reference to the manager object in the scene.
    private GameObject Manager;

    // Initialize the Manager.
    void Start()
    {
        // Find the Manager in the scene.
        Manager = GameObject.FindGameObjectWithTag("Manager");
    }

    /*
     * Called when the collider on this object is pressed.
    */ 
    void OnMouseDown()
    {
        // Check if the status of correct or in correct
        // is shown. If it is not, then call the
        // OnPlantClick function in the Manager class.
        if (!MatchingManager.statusVisible)
        {
            Manager.GetComponent<Manager>().OnPlantClick(gameObject);
        }
    }

    /*
     * ClearImages()
     * 
     * Runs through the children of this object and
     * sets them to false so if any image is visible
     * it is set to inactive.
    */ 
    public void ClearImages()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}