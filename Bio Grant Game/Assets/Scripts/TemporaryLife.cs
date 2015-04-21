using UnityEngine;
using System.Collections;

public class TemporaryLife : MonoBehaviour {

    // The length of time this object should remain alive.
    public float Lifetime;

	// Use this for initialization
	void Start () {

        // Set the static variable statusVisible to true
        MatchingManager.statusVisible = true;
	}
	
	// Update is called once per frame
	void Update () {

        // Check if there is still life in the object, if
        // so remove Time.deltaTime from the Lifetime.
        // If not, call end.
	    if (Lifetime > 0)
        {
            Lifetime -= Time.deltaTime;
        }
        else
        {
            End();
        }

	}

    /*
     * OnMouseDown()
     * 
     * Call end if this object is clicked on
    */
    void OnMouseDown()
    {
        End();
    }

    /*
     * End()
     * 
     * End the life of the object and change statusVisible to false.
    */
    private void End()
    {
        MatchingManager.statusVisible = false;
        Destroy(gameObject);
    }
}
