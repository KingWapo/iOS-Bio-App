using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IndexManager : Manager {

    public GameObject PlantParent;
    
    // Directory public variables
    public GameObject DirectoryView;
    public GameObject DirectoryPlantPositions;
    public GameObject DirectoryTextPositions;
    public GameObject NextButton;
    public GameObject PreviousButton;

    // Info public variables
    public GameObject InfoView;
    public GameObject InfoImagePositions;
    public GameObject InfoMainImagePosition;
    public GameObject InfoPlantName;
    public GameObject InfoLatinName;
    public GameObject InfoDetails;

    private bool inDirectoryView;
    private List<GameObject> plants;

    // Directory private variables
    private int page;
    private int plantsPerPage;
    private List<GameObject> currentPlants;
    private List<Transform> directoryPlantPositions;
    private List<Transform> directoryTextPositions;

    // Info private variables
    private GameObject viewingPlant;
    private GameObject currentImage;
    private List<GameObject> imageChoices;
    private List<GameObject> infoImagePositions;

	// Use this for initialization
	void Start () {
        inDirectoryView = true;
        plants = new List<GameObject>();

        // Set up Directory
        page = 0;
        plantsPerPage = 12;
        currentPlants = new List<GameObject>();
        directoryPlantPositions = new List<Transform>();
        directoryTextPositions = new List<Transform>();
        for (int i = 0; i < PlantParent.transform.childCount; i++)
        {
            plants.Add(PlantParent.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < DirectoryPlantPositions.transform.childCount; i++)
        {
            directoryPlantPositions.Add(DirectoryPlantPositions.transform.GetChild(i));
            directoryTextPositions.Add(DirectoryTextPositions.transform.GetChild(i));
        }
        setPlants();

        // Set up info
        imageChoices = new List<GameObject>();
        infoImagePositions = new List<GameObject>();
        for (int i = 0; i < InfoImagePositions.transform.childCount - 1; i++)
        {
            infoImagePositions.Add(InfoImagePositions.transform.GetChild(i).gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnPlantClick(GameObject PlantClicked)
    {
        base.OnPlantClick(PlantClicked);
        if (inDirectoryView)
        {
            viewingPlant = PlantClicked;
            switchViews();
        }
        else
        {
            int index = imageChoices.IndexOf(PlantClicked);
            print(index);
        }
    }

    private void switchViews()
    {
        if (inDirectoryView)
        {
            inDirectoryView = false;
            DirectoryView.SetActive(false);
            InfoView.SetActive(true);
            setInfo();
        }
        else
        {
            inDirectoryView = true;
            DirectoryView.SetActive(true);
            InfoView.SetActive(false);
        }
    }

    /*
     *  Directory view methods.
     */

    public void Next()
    {
        page++;
        setPlants();
    }

    public void Previous()
    {
        page--;
        setPlants();
    }

    private void setPlants()
    {
        destroyPlants();
        if (page == 0)
        {
            PreviousButton.SetActive(false);
        }
        else
        {
            PreviousButton.SetActive(true);
        }

        if (plants.Count <= (page + 1) * plantsPerPage)
        {
            NextButton.SetActive(false);
        }
        else
        {
            NextButton.SetActive(true);
        }

        for (int i = page * plantsPerPage; i < (page + 1) * plantsPerPage && i < plants.Count; i++)
        {
            GameObject newPlant = (GameObject) Instantiate(plants[i]);
            newPlant.transform.position = directoryPlantPositions[i - page * plantsPerPage].position;
            newPlant.transform.GetChild(Random.Range(0, newPlant.transform.childCount)).gameObject.SetActive(true);
            directoryTextPositions[i - page * plantsPerPage].GetComponent<Text>().text = newPlant.GetComponent<PlantInformation>().Name;
            directoryTextPositions[i - page * plantsPerPage].transform.position = new Vector3(newPlant.transform.position.x,
                                                                                              directoryTextPositions[i - page * plantsPerPage].transform.position.y,
                                                                                              directoryTextPositions[i - page * plantsPerPage].transform.position.z);
            currentPlants.Add(newPlant);
        }
    }

    private void destroyPlants()
    {
        int count = currentPlants.Count;
        for (int i = 0; i < count; i++)
        {
            Destroy(currentPlants[0]);
            currentPlants.RemoveAt(0);
            directoryTextPositions[i].GetComponent<Text>().text = "";
        }
    }

    /*
     * Info View Functions
     */

    private void setInfo()
    {
        print(viewingPlant);
        for (int i = 0; i < viewingPlant.transform.childCount; i++)
        {
            if (imageChoices.Count > i)
            {
                Destroy(imageChoices[i]);
                imageChoices[i] = viewingPlant.transform.GetChild(i).gameObject;
            }
            else
            {
                imageChoices.Add(viewingPlant.transform.GetChild(i).gameObject);
            }

            imageChoices[i].transform.position = infoImagePositions[i].transform.position;
            imageChoices[i].transform.localScale = infoImagePositions[i].transform.localScale;
            imageChoices[i].SetActive(true);
        }

        setCurrentImage();

        InfoPlantName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Name.ToUpper();
        InfoLatinName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().LatinName;
        for (int i = 0; i < viewingPlant.GetComponent<PlantInformation>().Details.Count && i < InfoDetails.transform.childCount; i++)
        {
            InfoDetails.transform.GetChild(i).gameObject.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Details[i];
        }
    }

    private void setCurrentImage()
    {
        Destroy(currentImage);
        currentImage = (GameObject)Instantiate(imageChoices[0]);
        currentImage.transform.position = InfoMainImagePosition.transform.position;
        currentImage.transform.localScale = InfoMainImagePosition.transform.localScale;
    }
}
