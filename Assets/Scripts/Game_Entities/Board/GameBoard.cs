using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameBoard : MonoBehaviour
{

    // NumberTiles must be % 4 == 0 ;
    public const int NumberTiles_Rat_Race = 18;
    public const int NumberTiles_Fat_Race = 32;
    public static GameBoard Instance;
    // Avatar Tile -- Prefabs;
    [SerializeField] private GameObject Tile_Rat_Race;
    [SerializeField] private GameObject Tile_Fat_Race;
    [SerializeField] public Tile_Material tile_avatar;
    [SerializeField] public List<Dream> dreams;
    // Size of Tile
    public float size = 4f;
    // Radius of Rat Race
    public float radius = 10f;
    // List Tile
    public List<GameObject> Tiles_Rat_Race;
    public List<GameObject> Tiles_Fat_Race;
    public GameObject Root;



    // Excecute when Loading Scence
    [Obsolete]
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        Instance = this;
        SpawnTiles();
    }


    private void Start()
    {
        
    }

    // Spawn Tile with number has been create
    [Obsolete]
    private void SpawnTiles()
    {
        Transform rootPos = Root.GetComponent<Transform>();
        rootPos.position = new Vector3(0, 0, 0);
        // Spawn Rat Race with Circle
        Spawn_Rat_Race(NumberTiles_Rat_Race);
        // Spawn Fat Race with Square
        Spawn_Fat_Race(NumberTiles_Fat_Race);
        Load_Dream();
    }

    public void Set_Tiles_Type()
    {
        StartCoroutine(EvenCard_Data.instance.helper.Get("tiles/all", (request,process) =>
        {
            //Debug.Log(request.downloadHandler.text);
            List<Tile_Entity> tiles = EvenCard_Data.instance.helper.ParseToList<Tile_Entity>(request);
            //Debug.Log(string.Format("Downloaded Tiles Process {0:P1}", process * 100f + "%"));
            foreach (Tile_Entity tile in tiles)
            {
                switch (tile.race_type)
                {
                    case "FAT":
                        Set_Fat_Race_Tile_Event(tile);
                        break;
                    default:
                        Set_Rat_Race_Tile_Event(tile);
                        break;
                }
            }
        }));
        
    }

    [Obsolete]
    private void Spawn_Rat_Race(int NumberTiles_Rat_Race)
    {
        Tiles_Rat_Race = new List<GameObject>();
        float nextAngle = 2 * Mathf.PI / NumberTiles_Rat_Race;
        float angle = 0;// start pos
        for (int i = 0; i < NumberTiles_Rat_Race; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            GameObject tile = Instantiate(Tile_Rat_Race, new Vector3(-x*size, 0f, z*size), Quaternion.identity);
            tile.transform.rotation = Quaternion.EulerAngles(0f, angle, 0f);
            tile.transform.parent = this.gameObject.transform;
            Tiles_Rat_Race.Add(tile);
            angle += nextAngle;
        }
    }   

    public void Load_Dream()
    {
        StartCoroutine(EvenCard_Data.instance.helper.Get("dreams/all", (request, process) =>
        {
            //Debug.Log(request.downloadHandler.text);
            if (dreams == null)
            {
                dreams = new List<Dream>();
            }
            dreams = EvenCard_Data.instance.helper.ParseToList<Dream>(request);
            Debug.Log(string.Format("Downloaded Tiles Process {0:P1}", process * 100f + "%"));

            // Set Event Type to All Race , TMP is RAT RACE
            Set_Tiles_Type();
        }));
    }

    private void Set_Rat_Race_Tile_Event(Tile_Entity tile)
    {
        TileType type;
        switch (tile.tile_type)
        {
            case "PayCheck":
                type = TileType.PayCheck;
                break;
            case "Get A Child":
                type = TileType.Baby;
                break;
            case "DownSize":
                type = TileType.DownSize;
                break;
            case "The Market":
                type = TileType.Market;
                break;
            case "Charity":
                type = TileType.Charity;
                break;
            case "Doodad":
                type = TileType.Doodads;
                break;
            default:
                type = TileType.Oppotunity;
                break;
        }
        foreach(int i in tile.positions)
        {
            //Test Event
            //Tiles_Rat_Race[i].GetComponent<Tile>().Type = TileType.Doodads;
            //Tiles_Rat_Race[i].GetComponent<Tile>().SetMaterialTile(TileType.Doodads, tile_avatar);
            // Main
            
            Tiles_Rat_Race[i].GetComponent<Tile>().Type = type;
            Tiles_Rat_Race[i].GetComponent<Tile>().SetMaterialTile(type, tile_avatar);
        }
    }
    private void Set_Fat_Race_Tile_Event(Tile_Entity tile)
    {
        TileType type;
        switch (tile.tile_type)
        {
            case "CashFlowDay":
                type = TileType.CashFlowDay;
                break;
            case "Divorce":
                type = TileType.Divorce;
                break;
            case "Dream":
                type = TileType.Dream;
                break;
            case "Charity":
                type = TileType.Charity;
                break;
            case "The Market":
                type = TileType.Market;
                break;
            case "accused":
                type = TileType.Accused;
                break;
            default:
                type = TileType.Oppotunity;
                break;
        }
        List<Dream> dream = this.dreams;
        int dream_amount = dreams.Count;
        foreach (int i in tile.positions)
        {
            Tiles_Fat_Race[i].GetComponent<Tile>().Type = type;
            if(type == TileType.Dream)
            {
                Tiles_Fat_Race[i].GetComponent<Tile>().SetDreamTile(dream[dream.Count - 1]);
                dream_amount--;
            }
            Tiles_Fat_Race[i].GetComponent<Tile>().SetMaterialTile(type,tile_avatar);
        }
    }

    private void Spawn_Fat_Race(int NumberTiles_Fat_Race)
    {
        Tiles_Fat_Race = new List<GameObject>();
        float nextQuare = NumberTiles_Fat_Race / 4;
        float quare = nextQuare / 2;// start pos
        for (int i = 0; i < NumberTiles_Fat_Race; i++)
        {
            //float x = 
            GameObject tile;
            if (i <= nextQuare)
            {
                tile = Instantiate(Tile_Fat_Race, new Vector3(size * (quare  - i), 0f,-size * quare), Quaternion.identity);
            }
            else if (i > nextQuare && i <= nextQuare * 2)
            {
                tile = Instantiate(Tile_Fat_Race, new Vector3(size * (quare-nextQuare), 0f, size * (i-nextQuare-quare)), Quaternion.Euler(0,90f,0));
            }
            else if (i > (nextQuare * 2) && i <= (nextQuare * 3))
            {
                tile = Instantiate(Tile_Fat_Race, new Vector3(size * (i-(nextQuare*2+quare)), 0f, size * (nextQuare - quare)), Quaternion.Euler(0f, 180f, 0f));
            }
            else
            {
                tile = Instantiate(Tile_Fat_Race, new Vector3(size * quare, 0f, size * (4 * nextQuare - i-quare)), Quaternion.Euler(0f, -90f, 0f));
            }

            tile.transform.parent = this.transform;
            Tiles_Fat_Race.Add(tile);
        }
    }
    public List<Dream> GetListDream()
    {
        List<Dream> dream_list = new List<Dream>();
        int rnd = Random.Range(0, this.dreams.Count - 1);
        if (rnd > 0 && rnd < dreams.Count - 1)
        {
            dream_list.Add(dreams[rnd]);
            dream_list.Add(dreams[rnd + 1]);
            dream_list.Add(dreams[rnd - 1]);
        }
        else if (rnd == 0)
        {
            dream_list.Add(dreams[rnd]);
            dream_list.Add(dreams[rnd + 1]);
            dream_list.Add(dreams[rnd + 2]);
        }
        else if (rnd == dreams.Count - 1)
        {
            dream_list.Add(dreams[rnd]);
            dream_list.Add(dreams[rnd - 1]);
            dream_list.Add(dreams[rnd - 2]);
        }
        Debug.Log(dream_list.Count);
        return dream_list;
    }
}
