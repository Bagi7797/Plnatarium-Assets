using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class SceneStart : MonoBehaviour
{
    [SerializeField]
    GameObject DustText ;
    private string source;
    private int plantsNum;
    private float xPosition;
    private float yPosition;
    private float zPosition = 0.64f;
    private Vector3 plantPosition;
    private string plantType;
    IDbConnection dbcon;
    private int br;
    public string[,] plants;

    // Start is called before the first frame update
    private void Start()
    {
        int dust = PlayerPrefs.GetInt("Dust", 0);
        DustText.GetComponent<Text>().text = "Happiness dust: " + dust;
        br = 0;
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "database";
        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        plantsNum = Int16.Parse(GetNumberOfPlants());
        plants = new string[plantsNum, 7];
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM plants";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            plants[br, 0] = reader[0].ToString(); //id
            plants[br, 1] = reader[1].ToString(); //type
            plants[br, 2] = reader[2].ToString(); //stage
            plants[br, 3] = reader[3].ToString(); //happiness
            plants[br, 4] = reader[4].ToString(); //stage_timer
            plants[br, 5] = reader[5].ToString(); //happiness_timer
            plants[br, 6] = reader[6].ToString(); //happy_reduce
            br++;
        }
        //createAllActivePlants(plants);
        dbcon.Close();
    }


    //not used for now
    public void updateNumberOfPlants()
    {
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "database";
        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        plantsNum = Int16.Parse(GetNumberOfPlants());
        plants = null;
        print("updejtam:," + plantsNum);
        plants = new string[plantsNum, 7];
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM plants";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            plants[br, 0] = reader[0].ToString(); //id
            plants[br, 1] = reader[1].ToString(); //type
            plants[br, 2] = reader[2].ToString(); //stage
            plants[br, 3] = reader[3].ToString(); //happiness
            plants[br, 4] = reader[4].ToString(); //stage_timer
            plants[br, 5] = reader[5].ToString(); //happiness_timer
            plants[br, 6] = reader[6].ToString(); //happy_reduce
            br++;
        }
        dbcon.Close();
    }

    private void createAllActivePlants(string[,] allPlants)
    {
        for (int i = 0; i < plantsNum; i++)
        {
            if (allPlants[i, 1] == "green")
            {
                source = "Plants/Green_1";
                plantType = "green";
            }
            else if (allPlants[i, 1] == "purple")
            {
                source = "Plants/Purple_1";
                plantType = "purple";
            }
            else
            {
                source = "Plants/White_1";
                plantType = "white";
            }
            GameObject plant = Instantiate(Resources.Load(source + "/Model", typeof(GameObject))) as GameObject;
            plantPosition = SetPlantPosition(i + 1);
            Renderer rend = plant.GetComponent<Renderer>();
            rend.material.mainTexture = Resources.Load(source + "/Textures/albedo") as Texture;
            plant.AddComponent<PlantBehaviour>();
            plant.AddComponent<BoxCollider>();
            plant.transform.position = plantPosition;
            plant.transform.localScale += new Vector3(5, 5, 5);
            plant.transform.rotation = Quaternion.Euler(270f, 0f, 180f);
            plant.name = (i + 1).ToString();
        }

    }

    private string GetNumberOfPlants()
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT COUNT(*) FROM plants";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
        //print(reader[0]);
        var returnValue = reader[0];
        return returnValue.ToString();

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

