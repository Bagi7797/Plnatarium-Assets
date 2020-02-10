using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterTheIncubator : MonoBehaviour
{
    GameObject SceneManager;
    private void Start()
    {
        SceneManager = GameObject.FindGameObjectWithTag("SceneManager");
    }
    private void OnMouseUp()
    {
        SceneManager.GetComponent<SceneChanger>().LoadNextScene();
    }
}
