using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Searching : MonoBehaviour {

// Public variables

    // The input field the user would type into.
    public InputField input;

    // The prefab for the Search Result button
    // to be used in displaying the search results.
    public GameObject searchResultPrefab;

// Private variables

    // This objects IndexManager Component.
    private IndexManager indexManager;
    
    // List to hold all the plants.
    private List<GameObject> plantList;

    // List to hold search results that
    // match the search term.
    private List<GameObject> searchResults;

    // Previous and current search term
    // to determine if the user typed or
    // not.
    private string prevSearchTerm = "";
    private string newSearchTerm = "";

	// Use this for initialization
	void Start () {

        // Recieve a reference to the IndexManager component
        // on this object.
        indexManager = GetComponent<IndexManager>();

        // A copy of the total list of plants
        // in this database.
        plantList = new List<GameObject>();

        // A list of the results from searching
        // for the plants.
        searchResults = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        // If we don't have a list of plants then get
        // one by using the IndexManager's GetPlants
        // method.
        if (plantList.Count == 0)
        {
            plantList = indexManager.GetPlants();
        }

        // Grab the latest search term from the 
        // input's text.
        newSearchTerm = input.text;

        // The search term changed from what it was previously
        // avoids updating the search results if we already
        // have them.
	    if (newSearchTerm != prevSearchTerm)
        {
            // Empty out the list and destroy its previous
            // objects to remove them from the scene.
            emptySearch();

            for (int i = 0; i < plantList.Count; i++ )
            {
                // Grab the PlantInformation component from a plant.
                PlantInformation info = plantList[i].GetComponent<PlantInformation>();

                // Compare the name of the plant to see if it contains the search term
                // used. If it does add it to search results. Max out at 10.
                if (info.Name.ToLower().Contains(newSearchTerm.ToLower()) && searchResults.Count < 10)
                {
                    // Create a temp GameObject, to create a copy of the
                    // search result prefab button.
                    GameObject result = (GameObject)Instantiate(searchResultPrefab);

                    // Set its parent as the Input Field
                    result.transform.SetParent(input.transform);

                    // Position the button based off of how many we have already.
                    result.transform.localPosition = new Vector3(0, (searchResults.Count + 1) * -30, 0);

                    // Get the SearchResult component on the button and Set the plant info
                    // for use if it is clicked.
                    result.GetComponent<SearchResult>().SetPlant(info.gameObject, indexManager);

                    // Set the text of the button by grabbing the child and its
                    // Text component.
                    result.transform.GetChild(0).gameObject.GetComponent<Text>().text = info.Name;

                    // It get's scaled weird, so make sure it's still <1,1,1>
                    result.transform.localScale = new Vector3(1, 1, 1);

                    // Add the result to searchResults list so it can get destroyed
                    // when we don't want it anymore.
                    searchResults.Add(result);
                }
            }
            // Set the previous serach term so we 
            // know if it changes or not.
            prevSearchTerm = newSearchTerm;
        }
	}

    /*
     * emptySearch()
     * 
     * Destroys all objects in searchResults list
     * and clears it for use of next set of 
     * search results.
    */ 
    private void emptySearch()
    {
        while(searchResults.Count > 0)
        {
            // Destroy the button saved at 
            // position zero.
            Destroy(searchResults[0]);

            // Remove this position to
            // avoid holding null references.
            searchResults.RemoveAt(0);
        }
    }
}
