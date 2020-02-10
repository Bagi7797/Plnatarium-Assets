using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk_on_Planet : MonoBehaviour
{
    public float rotationSpeed = 120.0f;
    public float translationSpeed = 10.0f;
    public float height = 0.1f;            
    private Transform centre;               
    private float radius;                   
    public SphereCollider planet;          

    void Start()
    {
        radius = planet.radius * planet.transform.localScale.y;
        centre = planet.transform;
        //starting position at north pole
        transform.position = centre.position + new Vector3(0, radius + height, 0);
    }

    void Update()
    {   
        float inputMag = Random.Range(0.1f,0.2f) * translationSpeed * Time.deltaTime;
        transform.position += transform.forward * inputMag;

        //radius + height
        Vector3 targetPosition = transform.position - centre.position;
        float ratio = (radius + height) / targetPosition.magnitude;
        targetPosition.Scale(new Vector3(ratio, ratio, ratio));
        transform.position = targetPosition + centre.position;

        //surface normal                      
        Vector3 surfaceNormal = transform.position - centre.position;
        surfaceNormal.Normalize();

        //heading
        float headingDeltaAngle = Random.Range(-0.6f, 0.6f) * Time.deltaTime * rotationSpeed;
        Quaternion headingDelta = Quaternion.AngleAxis(headingDeltaAngle, transform.up);

        transform.rotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;
        transform.rotation = headingDelta * transform.rotation;
    }
}
