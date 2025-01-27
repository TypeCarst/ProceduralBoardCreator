using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBehaviour : MonoBehaviour
{
    private float scaleY = 1.0f;

    public void SetHeight(float newHeight)
    {
        scaleY = newHeight;
        transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
    }
}
