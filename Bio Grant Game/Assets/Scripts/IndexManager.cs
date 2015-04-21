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
     * When a search result is clicked, switch to the
     * Info view on that plant.
     * 
     * plant - a copy of the plant that will be the 
     *         focus of the Info view.
    */
    public void SearchResultClicked(GameObject plant)
    {
        // Set all the objects that are
        // associated with Search View
        // to false so they are NOT seen.
        SearchView.SetActive(false);

        // Set all the objects that are
        // associated with Info View to
        // true so they ARE seen.
        InfoView.SetActive(true);

        // Set the viewing plant to the
        // plant that is passed in.
        viewingPlant = plant;

        // Set up the information of the plant that
        // should be viewed in Info View.
        setInfo();
    }

    /*
     * OnXButtonClicked()
     * 
     * When the X button is clicked, exit this scene
     * and return to the main menu.
    */
    public void OnXButtonClicked()
    {
        Application.LoadLevel("MainMenu");
    }

    // -----------------------------------------------------------------
    //
    // Directory view methods.
    //
    // -----------------------------------------------------------------

    /*
     * Next()
     * 
     * Incrememt the page and reset the plants. Called
     * when the Next button is hit in game.
    */
    public void Next()
    {
        page++;
        setPlants();
    }

    /*
     * Previous()
     * 
     * Decrement the page and reset the plants. Called
     * when the Previous button is hit in game.
    */
    public void Previous()
    {
        page--;
        setPlants();
    }

    /*
     * setPlants()
     * 
     * Reset the images for the plants in the directory.
    */
    private void setPlants()
    {
        // Destroy the previous set of plants.
        destroyPlants();

        // If the page is 0, the set the previous button
        // to inactive so that the user can't go back
        // any further.
        // Else set it to active.
        if (page == 0)
        {
            PreviousButton.SetActive(false);
        }
        else
        {
            PreviousButton.SetActive(true);
        }

        // If the page is the last page, determined by
        // (page + 1) * plantsPerPage being greater than
        // the amount of plants, then set the Next button
        // to inactive so that the user can't go further
        // forward.
        // Else set it to active.
        if (plants.Count <= (page + 1) * plantsPerPage)
        {
            NextButton.SetActive(false);
        }
        else
        {
            NextButton.SetActive(true);
        }

        // Run through the plants that should be seen
        // on this page, and display them in the correct spots.
        for (int i = page * plantsPerPage; i < (page + 1) * plantsPerPage && i < plants.Count; i++)
        {
            // Instantiate the object to make a copy into the scene.
            GameObject newPlant = (GameObject) Instantiate(plants[i]);

            // Set the scale to .8 x .8
            newPlant.transform.localScale = new Vector3(0.8f, 0.8f, 1);

            // Place the position based off of the plant positions.
            newPlant.transform.position = directoryPlantPositions[i - page * plantsPerPage].position;

            // Get a random image that is a child of this plant
            // and set it active. Randomizes the images on the
            // Directory view.
            newPlant.transform.GetChild(Random.Range(0, newPlant.transform.childCount)).gameObject.SetActive(true);
            
            // Set the text of the Text component in the directoryTextPositions
            // and adjust it's positions. 
            directoryTextPositions[i - page * plantsPerPage].GetComponent<Text>().text = newPlant.GetComponent<PlantInformation>().Name;
            directoryTextPositions[i - page * plantsPerPage].transform.position = new Vector3(newPlant.transform.position.x,
                                                                                              directoryTextPositions[i - page * plantsPerPage].transform.position.y,
                                                                                              directoryTextPositions[i - page * plantsPerPage].transform.position.z);
            
            // Add the plant to currentPlants to track objects.
            currentPlants.Add(newPlant);
        }
    }

    /*
     * destroyPlants()
     * 
     * Destroy the previous objects within currentPlants and
     * remove at that position to avoid nulls. Set text to
     * blank as well. 
    */
    private void destroyPlants()
    {
        int count = currentPlants.Count;
        for (int i = 0; i < count; i++)
        {
            // Destroy the game object at pos 0 and remove it
            // from the array.
            Destroy(currentPlants[0]);
            currentPlants.RemoveAt(0);

            // Set the text to blank.
            directoryTextPositions[i].GetComponent<Text>().text = "";
        }
    }

    /*
     * hidePlants()
     * 
     * Hide all the images for the plants that
     * are currently showing.
    */
    private void hidePlants()
    {
        for (int i = 0; i < currentPlants.Count; i++)
        {
            // Call clear images to make sure that all the children
            // of the plant are hidden.
            currentPlants[i].GetComponent<PlantInformation>().ClearImages();
        }
    }

    /*
     * showPlants()
     * 
     * Run through the current plants and set one of the chidren at
     * random to be active.
    */
    private void showPlants()
    {
        for (int i = 0; i < currentPlants.Count; i++)
        {
            currentPlants[i].transform.GetChild(Random.Range(0, currentPlants[i].transform.childCount)).gameObject.SetActive(true);
        }
    }


    // -----------------------------------------------------------------
    //
    // Info View Functions
    //
    // -----------------------------------------------------------------

    /*
     * setInfo()
     * 
     * Set up all the objects for the Info View and display it.
    */
    private void setInfo()
    {
        // Clear the currently displayed images on the
        // viewingPlant.
        viewingPlant.GetComponent<PlantInformation>().ClearImages();

        // Run through the children of the viewing plant to
        // set up imageChoices arrayList.
        for (int i = 0; i < viewingPlant.transform.childCount; i++)
        {
            // If there are more images than where we are
            // in the array, then there is an object at
            // that point. Therefore destroy the object
            // and replace it with a new one.
            // Else, just add it to the list.
            if (imageChoices.Count > i)
            {
                // Destroy the object, and Instantiate the viewing
                // plant to make an object place it at i in the array.
                Destroy(imageChoices[i]);
                imageChoices[i] = (GameObject) Instantiate(viewingPlant);
            }
            else
            {
                // Instantiate the viewingPlant and add it to the List.
                GameObject newPlant = (GameObject)Instantiate(viewingPlant);
                imageChoices.Add(newPlant);
            }

            // Set the image at child i to active.
            imageChoices[i].transform.GetChild(i).gameObject.SetActive(true);

            // Place it in the correct position and scale based off of 
            // the info from infoImagePositions.
            imageChoices[i].transform.position = infoImagePositions[i].transform.position;
            imageChoices[i].transform.localScale = infoImagePositions[i].transform.localScale;

            // Set this object to active to show it.
            imageChoices[i].SetActive(true);
        }

        // Sets the current image to the
        // 0th index of imageChoices.
        setCurrentImage(0);

        // Set the text in the InfoPlantName and InfoLatinName to the
        // respective information.
        InfoPlantName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Name.ToUpper();
        InfoLatinName.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().LatinName;

        // Run through the details and set the text in InfoDetails' children to the correct info.
        for (int i = 0; i < viewingPlant.GetComponent<PlantInformation>().Details.Count && i < InfoDetails.transform.childCount; i++)
        {
            InfoDetails.transform.GetChild(i).gameObject.GetComponent<Text>().text = viewingPlant.GetComponent<PlantInformation>().Details[i];
        }
    }

    /*
     * setCurrentImage(int)
     * 
     * Set the main image displayed based off
     * the index being passed in.
    */
    private void setCurrentImage(int index)
    {
        // Destroy the currentImage to remove it.
        Destroy(currentImage);

        // Instantiate a new version of the plant from
        // imageChoices based off the index passed in.
        currentImage = (GameObject)Instantiate(imageChoices[index]);

        // Set currentImage position and scale based off
        // of InfoMainImagePosition.
        currentImage.transform.position = InfoMainImagePosition.transform.position;
        currentImage.transform.localScale = InfoMainImagePosition.transform.localScale;
    }

    /*
     * destroyInfo()
     * 
     * Run through and destroy the objects used
     * in the Info view from imageChoices and
     * currentImage.
    */
    private void destroyInfo()
    {
        for (int i = 0; i < imageChoices.Count; i++)
        {
            Destroy(imageChoices[i]);
        }
        Destroy(currentImage);
    }
}
