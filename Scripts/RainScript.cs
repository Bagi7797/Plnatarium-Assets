using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainScript : MonoBehaviour
{
    private void Start()
    {
        particles.GetComponent<ParticleSystem>().Stop();
    }
    private bool isOn = false;
    public GameObject particles;
    public void StartRain()
    {
        if (isOn)
        {
            particles.GetComponent<ParticleSystem>().Stop();
            isOn = false;
        }
        else
        {
            particles.GetComponent<ParticleSystem>().Play();
            isOn = true;
        }
       
    }

}
