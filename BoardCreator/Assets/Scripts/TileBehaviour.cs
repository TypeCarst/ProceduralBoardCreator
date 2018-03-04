using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private float scaleY = 1.0f;

    public void ScaleY(float newScale)
    {
        scaleY = newScale;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }
}
