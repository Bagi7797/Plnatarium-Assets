using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_behaviour : MonoBehaviour
{
    bool cameraZoomIsOn = false;
    GameObject planetToZoomIn;
    [SerializeField]
    private Vector3 offset = new Vector3(-1, -0, -1);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraZoomIsOn == true)
            transform.position = (planetToZoomIn.transform.position + offset);
        
    }

    public void zoomIn(Vector3 newPosition, GameObject planet)
    {
        cameraZoomIsOn = true;
        transform.position = newPosition + offset;
        planetToZoomIn = planet;
        if (planetToZoomIn.transform.childCount > 0) 
        {
            print("nesto "+ planet.transform.Find("Planet")+ " inace" + planetToZoomIn.transform.position.x);
        }
    }

    public void zoomOut(Vector3 oldPosition)
    {
        cameraZoomIsOn = false;
        transform.position = oldPosition;
    }
}
