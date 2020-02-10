using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotate : MonoBehaviour
{
    public Camera camera;
    public GameObject planet;
    bool doIt;
    Vector3 mPreviousPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    void Update()
    {
        doIt = planet.GetComponent<PlanetVars>().isGrapped;
        if (Input.GetMouseButton(0) & doIt == true)
        {
            mPosDelta = Input.mousePosition - mPreviousPos;

            transform.Rotate(transform.up, 0.2f*Vector3.Dot(mPosDelta, camera.transform.right), Space.World);

            
            transform.Rotate(camera.transform.right, 0.2f*Vector3.Dot(mPosDelta, camera.transform.up), Space.World);
        }

        mPreviousPos = Input.mousePosition;
    }
}
