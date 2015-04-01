using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MatchingManager : MonoBehaviour {

    public static bool statusVisible;

    public GameObject PlantList;
    public List<GameObject> Positions;
    public GameObject Title;

    public GameObject CorrectPrefab;
    public GameObject IncorrectPrefab;

    private int amountOfPlants;
    private List<GameObject> Plants;
    private List<GameObject> SessionPlants;
    private List<GameObject> PlantsInUse;
    private List<GameObject> TextPositions;
    private GameObject correctPlant;

	// Use this for initialization
	void Start () {
        amountOfPlants = PlantList.transform.childCount;
        Plants = new List<GameObject>();
        SessionPlants = new List<GameObject>();
        PlantsInUse = new List<GameObject>();
        TextPositions = new List<GameObject>();
        PlantList.GetComponent<Plant>().Init();
        for (int i = 0; i < amountOfPlants; i++)
        {
            Plants.Add(PlantList.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < Title.transform.childCount; i++ )
        {
            TextPositions.Add(Title.transform.GetChild(i).gameObject);
        }

        SetupMatching();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.R))
        {
            SetupMatching();
        }
	}

    public void SetupMatching()
    {
        ErasePreviousMatching();

        SessionPlants = PlantList.GetComponent<Plant>().GetPlants(4);
        for (int i = 0; i < SessionPlants.Count; i++)
        {
            GameObject plant = (GameObject)Instantiate(SessionPlants[i]);
            plant.transform.position = Positions[i].transform.position;
            plant.transform.GetChild(Random.Range(0, plant.transform.childCount)).gameObject.SetActive(true);
            PlantsInUse.Add(plant);
        }
        correctPlant = PlantsInUse[Random.Range(0, PlantsInUse.Count)];
        correctPlant.GetComponent<PlantInformation>().IsCorrect = true;
        int index = 0;
        //Title.GetComponent<Text
        Title.GetComponent<Text>().text = correctPlant.GetComponent<PlantInformation>().Name + " (" +
                                          correctPlant.GetComponent<PlantInformation>().LatinName + ")";
        while (index < TextPositions.Count && index < correctPlant.GetComponent<PlantInformation>().Details.Count)
        {
            TextPositions[index].GetComponent<Text>().text = correctPlant.GetComponent<PlantInformation>().Details[index];
            index++;
        }
    }

    private void ErasePreviousMatching()
    {
        while (PlantsInUse.Count > 0)
        {
            GameObject plant = PlantsInUse[0];
            PlantsInUse.RemoveAt(0);
            plant.GetComponent<PlantInformation>().IsCorrect = false;
            DestroyImmediate(plant, true);
        }

        for (int i = 0; i < TextPositions.Count; i++)
        {
            TextPositions[i].GetComponent<Text>().text = "";
        }
    }

    public void ChoiceMade(bool isCorrect)
    {
        if (isCorrect)
        {
            Instantiate(CorrectPrefab);
        }
        else
        {
            Instantiate(IncorrectPrefab);
        }

        SetupMatching();
    }

    public void XButtonClicked()
    {
        Application.LoadLevel("MainMenu");
    }
}
