using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantStats : MonoBehaviour
{
    // Start is called before the first frame update
    public Image healthBar;
    public Image waterBar;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeValues(float health, float water)
    {
        healthBar.fillAmount = health;
        waterBar.fillAmount = water;
    }

}
