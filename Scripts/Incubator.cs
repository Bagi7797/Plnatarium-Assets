using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Incubator : MonoBehaviour
{
    // Start is called before the first frame update
    On_planet _incubator;
    [SerializeField]
    GameObject Manager;
    void Start()
    {
        _incubator = Manager.GetComponent<On_planet>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*private void OnMouseUp()
    {
        print("lala");
        _incubator.ZoomInIncubator();
    }*/
}
