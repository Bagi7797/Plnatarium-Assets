using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject _planet_desc_menu;
    void Start()
    {
        
    }


    public void TurnOnPlanetDescription(string planetDesc)
    {
        if (_planet_desc_menu.active == true)
        {
            _planet_desc_menu.SetActive(false);
        }
        else
        {
            _planet_desc_menu.SetActive(true);
        }
    }
}
