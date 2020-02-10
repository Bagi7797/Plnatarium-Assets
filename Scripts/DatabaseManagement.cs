using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;

public class DatabaseManagement : MonoBehaviour
{
    private string connection;
    IDbCommand dbcmd;
    IDbConnection dbcon;
    IDataReader reader;
    string [] plantStats = new string [10];



    public Dictionary<string, string[]> plantsDic; // = new Dictionary<string, string[]>();
    public Dictionary<string, string[]> plantsPlanetDic; // = new Dictionary<string, string[]>();

    private void Awake()
    {
        connection = "URI=file:" + Application.persistentDataPath + "/" + "database";
        // Open connection
        dbcon = new SqliteConnection(connection);
        dbcon.Open();
        CreateDatabaseTable();
        GetAllPlantsAndStatsForPlanet();
        GetAllPlantsAndStats();

        //GetFirstFreeSpace();
    }

    private void CreateDatabaseTable()
    {
        
        dbcmd = dbcon.CreateCommand();
        /*
         0 uuid  1 place_id  2 type  3 stage 4 happiness_level  5 water_level  6 happiness_desc  7 water_desc  8 dust_give   9 stage_timer
         */
        string q_createTable = "CREATE TABLE IF NOT EXISTS plants ( uuid TEXT, place_id INTEGER NOT NULL, type TEXT, stage INTEGER, happiness_level INTEGER, water_level INTEGER, happiness_desc TEXT, water_desc TEXT, dust_give TEXT, stage_timer TEXT )";
        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();
    }


    public void InsertRow(string uuid, int place_id, string type, int stage, int happiness_level, int water_level, string time_of_creation)
    {
        IDbCommand cmnd = dbcon.CreateCommand();   
        cmnd.CommandText = "INSERT INTO plants (uuid, place_id, type, stage, happiness_level, water_level, happiness_desc, water_desc, dust_give, stage_timer) VALUES ('" + uuid + "', '" + place_id + "', '" + type + "', '" + stage + "', '" + happiness_level + "', '" + water_level + "', '" + time_of_creation + "', '" + time_of_creation + "', '" + time_of_creation + "', '" + time_of_creation + "')";
        cmnd.ExecuteNonQuery();
    }

    public void test()
    {
        print("test");
    }

    public string NumberOfPlantsInIncubator()
    {
        string connection_nop = "URI=file:" + Application.persistentDataPath + "/" + "database";
        IDbConnection dbcon_nop;
        dbcon_nop = new SqliteConnection(connection_nop);
        dbcon_nop.Open();
        string query = "SELECT COUNT(*) FROM plants WHERE place_id < 13";
        IDbCommand cmnd_read_nop = dbcon_nop.CreateCommand();
        cmnd_read_nop.CommandText = query;
        reader = cmnd_read_nop.ExecuteReader();
        var returnValue = reader[0];
        if (returnValue == null)
        {
            returnValue = 0;
        }
        dbcon_nop.Close();
        return returnValue.ToString();
    }

    public string NumberOfPlantsOutIncubator()
    {
        string connection_nop = "URI=file:" + Application.persistentDataPath + "/" + "database";
        IDbConnection dbcon_nop;
        dbcon_nop = new SqliteConnection(connection_nop);
        dbcon_nop.Open();
        string query = "SELECT COUNT(*) FROM plants WHERE place_id > 13";
        IDbCommand cmnd_read_nop = dbcon_nop.CreateCommand();
        cmnd_read_nop.CommandText = query;
        reader = cmnd_read_nop.ExecuteReader();
        var returnValue = reader[0];
        if (returnValue == null)
        {
            returnValue = 0;
        }
        dbcon_nop.Close();
        return returnValue.ToString();
    }


    public Dictionary<string, string[]> GetAllPlantsAndStatsForPlanet()
    {
        string connection_gapas = "URI=file:" + Application.persistentDataPath + "/" + "database";
        IDbConnection dbcon_gapas;
        dbcon_gapas = new SqliteConnection(connection_gapas);
        dbcon_gapas.Open();
        int br = 0;
        string[,] plants = new string[Int16.Parse(NumberOfPlantsOutIncubator()), 10];
        string query = "SELECT * FROM plants WHERE place_id > 13";
        IDbCommand cmnd_read_gapas = dbcon_gapas.CreateCommand();
        cmnd_read_gapas.CommandText = query;
        reader = cmnd_read_gapas.ExecuteReader();

        plantsPlanetDic = new Dictionary<string, string[]>();

        while (reader.Read())
        {
            /*
            plants[br, 0] = reader[0].ToString(); //0 uuid  
            plants[br, 1] = reader[1].ToString(); //1 place_id  
            plants[br, 2] = reader[2].ToString(); //2 type  
            plants[br, 3] = reader[3].ToString(); //3 stage 
            plants[br, 4] = reader[4].ToString(); //4 happiness_level
            plants[br, 5] = reader[5].ToString(); //5 water_level  
            plants[br, 6] = reader[6].ToString(); //6 happiness_desc 
            plants[br, 7] = reader[7].ToString(); //7 water_desc  
            plants[br, 8] = reader[8].ToString(); //8 dust_give   
            plants[br, 9] = reader[9].ToString(); //9 stage_timer
            */

            string[] plantData = new string[10];

            //print(reader[0].ToString());
            plantData[0] = reader[0].ToString(); //0 uuid  
            plantData[1] = reader[1].ToString(); //1 place_id  
            plantData[2] = reader[2].ToString(); //2 type  
            plantData[3] = reader[3].ToString(); //3 stage 
            plantData[4] = reader[4].ToString(); //4 happiness_level
            plantData[5] = reader[5].ToString(); //5 water_level  
            plantData[6] = reader[6].ToString(); //6 happiness_desc 
            plantData[7] = reader[7].ToString(); //7 water_desc  
            plantData[8] = reader[8].ToString(); //8 dust_give   
            plantData[9] = reader[9].ToString(); //9 stage_timer

            if (plantsPlanetDic.ContainsKey(plantData[0]))
                plantsPlanetDic[plantData[0]] = plantData;
            else
                plantsPlanetDic.Add(plantData[0], plantData);


            string output = "Key: " + plantData[0];
            for (int i = 0; i < 10; i++)
                output += "\n   " + plantsPlanetDic[plantData[0]][i];
            //print(output);

        }

        return plantsPlanetDic;
    }

    public Dictionary<string, string[]> GetAllPlantsAndStats()
    {
        string connection_gapas = "URI=file:" + Application.persistentDataPath + "/" + "database";
        IDbConnection dbcon_gapas;
        dbcon_gapas = new SqliteConnection(connection_gapas);
        dbcon_gapas.Open();
        int br = 0;
        string[,] plants = new string[Int16.Parse(NumberOfPlantsInIncubator()), 10];
        string query = "SELECT * FROM plants WHERE place_id < 13";
        IDbCommand cmnd_read_gapas = dbcon_gapas.CreateCommand();
        cmnd_read_gapas.CommandText = query;
        reader = cmnd_read_gapas.ExecuteReader();

        plantsDic = new Dictionary<string, string[]>();

        while (reader.Read())
        {
            /*
            plants[br, 0] = reader[0].ToString(); //0 uuid  
            plants[br, 1] = reader[1].ToString(); //1 place_id  
            plants[br, 2] = reader[2].ToString(); //2 type  
            plants[br, 3] = reader[3].ToString(); //3 stage 
            plants[br, 4] = reader[4].ToString(); //4 happiness_level
            plants[br, 5] = reader[5].ToString(); //5 water_level  
            plants[br, 6] = reader[6].ToString(); //6 happiness_desc 
            plants[br, 7] = reader[7].ToString(); //7 water_desc  
            plants[br, 8] = reader[8].ToString(); //8 dust_give   
            plants[br, 9] = reader[9].ToString(); //9 stage_timer
            */

            string[] plantData = new string[10];

            //print(reader[0].ToString());
            plantData[0] = reader[0].ToString(); //0 uuid  
            plantData[1] = reader[1].ToString(); //1 place_id  
            plantData[2] = reader[2].ToString(); //2 type  
            plantData[3] = reader[3].ToString(); //3 stage 
            plantData[4] = reader[4].ToString(); //4 happiness_level
            plantData[5] = reader[5].ToString(); //5 water_level  
            plantData[6] = reader[6].ToString(); //6 happiness_desc 
            plantData[7] = reader[7].ToString(); //7 water_desc  
            plantData[8] = reader[8].ToString(); //8 dust_give   
            plantData[9] = reader[9].ToString(); //9 stage_timer

            if (plantsDic.ContainsKey(plantData[0]))
                plantsDic[plantData[0]] = plantData;
            else
                plantsDic.Add(plantData[0], plantData);


            string output = "Key: " + plantData[0];
            for (int i = 0; i < 10; i++)
                output += "\n   " + plantsDic[plantData[0]][i];
            //print(output);

        }

       /* foreach (string key in plantsDic.Keys)
        {
            string output = "Key: " + key;
            for (int i = 0; i < 10; i++)
                output += "\n   " + plantsDic[key][i];

            print(output);

        }*/

        return plantsDic;
    }

    public string[] GetPlantStatsPlanet(string id)
    {
        return plantsPlanetDic[id];
    }

    public string[] GetPlantStats(string id)
    {
        return plantsDic[id];
    }

    public void ChangeStage(string uuid, string  stage)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET stage = '" + stage + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeHappinessLevel(string uuid, string level)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET happiness_level = '" + level + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeWaterLevel(string uuid, string level)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET water_level = '" + level + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeHappinessDate(string uuid, string date)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET happiness_desc = '" + date + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeWaterDate(string uuid, string date)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET water_desc = '" + date + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeDustDate(string uuid, string date)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET dust_give = '" + date + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangeStageDate(string uuid, string date)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET stage_timer = '" + date + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public void ChangePlace(string uuid, string pID)
    {
        IDbCommand cmnd_read = dbcon.CreateCommand();
        string query = "UPDATE plants SET place_id = '" + pID + "' WHERE uuid ='" + uuid + "'";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();
    }

    public string[] GetFirstFreeSpace()
    {
        int br = 0;
        string[] returnValue = new string[short.Parse(NumberOfPlantsInIncubator())];
        string connection_gffs = "URI=file:" + Application.persistentDataPath + "/" + "database";
        IDbConnection dbcon_gffs;
        dbcon_gffs = new SqliteConnection(connection_gffs);
        dbcon_gffs.Open();
        string query = "SELECT place_id FROM plants WHERE place_id < 13";
        IDbCommand cmnd_read_gffs = dbcon_gffs.CreateCommand();
        cmnd_read_gffs.CommandText = query;
        reader = cmnd_read_gffs.ExecuteReader();
        while (reader.Read())
        {
            returnValue[br] = reader[br].ToString(); 
            br++;
        }
        print(returnValue[1]);
        return returnValue;
    }

    private void OnDestroy()
    {
        //dbcon.Close();
    }
}
