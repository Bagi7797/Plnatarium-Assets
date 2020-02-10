using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    Camera_behaviour _camera;
    GameObject Camera;
    bool cameraZoom = false;
    [SerializeField]
    Game_Manager _gameManager;

    private void Start()
    {
        Camera = GameObject.Find("Main Camera");
        _camera = Camera.GetComponent<Camera_behaviour>();
        PlayerPrefs.SetInt("dust", 5000);
    }
    

   // treba napraviti to da se poziv u cameri ne u planetu

    // Update is called once per frame
    void Update()
    {

        //Camera.transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 30 * Time.deltaTime);
        if (gameObject.name == "Planet")
        {
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0,1), 30 * Time.deltaTime);
        }
        else
        {
            transform.RotateAround(new Vector3(0, 0, 0), new Vector3(0, 0, 1)*0.033f, 30 * Time.deltaTime);
        }
        

    }

    void OnMouseUp()
    {
        if (this.gameObject.name == "Planet")
        {
            // zoom in on this planet 
            if (cameraZoom == false)
            {
                cameraZoom = true;
                _camera.zoomIn(this.transform.position, this.gameObject);
            }
            else
            {
                cameraZoom = false;
                _camera.zoomOut(new Vector3(0, 0, -10f));
            }
            _gameManager.TurnOnPlanetDescription("fsd");

        }
    }
}
