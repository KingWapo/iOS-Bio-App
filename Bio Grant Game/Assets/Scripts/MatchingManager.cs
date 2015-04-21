using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchingManager : Manager {

// Public variables

    // Static bool to keep track globally of
    // if the status is being displayed or not.
    public static bool statusVisible;

    // Parent object of the total list of plants.
    public GameObject PlantList;

    // List of the positions to place
    // the images at.
    public List<GameObject> Positions;

    // Object holding Text component to display
    // the name of the plant the user is looking
    // for. Also houses the positions for the
    // details as children.
    public GameObject Title;

    // Prefabs holding the display of the
    // correct and incorrect graphics.
    public GameObject CorrectPrefab;
    public GameObject IncorrectPrefab;

// Private variables

    // The total amount of plants.
    private int amountOfPlants;

    // List of the plants.
    private List<GameObject> Plants;

    // Direct list of the chosen plants.
    // These aren't in the scene.
    private List<GameObject> SessionPlants;

    // Plants that are being used in scene
    // at a given time.
    private List<GameObject> PlantsInUse;

    // Positions for the details of the plant.
    private List<GameObject> TextPositions;

    // Reference to the plant the user is lookign for.
    private GameObject correctPlant;

	// Use this for initialization
	void Start () {

        // Initialize the variables.
        amountOfPlants = PlantList.transform.childCount;
        Plants = new List<GameObject>();
        SessionPlants = new List<GameObject>();
        PlantsInUse = new List<GameObject>();
        TextPositions = new List<GameObject>();
        PlantList.GetComponent<Plant>().Init();

        // Run through the PlantList's children and
        // add them to Plants.
        for (int i = 0; i < amountOfPlants; i++)
        {
            Plants.Add(PlantList.transform.GetChild(i).gameObject);
        }

        // Run through the Title's children and adds them
        // to the TextPositions.
        for (int i = 0; i < Title.transform.childCount; i++ )
        {
            TextPositions.Add(Title.transform.GetChild(i).gameObject);
        }

        // Initialze the matching session.
        SetupMatching();
	}

    /*
     * SetupMatching()
     * 
     * Erase the previous set up and choose new plants
     * and information to display. 
    */
    public void SetupMatching()
    {
        // Removes all previous objects no longer needed.
        ErasePreviousMatching();

        // Gets four plants using the Plant function.
        SessionPlants = PlantList.GetComponent<Plant>().GetPlants(4);

        // Run through the count of the Session plants and
        // Instantiate copies into the game.
        for (int i = 0; i < SessionPlants.Count; i++)
        {
            // Instantiate a copy of the plant.
            GameObject plant = (GameObject)Instantiate(SessionPlants[i]);

            // Set its position.
            plant.transform.position = Positions[i].transform.position;

            // Set a random child image to be visible.
            plant.transform.GetChild(Random.Range(0, plant.transform.childCount)).gameObject.SetActive(true);

            // Add to the PlantsInUse list.
            PlantsInUse.Add(plant);
        }
        // Randomly choose one of the four plants to be the correct choice.
        // Set that IsCorrect bool on it to true.
        correctPlant = PlantsInUse[Random.Range(0, PlantsInUse.Count)];
        correctPlant.GetComponent<PlantInformation>().IsCorrect = true;

        // Set the title's text to be the Name and Latin Name of the plant
        Title.GetComponent<Text>().text = correctPlant.GetComponent<PlantInformation>().Name + " (" +
                                          correctPlant.GetComponent<PlantInformation>().LatinName + ")";

        // Run through the details and set the text of the TextPositions to that.
        int index = 0;
        while (index < TextPositions.Count && index < correctPlant.GetComponent<PlantInformation>().Details.Count)
        {
            TextPositions[index].GetComponent<Text>().text = correctPlant.GetComponent<PlantInformation>().Details[index];
            index++;
        }
    }

    /*
     * ErasePreviousMatching()
     * 
     * Destroy the objects being used to clear the screen.
    */
    private void ErasePreviousMatching()
    {
        // Run through and destroy the plants
        // in PlantsInUse.
        while (PlantsInUse.Count > 0)
        {
            GameObject plant = PlantsInUse[0];
            PlantsInUse.RemoveAt(0);
            plant.GetComponent<PlantInformation>().IsCorrect = false;
            DestroyImmediate(plant, true);
        }

        // Run through and set the texts blank
        // in TextPositions.
        for (int i = 0; i < TextPositions.Count; i++)
        {
            TextPositions[i].GetComponent<Text>().text = "";
        }
    }

    /*
     * OnPlantClicked(GameObject)
     * 
     * Inherited from Manager. Check if the clicked
     * plant is correct or not, display the respective
     * prefab, and setup the next matching. 
    */
    public override void OnPlantClick(GameObject PlantClicked)
    {
        // Call the parent OnPlantClick method
        // Unneccessary since there is nothing
        // in there for now.
        base.OnPlantClick(PlantClicked);

        // Determine if the clicked plant is
        // correct or not.
        bool isCorrect = PlantClicked.GetComponent<PlantInformation>().IsCorrect;

        // Display the prefab based off of
        // if the clicked plant is correct
        // or not. 
        if (isCorrect)
        {
            Instantiate(CorrectPrefab);
        }
        else
        {
            Instantiate(IncorrectPrefab);
        }

        // Setup the next round.
        SetupMatching();
    }

    /*
     * XButtonClicked()
     * 
     * If the X button is clicked, go back to
     * the main menu.
    */
    public void XButtonClicked()
    {
        Application.LoadLevel("MainMenu");
    }
}
