using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class On_planet : MonoBehaviour
{
    private float duration = 0.6f;
    private float durationTime;
    private bool zoomIn = false;
    private bool inProces = false;
    [SerializeField]
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("dust", 5000);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if (Input.GetKeyUp("z")&inProces==false)
        {
            inProces = true;
            StartMovement(); 
        }
    }

    public void StartMovement()
    {
        // transform.localRotation = new Quaternion(0, 0, 0, 0);
        InvokeRepeating("MovementFunction", Time.fixedDeltaTime, Time.fixedDeltaTime); //Time.deltaTime is the time passed between two frames
        durationTime = Time.time + duration; //This is how long the invoke will repeat
    }

    private void MovementFunction()
    {
        if (durationTime > Time.time)
        {
            if (zoomIn == true)
            {
                camera.fieldOfView += 35 * Time.fixedDeltaTime; 
            }
            else
            {
                camera.fieldOfView -= 35 * Time.fixedDeltaTime;
            }
            
        }
        else
        {
            CancelInvoke("MovementFunction"); //Stop the invoking of this function
            if (zoomIn == true)
            {
                zoomIn = false;
            }
            else
            {
                zoomIn = true;
            }
            inProces = false;
            return;
        }
    }
}
