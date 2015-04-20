using UnityEngine;
using System.Collections;

public class SearchResult : MonoBehaviour {

// Private variables.

    // Reference of the plant this result
    // will hold the information of.
    private GameObject plant;

    // Reference to the IndexManager
    // used for this session.
    private IndexManager indexManager;

    /*
     * SetPlant(GameObject, IndexManager)
     * 
     * Sets the plant information for this button, and gives it
     * a reference to the IndexManager being used for this session
     * 
     * setPlant - A copy of the plant this object will hold a reference to.
     * index - The IndexManager being used in this session.
    */
    public void SetPlant(GameObject setPlant, IndexManager index)
    {
        plant = setPlant;
        indexManager = index;
    }


    /*
     * Clicked()
     * 
     * Activates the IndexManager's SearchResultClicked when
     * the button gets clicked.
    */ 
    public void Clicked()
    {
        // Instantiate a copy when returned so that the original
        // information doesn't get tampered and so it appears
        // on the screen.
        GameObject temp = (GameObject)Instantiate(plant);

        // Call the Search Result Clicked button to 
        // change the view to looking at this plant.
        indexManager.SearchResultClicked(temp);
    }
}
