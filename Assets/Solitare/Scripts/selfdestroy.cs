using UnityEngine;
using System.Collections;

public class selfdestroy : MonoBehaviour {

    public void DestroyMe()
    {
        Destroy(transform.parent.gameObject);
    }
        
}
