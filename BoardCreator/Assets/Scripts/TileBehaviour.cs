using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour {

    private GameObject tile;
    private int scaleY = 1;

	// Use this for initialization
	void Start () {
        tile = transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ScaleY(int newScale)
    {

    }
}
