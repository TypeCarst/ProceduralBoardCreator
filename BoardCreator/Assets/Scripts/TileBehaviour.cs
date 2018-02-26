using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    private GameObject tile;
    private float scaleY = 1.0f;

	// Use this for initialization
	void Awake () {
        tile = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ScaleY(float newScale)
    {
        scaleY = newScale;
        tile.transform.localScale = new Vector3(tile.transform.localScale.x, scaleY, tile.transform.localScale.z);
    }
}
