using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetVars : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isGrapped = false;

    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            isGrapped = true;
        }
        
    }

    private void OnMouseUp()
    {
        isGrapped = false;
    }
}
