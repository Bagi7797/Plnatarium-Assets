using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System.IO;
using UnityEngine.UI;
using System;

public class UltimatePlantBehaviour : MonoBehaviour
{
    private string[] plantStats = new string[10];
    DatabaseManagement db;
    GameObject databaseManager;
    GameObject incubatorManager;
    private IEnumerator coroutine;
    private PlantVars vars;
    private string source;
    private string happinessState;
    /// ///////////////////////////////////////////////////////////////////
    private int love_meter_max;
    private int love_down_per_minute;
    private int water_meter_max;
    private int water_down_per_minute;
    private int baby_time;
    private int teen_time;
    private int adult_time;
    private int elderly_time;
    private int bonus;
    int timeToChangeTheStage;
    private float xPosition;
    private float yPosition;
    private float zPosition = 0.64f;
    GameObject DustText;
    private int dust;
    /// //////////////////////////////////////////////////////////////////

   // GameObject statsGUI;
    //private bool statsGUIOn = false;
   // PlantStats pS;

    GameObject bunny;
    private RuntimeAnimatorController[] greenAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] whiteAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] purpleAnim = new RuntimeAnimatorController[4];
    private RuntimeAnimatorController[] anim_from_behaviour = new RuntimeAnimatorController[4];

    float timeNeededToSpawnDust;
    // Start is called before the first frame update

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
    private void Awake()
    {
        bunny = GameObject.FindGameObjectWithTag("dust");
        greenAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().green;
        whiteAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().purple;
        purpleAnim = GameObject.Find("Animators").GetComponent<AnimatorControllerVars>().white;
        DustText = GameObject.FindGameObjectWithTag("DustText");
        incubatorManager = GameObject.FindGameObjectWithTag("IncubatorManager");
        // statsGUI = GameObject.FindGameObjectWithTag("StatsWindow");
        //statsGUI.SetActive(false);

    }
    void Start()
    {

        if (plantStats[2] == "Green")
        {
            anim_from_behaviour = greenAnim;
        }
        if (plantStats[2] == "White")
        {
            anim_from_behaviour = whiteAnim;
        }
        if (plantStats[2] == "Purple")
        {
            anim_from_behaviour = purpleAnim;
        }

        //pS = statsGUI.gameObject.GetComponent<PlantStats>();
        databaseManager = GameObject.FindGameObjectWithTag("DatabaseManager");
        db = databaseManager.GetComponent<DatabaseManagement>();
        vars = GameObject.FindGameObjectWithTag("PlantVars").GetComponent<PlantVars>();

        plantStats = db.GetPlantStats(transform.name);
        source = "Plants_with_anim/" + plantStats[2];
        SetThisPlantVars(plantStats[2]);
        //print("dust_give: " + plantStats[8]+" for uuid"+transform.name);
        coroutine = Check(1.0f);
        StartCoroutine(coroutine);
    }

    void Update()
    {
        if (Int16.Parse(plantStats[4]) >= 66)
        {
            Renderer rend = transform.GetComponentInChildren<Renderer>();
            rend.material = Resources.Load(source + "/Materials/"+plantStats[2]+plantStats[3]+"/"+ plantStats[2] + plantStats[3]+" joy") as Material; //joy
            happinessState = "sh";
            /*Animator anim = transform.GetComponentInChildren<Animator>();
            anim.runtimeAnimatorController = anim_from_behaviour[short.Parse(plantStats[3]) - 1];*/
        }
        if (Int16.Parse(plantStats[4]) < 66 & Int16.Parse(plantStats[4]) >= 33)
        {
            Renderer rend = transform.GetComponentInChildren<Renderer>();
            rend.material = Resources.Load(source + "/Materials/" + plantStats[2] + plantStats[3] + "/" + plantStats[2] + plantStats[3] + " def") as Material;
            happinessState = "h";
        }
        if (Int16.Parse(plantStats[4]) < 33)
        {
            Renderer rend = transform.GetComponentInChildren<Renderer>();
            rend.material = Resources.Load(source + "/Materials/" + plantStats[2] + plantStats[3] + "/" + plantStats[2] + plantStats[3] + " sad") as Material;
            happinessState = "s";
        }
        if (Int16.Parse(plantStats[4]) < 1)
        {
            plantStats[4] = "0";
            KillMePlease();
        }


    }

    private IEnumerator Check(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //Code part where we check every second is enough time passed for us to give the player dust or reduce water or reduce happiness or change the look and/or place of plant
            CheckHappinessReduce();
            CheckWaterReduce();
            CheckStage();
            SpawnDustIfPossible();
        }
    }
    /*
   0 uuid  
   1 place_id  1-12 100 101 200
   2 type  
   3 stage 
   4 happiness_level  
   5 water_level  
   6 happiness_desc  
   7 water_desc  
   8 dust_give   
   9 stage_timer
   */
    private void CheckHappinessReduce()
    {
        //happens every minute for every plant
        var start = DateTime.Now;
        var oldDate = DateTime.Parse(plantStats[6]);
        if ((start - oldDate).TotalMinutes >= 1)
        {
            plantStats[4] = (Int16.Parse(plantStats[4]) - love_down_per_minute).ToString();
            plantStats[6] = DateTime.Now.ToString();
            //database
            
            db.ChangeHappinessLevel(transform.name, plantStats[4]);
            db.ChangeHappinessDate(transform.name, plantStats[6]);
        }
    }

    private void CheckWaterReduce()
    {
        //happens every minute for every plant
        var start = DateTime.Now;
        var oldDate = DateTime.Parse(plantStats[7]);
        if ((start - oldDate).TotalMinutes >= 1)
        {
            plantStats[5] = (Int16.Parse(plantStats[5]) - water_down_per_minute).ToString();
            plantStats[7] = DateTime.Now.ToString();
            //database
            db.ChangeWaterLevel(transform.name, plantStats[5]);
            db.ChangeWaterDate(transform.name, plantStats[7]);
        }
    }

    private void CheckStage()
    {
        
        var start = DateTime.Now;
        var oldDate = DateTime.Parse(plantStats[9]);
        if (plantStats[3] == "1") //baby
        {
            timeToChangeTheStage = baby_time;
            //print(timeToChangeTheStage);
        }
        if (plantStats[3] == "2") //teen
        {
            timeToChangeTheStage = teen_time;
        }
        if (plantStats[3] == "3") //adult
        {
            timeToChangeTheStage = adult_time;
        }
        if (plantStats[3] == "4") //elderly
        {
            timeToChangeTheStage = elderly_time;
        }
        if ((start - oldDate).TotalMinutes >= timeToChangeTheStage)
        {
            plantStats[3] = (Int16.Parse(plantStats[3]) + 1).ToString();
            plantStats[9] = DateTime.Now.ToString();
            //change texture-- get ze plant out-- change database;

            
            db.ChangeStageDate(transform.name, plantStats[9]);
            if (short.Parse(plantStats[3])<=2)
            {
                db.ChangeStage(transform.name, plantStats[3]);
                EvolveThePlant();
            }
            else if (short.Parse(plantStats[3]) > 2)
            {
                db.ChangeStage(transform.name, plantStats[3]);
                db.ChangePlace(transform.name, "100");
                GetPlantOutOfIncubator();
            }
            
        }
    }

    private void SpawnDustIfPossible()
    {
        var start = DateTime.Now;
        var oldDate = DateTime.Parse(plantStats[8]);
        if (happinessState == "sh")
        {
            timeNeededToSpawnDust = 10f;
        }
        if (happinessState=="h")
        {
            timeNeededToSpawnDust = 30f;
        }
        if (happinessState == "s")
        {
            timeNeededToSpawnDust = 60f;
        }
        if ((start - oldDate).TotalMinutes >= timeNeededToSpawnDust)
        {
            dust = PlayerPrefs.GetInt("dust", 0);
            dust++;//= Mathf.Abs((start - oldDate).TotalMinutes);
            plantStats[8] = DateTime.Now.ToString();
            db.ChangeDustDate(transform.name, plantStats[8]);
            
            PlayerPrefs.SetInt("dust", dust);
            PlayerPrefs.Save();
            DustText.GetComponent<Text>().text = dust.ToString();
            SpawnDust();
        }
    }

    private void SpawnDust()
    {
        GameObject g = Instantiate(bunny, this.transform.position, Quaternion.identity);
        g.transform.position = this.transform.position;
    }

    private void OnMouseDrag()
    {

        if (Int16.Parse(plantStats[4]) < love_meter_max)
        {
            plantStats[4] = (Int16.Parse(plantStats[4]) + 1).ToString();
            db.ChangeHappinessLevel(transform.name, plantStats[4]);
        }

        if (Int16.Parse(plantStats[5]) < water_meter_max)
        {
            plantStats[5] = (Int16.Parse(plantStats[5]) + 1).ToString();
            db.ChangeWaterLevel(transform.name, plantStats[5]);
        }


        incubatorManager.GetComponent<IncubatorSceneScript>().ChangeStatsValues(float.Parse(plantStats[4]) / love_meter_max, float.Parse(plantStats[5]) / water_meter_max);
    }

   /* private void OnMouseDown()
    {
        statsGUI.SetActive(true);
        statsGUIOn = true;
    }*/

    private void KillMePlease()
    {
        Destroy(this.gameObject);
    }

    private void GetPlantOutOfIncubator()
    {
        Destroy(this.gameObject);
    }
 
    private void EvolveThePlant()
    {
        GameObject plant = Instantiate(Resources.Load(source + "/Models/" + plantStats[2] + plantStats[3] + "/" + plantStats[2] + plantStats[3] + "", typeof(GameObject))) as GameObject; 
        plant.AddComponent<BoxCollider>();
        plant.transform.position = SetPlantPosition(short.Parse(plantStats[1]));
        plant.transform.localScale += new Vector3(13, 13, 13);
        plant.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
        plant.name =this.transform.name;
        Animator anim = plant.GetComponentInChildren<Animator>();
        anim.runtimeAnimatorController = anim_from_behaviour[short.Parse(plantStats[3])-1];
        plant.AddComponent<UltimatePlantBehaviour>();
        BoxCollider plantBox = plant.GetComponent<BoxCollider>();
        plantBox.size = new Vector3(0.03f, 0.03f, 0.03f);
        Destroy(this.gameObject);

    }

    private void SetThisPlantVars(string plantType)
    {
        if (plantType == "Green")
        {
            love_meter_max = vars.green_love_meter_max;
            love_down_per_minute = vars.green_love_down_per_minute;
            water_meter_max = vars.green_water_meter_max;
            water_down_per_minute = vars.green_water_down_per_minute;
            baby_time = vars.green_baby_time;
            teen_time = vars.green_teen_time;
            adult_time = vars.green_adult_time;
            elderly_time = vars.green_elderly_time;
            bonus = vars.green_bonus;
        }
        if (plantType == "White")
        {
            love_meter_max = vars.white_love_meter_max;
            love_down_per_minute = vars.white_love_down_per_minute;
            water_meter_max = vars.white_water_meter_max;
            water_down_per_minute = vars.white_water_down_per_minute;
            baby_time = vars.white_baby_time;
            teen_time = vars.white_teen_time;
            adult_time = vars.white_adult_time;
            elderly_time = vars.white_elderly_time;
            bonus = vars.white_bonus;
        }
        if (plantType == "Purple")
        {
            love_meter_max = vars.purple_love_meter_max;
            love_down_per_minute = vars.purple_love_down_per_minute;
            water_meter_max = vars.purple_water_meter_max;
            water_down_per_minute = vars.purple_water_down_per_minute;
            baby_time = vars.purple_baby_time;
            teen_time = vars.purple_teen_time;
            adult_time = vars.purple_adult_time;
            elderly_time = vars.purple_elderly_time;
            bonus = vars.purple_bonus;
        }
    }

    private Vector3 SetPlantPosition(int plantNumber)
    {
        if (plantNumber <= 4)
        {
            yPosition = 0.5f;
        }
        if (plantNumber > 4 & plantNumber <= 8)
        {
            yPosition = -0.54f;
        }
        if (plantNumber > 8)
        {
            yPosition = -1.66f;
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
