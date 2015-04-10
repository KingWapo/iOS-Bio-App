using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantInformation : MonoBehaviour
{

    public string Name;
    public string LatinName;
    public bool IsInvasive;
    public List<string> Details;
    public bool IsCorrect;
    public GameObject PlantList;

    private GameObject Manager;

    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("Manager");
    }

    void OnMouseDown()
    {
        if (!MatchingManager.statusVisible)
        {
            Manager.GetComponent<Manager>().OnPlantClick(gameObject);
        }
    }

    public void ClearImages()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}