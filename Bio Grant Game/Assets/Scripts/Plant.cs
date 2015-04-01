using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Plant : MonoBehaviour {

    private List<GameObject> plantList;
    private int numOfPlants;

	// Use this for initialization
	public void Init () {
        numOfPlants = transform.childCount;
        plantList = new List<GameObject>();
        for (int i = 0; i < numOfPlants; i++)
        {
            plantList.Add(transform.GetChild(i).gameObject);
        }
	}

    public GameObject GetRandomPlant()
    {
        return plantList[Random.Range(0, numOfPlants)];
    }

    public List<GameObject> GetPlants(int num)
    {
        List<GameObject> temp = new List<GameObject>();
        List<int> indexes = new List<int>();
        for (int i = 0; i < num; i++)
        {
            int plantIndex;
            do
            {
                plantIndex = Random.Range(0, numOfPlants);
            } while (indexes.Contains(plantIndex));
            indexes.Add(plantIndex);
            temp.Add(plantList[plantIndex]);
        }

        return temp;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
