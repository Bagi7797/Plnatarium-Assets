using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class SqliteTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        // Create database
        string connection = "URI=file:" + Application.persistentDataPath + "/" + "database";

        // Open connection
        IDbConnection dbcon = new SqliteConnection(connection);
        dbcon.Open();

        // Create table
        IDbCommand dbcmd;
        dbcmd = dbcon.CreateCommand();
        string q_createTable = "CREATE TABLE IF NOT EXISTS plants ( id INTEGER NOT NULL, type TEXT, stage INTEGER, happiness INTEGER, stage_timer TEXT, happiness_timer TEXT )";

        dbcmd.CommandText = q_createTable;
        dbcmd.ExecuteReader();

        /* Insert values in table
        IDbCommand cmnd = dbcon.CreateCommand();
        cmnd.CommandText = "INSERT INTO plants (id, type, stage, happiness,  stage_timer, ) VALUES (0, 'green', 1.60, 'neko vrijeme', 'neko vrijeme')";
        cmnd.ExecuteNonQuery();*/

        // Read and print all values in table
        IDbCommand cmnd_read = dbcon.CreateCommand();
        IDataReader reader;
        string query = "SELECT * FROM plants";
        cmnd_read.CommandText = query;
        reader = cmnd_read.ExecuteReader();

        while (reader.Read())
        {
            Debug.Log("id: " + reader[0].ToString());
            Debug.Log("type: " + reader[1].ToString());
            Debug.Log("stage: " + reader[2].ToString());
            Debug.Log("happiness: " + reader[3].ToString());
            Debug.Log("timer: " + reader[4].ToString());
        }

        // Close connection
        dbcon.Close();

    }

    // Update is called once per frame
    void Update()
    {

    }
}