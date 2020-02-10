using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyBehaviour : MonoBehaviour
{
    private float duration = 3f;
    private float durationTime;
    // Start is called before the first frame update
    void Start()
    {
        StartMovement();
    }
    public void StartMovement()
    {
        // transform.localRotation = new Quaternion(0, 0, 0, 0);
        InvokeRepeating("MovementFunction", Time.deltaTime, Time.deltaTime); //Time.deltaTime is the time passed between two frames
        durationTime = Time.time + duration; //This is how long the invoke will repeat
    }

    private void MovementFunction()
    {
        if (durationTime > Time.time)
        {
            transform.Translate(new Vector3(0, 2f, 0) * Time.deltaTime);
        }
        else
        {
            if (this.gameObject.name != "bunny")
            {
                Destroy(this.gameObject);
            }
            
            CancelInvoke("MovementFunction"); //Stop the invoking of this function
           
            return;
        }
    }

}
