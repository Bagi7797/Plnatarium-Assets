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

public class PlantBehaviour : MonoBehaviour
{
    private int plantID;
    SceneStart sceneStart;
    GameObject DustText;
    private string[] plantStats;
    private IEnumerator coroutine;
    private int dust;
    private string source;
    IDbConnection dbcon;
    public int timeToCollectHappy = 10;
    public int timeToCollectNeutral = 30;
    public int timeToCollectSad = 60;
    private int collectTime;
    public int timeToReduceHappiness = 1;
    public int overallHappiness;
    // Start is called before the first frame update
    void Start()
    {

        string connection = "URI=file:" + Application.persistentDataPath + "/" + "database";
        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();

        DustText =  GameObject.FindGameObjectWithTag("DustText");
        sceneStart = GameObject.Find("IncubatorManager").GetComponent<SceneStart>();
        plantStats = new string[7];
        plantID = Int16.Parse(transform.name);
        for (int i = 0; i < 7; i++)
        {
            plantStats[i] = sceneStart.plants[plantID-1, i];
        }
        print(plantStats[1] + ", id:"+ plantID);

        if (plantStats[1] == "green")
        {
            source = "Plants/Green_1";
        }
        else if (plantStats[1] == "purple")
        {
            source = "Plants/Purple_1";
        }
        else
        {
            source = "Plants/White_1";
        }

        coroutine = HappyCheck(10.0f);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (Int16.Parse(plantStats[3]) >= 66)
        {
            Renderer rend = transform.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load(source + "/Textures/Joy emotion/joy") as Texture;
        }
        if (Int16.Parse(plantStats[3]) < 66 & Int16.Parse(plantStats[3]) >= 33)
        {
            Renderer rend = transform.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load(source + "/Textures/albedo") as Texture;
        }
        else
        {
            Renderer rend = transform.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load(source + "/Textures/Sad emotion/sad") as Texture;
        }
    }
     //0id
     //1type
     //2stage
     //3happiness
     //4stage_timer
     //5happiness_timer
     //6 happy_reduce

    private IEnumerator HappyCheck(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            //print("after 10 sec " +plantStats[4]);
            //green -- 10
            //white -- 30
            //check happiness status
            if (Int16.Parse(plantStats[3])>=66)
            {
                collectTime = timeToCollectHappy;
            }
            if (Int16.Parse(plantStats[3]) < 66 & Int16.Parse(plantStats[3]) >= 33)
            {
                collectTime = timeToCollectNeutral;
            }
            else
            {
                collectTime = timeToCollectSad;
            }


            dust = PlayerPrefs.GetInt("Dust", 0);
            var start = DateTime.Now;
            var oldDate = DateTime.Parse(plantStats[4]);
            //add dust from regular plant behaviour
            if ((start - oldDate).TotalMinutes >= collectTime)
            {
                PlayerPrefs.SetInt("Dust", dust + 10);
                DustText.GetComponent<Text>().text = "Happiness dust: " + dust;
                plantStats[4] = DateTime.Now.ToString();
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                string query = "UPDATE plants SET happiness_timer = '" + plantStats[4]+"' WHERE id ='" + plantStats[0]+"'";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();   
            }

            var startH = DateTime.Now;
            var oldDateH = DateTime.Parse(plantStats[6]);
            if ((startH - oldDateH).TotalMinutes >= timeToReduceHappiness)
            {
                IDbCommand cmnd_read = dbcon.CreateCommand();
                IDataReader reader;
                plantStats[6] = DateTime.Now.ToString();
                plantStats[3] = (Int16.Parse(plantStats[3]) - 1).ToString();
                string query = "UPDATE plants SET happy_reduce = '" + plantStats[6] + "',happiness = '" + plantStats[3] + "'     WHERE id ='" + plantStats[0] + "'";
                cmnd_read.CommandText = query;
                reader = cmnd_read.ExecuteReader();
               
            }
        }
    }

    private void OnMouseUp()
    {

        plantStats[3] = (Int16.Parse(plantStats[3]) + 1).ToString();
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "UPDATE plants SET happiness = '" + plantStats[3] + "'     WHERE id ='" + plantStats[0] + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    private void OnDestroy()
    {
        dbcon.Close();
    }
}
