using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using UnityEngine.UI;
using System;

public class IncubatorSceneScript : MonoBehaviour
{
    //string[,] allPlants;
    Dictionary<string, string[]> allPlants;
    DatabaseManagement db;
    GameObject databaseManager;
    int plantsNum;
    private float xPosition;
    private float yPosition;
    private float zPosition = 0.64f;
    private Vector3 plantPosition;
    string source;
    public Text DustText;

    private RuntimeAnimatorController[] greenAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] whiteAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] purpleAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] anim_start_inc = new RuntimeAnimatorController[4];

    public Image health;
    public Image water;
    //0 uuid  
    //1 place_id  
    //2 type  
    //3 stage 
    //4 happiness_level
    //5 water_level  
    //6 happiness_desc 
    //7 water_desc  
    //8 dust_give   
    //9 stage_timer

    // Start is called before the first frame update
    void Awake()
    {

        databaseManager = GameObject.FindGameObjectWithTag("DatabaseManager");
        db = databaseManager.GetComponent<DatabaseManagement>();
    }

    void Start()
    {
        greenAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().green;
        whiteAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().white;
        purpleAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().purple;
        DustText.GetComponent<Text>().text = PlayerPrefs.GetInt("dust", 150) + "";

        string dbPlantNum = db.NumberOfPlantsInIncubator();
        plantsNum = Int16.Parse(dbPlantNum);
        //allPlants = new string[plantsNum, 10];
        allPlants = db.plantsDic; //db.GetAllPlantsAndStats();
        createAllActivePlants(allPlants);
    }

    private void createAllActivePlants(Dictionary<string, string[]> allPlants) // string[,] allPlants)
    {
        //for (int i = 0; i < plantsNum; i++)
                /*
            0 uuid  
            1 place_id  
            2 type  
            3 stage 
            4 happiness_level  
            5 water_level  
            6 happiness_desc  
            7 water_desc  
            8 dust_give   
            9 stage_timer
            */
        int i = 0;
        foreach (string key in allPlants.Keys)
        {
            if (allPlants[key][2] == "Green")
            {
                print("tu sam");
                anim_start_inc = greenAnim;
            }
            if (allPlants[key][2] == "White")
            {
                anim_start_inc = whiteAnim;
            }
            if (allPlants[key][2] == "Purple")
            {
                anim_start_inc = purpleAnim;
            }

            source = "Plants_with_anim/" + allPlants[key][2];
            GameObject plant = Instantiate(Resources.Load(source + "/Models/" + allPlants[key][2] + allPlants[key][3]+"/" + allPlants[key][2] + allPlants[key][3] + "", typeof(GameObject))) as GameObject;
            plantPosition = SetPlantPosition(i + 1);
            Renderer rend = plant.GetComponentInChildren<Renderer>();
            rend.material = Resources.Load(source + "/Materials/" + allPlants[key][2] + allPlants[key][3] + "/" + allPlants[key][2] + allPlants[key][3] + " def") as Material;
            plant.transform.position = plantPosition;
            plant.transform.localScale += new Vector3(13, 13, 13);
            plant.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
            plant.name = allPlants[key][0];
            Animator anim = plant.GetComponentInChildren<Animator>();
            anim.runtimeAnimatorController = anim_start_inc[ (short.Parse(allPlants[key][3]) - 1) ];
            plant.AddComponent<UltimatePlantBehaviour>();
            plant.AddComponent<BoxCollider>();
            BoxCollider plantBox = plant.GetComponent<BoxCollider>();
            plantBox.size = new Vector3(0.03f, 0.03f, 0.03f);
            i++;
        }

    }

    public void ChangeStatsValues(float h, float w)
    {
        health.fillAmount = h;
        water.fillAmount = w;
    }

    private Vector3 SetPlantPosition(int plantNumber)
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

