using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotsShop : MonoBehaviour
{
    [SerializeField]
    private GameObject shop;
    private void Start()
    {
        print("script is active");
    }
    private void OnMouseUp()
    {
        print("set ActiveSHop");
        shop.SetActive(true);
    }

    public void CollapseShop()
    {
        shop.SetActive(false);
    }

}
