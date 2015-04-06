using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndexManager : MonoBehaviour {

    public GameObject PlantParent;
    public GameObject DirectoryView;
    public GameObject DirectoryPlantPositions;
    public GameObject DirectoryTextPositions;
    public GameObject InfoView;

    private bool inDirectoryView;
    private int page;
    private int plantsPerPage;
    private List<GameObject> plants;

	// Use this for initialization
	void Start () {
        inDirectoryView = true;
        page = 0;
        plantsPerPage = 12;
        plants = new List<GameObject>();
        for (int i = 0; i < PlantParent.transform.childCount; i++)
        {
            plants.Add(PlantParent.transform.GetChild(i).gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
        for (int i = page * plantsPerPage; i < (page + 1) * plantsPerPage || i < plants.Count; i++)
        {

        }
    }
}
