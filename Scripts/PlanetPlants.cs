using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlants : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<string, string[]> allPlants;
    string source;
    DatabaseManagement db;
    GameObject databaseManager;
    public GameObject planet;
    public SphereCollider Sphere;
    void Awake()
    {

        databaseManager = GameObject.FindGameObjectWithTag("DatabaseManager");
        db = databaseManager.GetComponent<DatabaseManagement>();
    }

    void Start()
    {
        allPlants = db.plantsPlanetDic;
        CreateAllActivePlants(allPlants);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateAllActivePlants(Dictionary<string, string[]> allPlants) // string[,] allPlants)
    {
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
         
            source = "Plants_with_anim/" + allPlants[key][2];
            GameObject plant = Instantiate(Resources.Load(source + "/Models/" + allPlants[key][2] + allPlants[key][3] + "/" + allPlants[key][2] + allPlants[key][3] + "", typeof(GameObject)),planet.transform) as GameObject;
            Renderer rend = plant.GetComponentInChildren<Renderer>();
            rend.material = Resources.Load(source + "/Materials/" + allPlants[key][2] + allPlants[key][3] + "/" + allPlants[key][2] + allPlants[key][3] + " def") as Material;
            plant.transform.position = new Vector3(0,0,0);
            plant.transform.localScale += new Vector3(9, 9, 9);
            plant.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
            plant.name = allPlants[key][0];
            plant.AddComponent<Walk_on_Planet>();
            plant.GetComponent<Walk_on_Planet>().planet = Sphere;
            plant.AddComponent<BoxCollider>();
            BoxCollider plantBox = plant.GetComponent<BoxCollider>();
            plantBox.size = new Vector3(0.03f, 0.03f, 0.03f);
            i++;
            if (i==21)
            {
                break;
            }
        }

    }
}
