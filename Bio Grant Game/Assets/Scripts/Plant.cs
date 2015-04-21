using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour {

    // List of all available plants.
    private List<GameObject> plantList;

    // total number of plants.
    private int numOfPlants;

	/*
     * Init()
     * 
     * Custom initialization since the script
     * is never in scene. 
    */
	public void Init () {
        
        // Get the number of plants based off of
        // the number of children.
        numOfPlants = transform.childCount;
        plantList = new List<GameObject>();

        // Run through all the children and add
        // them to the list.
        for (int i = 0; i < numOfPlants; i++)
        {
            plantList.Add(transform.GetChild(i).gameObject);
        }
	}

    /*
     * GetRandomPlant()
     * 
     * Returns a random plant in the list.
    */
    public GameObject GetRandomPlant()
    {
        return plantList[Random.Range(0, numOfPlants)];
    }

    /*
     * GetPlants(int)
     * 
     * Returns a list of plants as long as the
     * int passed in. The values are randomized
     * with no repeats.
    */
    public List<GameObject> GetPlants(int num)
    {
        // Initialize temporary lists to keep track
        // of the GameObjects and the indexes those
        // GameObjects were grabbed from.
        List<GameObject> temp = new List<GameObject>();
        List<int> indexes = new List<int>();

        // Run through until the list contains num
        // elements.
        for (int i = 0; i < num; i++)
        {
            // Keep track of the index.
            int plantIndex;

            // Do-while loop to randomly grab an
            // int until one is found that was
            // not already in the indexes list.
            do
            {
                plantIndex = Random.Range(0, numOfPlants);
            } while (indexes.Contains(plantIndex));

            // When it's found, add the index and
            // the GameObject to the respective lists.
            indexes.Add(plantIndex);
            temp.Add(plantList[plantIndex]);
        }

        // Return the list.
        return temp;
    }
}
