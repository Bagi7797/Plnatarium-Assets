using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class SampleButton : MonoBehaviour
{
    public GameObject shopMenu;
    private GameObject DustText;
    private string source;
    private int plantsNum;
    private float xPosition;
    private float yPosition;
    private float zPosition = 0.64f;
    private Vector3 plantPosition;
    private string plantType;
    private int dust;
    private bool canBuy = true;

    private RuntimeAnimatorController[] greenAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] whiteAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] purpleAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController anim_from_shop;
    private string uniqueID;

    GameObject databaseManager;
    DatabaseManagement db;

    private void Start()
    {
        db = GameObject.FindGameObjectWithTag("DatabaseManager").GetComponent<DatabaseManagement>();
        greenAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().green;
        whiteAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().purple;
        purpleAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().white;
        DustText = GameObject.FindGameObjectWithTag("DustText");
    }

    public void CreatePlant()
    {
       
        plantsNum = Int16.Parse(db.NumberOfPlantsInIncubator());
        dust = PlayerPrefs.GetInt("Dust", 5000);
        dust = 5000;
        if (plantsNum<12)
        {
            //print("kliknuo sam");
            if (transform.tag == "green")
            {
                if (dust < 50)
                {
                    canBuy = false;
                }
                else
                {
                    canBuy = true;
                    dust = dust - 50;
                    PlayerPrefs.SetInt("Dust", dust);
                    PlayerPrefs.Save();
                    DustText.GetComponent<Text>().text = dust+"";
                }
                source = "Plants_with_anim/Green";
                plantType = "Green";
                anim_from_shop = greenAnim[0];
            }
            else if (transform.tag == "purple")
            {
                if (dust < 150)
                {
                    canBuy = false;
                }
                else
                {
                    canBuy = true;
                    dust = dust - 150;
                    PlayerPrefs.SetInt("Dust", dust);
                    PlayerPrefs.Save();
                    DustText.GetComponent<Text>().text = dust.ToString();
                }
                source = "Plants_with_anim/Purple";
                plantType = "Purple";
                anim_from_shop = purpleAnim[0];
            }
            else
            {
                if (dust < 300)
                {
                    canBuy = false;
                }
                else
                {
                    canBuy = true;
                    dust = dust - 300;
                    PlayerPrefs.SetInt("Dust", dust);
                    PlayerPrefs.Save();
                    DustText.GetComponent<Text>().text = dust.ToString();
                }
                source = "Plants_with_anim/White";
                plantType = "White";
                anim_from_shop = whiteAnim[0];
            }
            if (canBuy)
            {
                GameObject plant = Instantiate(Resources.Load(source + "/Models/"+plantType+"1/"+plantType+"1", typeof(GameObject))) as GameObject;
                plantsNum++;


                plantPosition = SetPlantPosition(plantsNum);
                uniqueID = Guid.NewGuid().ToString();
                db.InsertRow(uniqueID, plantsNum, plantType, 1, 80, 80, DateTime.Now.ToString());
                db.GetAllPlantsAndStats();
                Renderer rend = plant.GetComponentInChildren<Renderer>();
                rend.material = Resources.Load(source + "/Materials/" + plantType + "1/" + plantType + "1 def") as Material;
                plant.transform.position = plantPosition;
                plant.transform.localScale += new Vector3(13, 13, 13);
                plant.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
                plant.name = uniqueID;
                Animator anim = plant.GetComponentInChildren<Animator>();
                anim.runtimeAnimatorController = anim_from_shop;
                plant.AddComponent<UltimatePlantBehaviour>();
                plant.AddComponent<BoxCollider>();
                BoxCollider plantBox = plant.GetComponent<BoxCollider>();
                plantBox.size = new Vector3(0.03f, 0.03f, 0.03f);
                shopMenu.SetActive(false);
            }
            else
            {
                Debug.Log("You need more stardust");
            }
            
        }
        else
        {
            Debug.Log("first you need to raise your plants before you can buy more :)");
        }
    }


    private Vector3 SetPlantPosition( int plantNumber)
    {
        if (plantNumber <= 4)
        {
            yPosition = 0.56f;
        }
        if (plantNumber > 4 & plantNumber <= 8)
        {
            yPosition = -0.48f;
        }
        if (plantNumber > 8)
        {
            yPosition = -1.60f;
        }

        if (plantNumber == 1 | plantNumber == 5 | plantNumber == 9)
        {
            xPosition = -2f;
        }
        if (plantNumber == 2 | plantNumber == 6 | plantNumber == 10)
        {
            xPosition = -0.9f;
        }
        if (plantNumber == 3 | plantNumber == 7 | plantNumber == 11)
        {
            xPosition = 1f;
        }
        if (plantNumber == 4 | plantNumber == 8 | plantNumber == 12)
        {
            xPosition = 2f;
        }

        return new Vector3(xPosition, yPosition, zPosition);
    }
}
