using UnityEngine;
using System.Collections;

public class wasteActive : MonoBehaviour {


    public int wasteActiveIndex = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUpAsButton()
    {
        kdeck.GetInstance().OnWasteActiveClicked(this);    
    }

    void OnMouseDown()
    {
        kdeck.GetInstance().OnWasteActiveDrag(this);
    }

    void OnMouseUp()
    {
        kdeck.GetInstance().OnWasteActiveDrop(this);
    }
}
