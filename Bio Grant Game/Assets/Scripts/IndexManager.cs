using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class IndexManager : Manager {

// Public variables.

    // A reference to the Parent holding
    // all the plants.
    public GameObject PlantParent;
    
    /* Directory public variables */

    // The view holding the various objects that
    // can only be viewed in the directory setup.
    public GameObject DirectoryView;

    // The Parent objects holding the positions
    // to dynamically place the plant's pictures
    // and names at.
    public GameObject DirectoryPlantPositions;
    public GameObject DirectoryTextPositions;

    // Next and Previous button used to move
    // forward or backward down the list.
    // Need a reference to activate or deactivate
    // them as needed.
    public GameObject NextButton;
    public GameObject PreviousButton;

    /* Info public variables */

    // The view holding the various objects that
    // can only be viewed in the directory setup.
    public GameObject InfoView;

    // The Parent object holding the positions
    // to dynamically place the various images 
    // of the plant.
    public GameObject InfoImagePositions;

    // The main image position and scale.
    public GameObject InfoMainImagePosition;

    // The Text object to hold the name.
    public GameObject InfoPlantName;

    // The Text object to hold the Latin name.
    public GameObject InfoLatinName;

    // The parent object holding the text
    // objects for the plant's details.
    public GameObject InfoDetails;

    /* Search public variables */

    // The view holding the various objects that
    // can only be viewed in the Search View.
    public GameObject SearchView;

// Private variables.

    // Boolean to determine if the user is
    // in the directory view or not.
    private bool inDirectoryView;

    // A list to hold all the various objects
    // the user can interact with.
    private List<GameObject> plants;

    /* Directory private variables */

    // The current page the user is on
    // for viewing the plants.
    private int page;

    // How many plants can fit on a page
    private int plantsPerPage;

    // List of the current plants that are
    // being shown on a page. 
    private List<GameObject> currentPlants;

    // List of the plant positions for the images
    // grabbed from the parent.
    private List<Transform> directoryPlantPositions;

    // List of the plant positions for the text
    // grabbed from the parent.
    private List<Transform> directoryTextPositions;

    /* Info private variables */
    
    // The plant that is being looked at
    // for further information.
    private GameObject viewingPlant;

    // The current image being shown as
    // the large main image for the plant.
    private GameObject currentImage;

    // A list of all the plant's different
    // images.
    private List<GameObject> imageChoices;

    // A list of positions for th images in
    // imageChoices to be placed. Grabbed from
    // the parent.
    private List<GameObject> infoImagePositions;

	// Use this for initialization
	void Start () {
        // Start out in Directory view.
        inDirectoryView = true;

        // Initialize the plants list.
        plants = new List<GameObject>();

        // Initialize the Directory variables.
        page = 0;
        plantsPerPage = 12;
        currentPlants = new List<GameObject>();
        directoryPlantPositions = new List<Transform>();
        directoryTextPositions = new List<Transform>();

        // Grab each child of the PlantParent and add it to
        // the plants list for references to all the plants.
        for (int i = 0; i < PlantParent.transform.childCount; i++)
        {
            plants.Add(PlantParent.transform.GetChild(i).gameObject);
        }

        // Grab each child of the DirectoryPlantPosition and
        // DirectoryTextPosition and add them to a list for 
        // references to the positions.
        for (int i = 0; i < DirectoryPlantPositions.transform.childCount; i++)
        {
            directoryPlantPositions.Add(DirectoryPlantPositions.transform.GetChild(i));
            directoryTextPositions.Add(DirectoryTextPositions.transform.GetChild(i));
        }

        // Call setPlants() to set up the images based off of these
        // initialzed values.
        setPlants();

        // Initialize the Info variables.
        imageChoices = new List<GameObject>();
        infoImagePositions = new List<GameObject>();

        // Grab each child of the InfoImagePositions and add them to the
        // infoImagePositions for references to the position.
        for (int i = 0; i < InfoImagePositions.transform.childCount - 1; i++)
        {
            infoImagePositions.Add(InfoImagePositions.transform.GetChild(i).gameObject);
        }
	}

    /*
     * OnPlantClicked(GameObject)
     * 
     * When a plant is clicked on screen, call this
     * method to determine what to do.
     * 
     * PlantClicked - Reference to the plant that was
     *                clicked.
    */
    public override void OnPlantClick(GameObject PlantClicked)
    {
        // Call the Manager's OnPlantClicked() first.
        // Unneccessary for now since there's nothing
        // in Manager's OnPlantClicked().
        base.OnPlantClick(PlantClicked);

        // Determine if in Directory view or
        // info view.
        // If in Directory view, switch to Info.
        // If in Info View, switch main image.
        if (inDirectoryView)
        {
            // Set the viewingPlant to
            // be the one that's clicked.
            viewingPlant = PlantClicked;

            // Switch the views to Info View.
            SwitchViews();
        }
        else
        {
            // Grab the index of the image clicked on
            // in the imageChoices list.
            int index = imageChoices.IndexOf(PlantClicked);

            // If the image exists, set this as the
            // main image. If not, I don't know what
            // was clicked so don't do anything!
            if (index > 0)
                setCurrentImage(index);
        }
    }

    /*
     * GetPlants()
     * 
     * Returns the list of all the plants.
    */
    public List<GameObject> GetPlants()
    {
        return plants;
    }

    /*
     * SwitchViews()
     * 
     * Switch the view from Directory to Info
     * or vice versa.
    */
    public void SwitchViews()
    {
        // If in Directory, switch to Info,
        // If in Info, switch to Directory.
        if (inDirectoryView)
        {
            inDirectoryView = false;

            // Set all the objects that are
            // associated with Directory View
            // to false so they are NOT seen.
            DirectoryView.SetActive(false);

            // Set all the objects that are
            // associated with Info View to
            // true so they ARE seen.
            InfoView.SetActive(true);

            // Hide the images of all the plants that were currently
            // being viewed in the Directory View.
            hidePlants();

            // Set up the information of the plant that
            // should be viewed in Info View.
            setInfo();
        }
        else
        {
            inDirectoryView = true;

            // Set all the objects that are
            // associated with Directory View
            // to true so they ARE seen.
            DirectoryView.SetActive(true);

            // Set all the objects that are
            // associated with Info View to
            // false so they are NOT seen.
            InfoView.SetActive(false);

            // Reactivates an image for each plant 
            // that was in the currently viewed images
            // so the user doesn't lose their page.
            showPlants();

            // Destroy all the temp objects used
            // in info since they will not be needed anymore.
            destroyInfo();
        }
    }

    /*
     * SearchClicked()
     * 
     * If the user clicks the search button
     * send them into the hybrid view mode
     * for displaying search results.
    */
    public void SearchClicked()
    {
        inDirectoryView = false;

        // Set all the objects that are
        // associated with Directory View
        // to false so they are NOT seen.
        DirectoryView.SetActive(false);

        // Set all the objects that are
        // associated with Search View
        // to true so they ARE seen.
        SearchView.SetActive(true);

        // Hide the images of all the plants that were currently
        // being viewed in the Directory View.
        hidePlants();
    }

    /*
     * SearchResultClicked(GameObject)
     * 
     * 
    */
    public void SearchResultClicked(GameObject plant)
    {
        SearchView.SetActive(false);
        InfoView.SetActive(true);
        viewingPlant = plant;
        setInfo();
    }

    public void OnXButtonClicked()
    {
        Application.LoadLevel("MainMenu");
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
            newPlant.transform.localScale = new Vector3(0.8f, 0.8f, 1);
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

    private void hidePlants()
    {
        for (int i = 0; i < currentPlants.Count; i++)
        {
            currentPlants[i].GetComponent<PlantInformation>().ClearImages();
        }
    }

    private void showPlants()
    {
        for (int i = 0; i < currentPlants.Count; i++)
        {
            currentPlants[i].transform.GetChild(Random.Range(0, currentPlants[i].transform.childCount)).gameObject.SetActive(true);
        }
    }

    /*
     * Info View Functions
     */

    private void setInfo()
    {
        viewingPlant.GetComponent<PlantInformation>().ClearImages();
        for (int i = 0; i < viewingPlant.transform.childCount; i++)
        {
            if (imageChoices.Count > i)
            {
                Destroy(imageChoices[i]);
                imageChoices[i] = (GameObject) Instantiate(viewingPlant);
            }
            else
            {
                GameObject newPlant = (GameObject)Instantiate(viewingPlant);
                imageChoices.Add(newPlant);
            }

            imageChoices[i].transform.GetChild(i).gameObject.SetActive(true);
            imageChoices[i].transform.position = infoImagePositions[i].transform.position;
            imageChoices[i].transform.localScale = infoImagePositions[i].transform.localScale;
            imageChoices[i].SetActive(true);
        }

        setCurrentImage(0);

        InfoPlantName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Name.ToUpper();
        InfoLatinName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().LatinName;
        for (int i = 0; i < viewingPlant.GetComponent<PlantInformation>().Details.Count && i < InfoDetails.transform.childCount; i++)
        {
            InfoDetails.transform.GetChild(i).gameObject.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Details[i];
        }
    }

    private void setCurrentImage(int index)
    {
        Destroy(currentImage);
        currentImage = (GameObject)Instantiate(imageChoices[index]);
        currentImage.transform.position = InfoMainImagePosition.transform.position;
        currentImage.transform.localScale = InfoMainImagePosition.transform.localScale;
    }

    private void destroyInfo()
    {
        for (int i = 0; i < imageChoices.Count; i++)
        {
            Destroy(imageChoices[i]);
        }
        Destroy(currentImage);
    }
}
