using UnityEngine;
using System.Collections;

public class TemporaryLife : MonoBehaviour {

    public float Lifetime;

	// Use this for initialization
	void Start () {
        MatchingManager.statusVisible = true;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Lifetime > 0)
        {
            Lifetime -= Time.deltaTime;
        }
        else
        {
            End();
        }

	}

    void OnMouseDown()
    {
        End();
    }

    private void End()
    {
        MatchingManager.statusVisible = false;
        Destroy(gameObject);
    }
}
