using UnityEngine;
using System.Collections;

public class selfDeactivate : MonoBehaviour {

    public bool deactivate = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DeactivateMe()
    {
        if (deactivate)
        {
            gameObject.SetActive(false);
            deactivate = false;
        }
    }
}
