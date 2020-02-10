using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;

    void Update()
    {
        /*if (Input.GetMouseButtonDown(0))
        {
            LoadNextScene();
        }
        if (Input.GetMouseButtonDown(1))
        {
            LoadPreviousScene();
        }*/
    }

    public void LoadNextScene()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void onFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
