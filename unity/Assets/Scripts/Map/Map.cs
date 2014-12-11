using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public List<Color> indexcolors = new List<Color>();

    public Vector2 mapSize;

    public static Map instance;
    public static List<Floor> floors;

    public GameObject tilePrefab;
    public GameObject columnPrefab;
    public GameObject coinsPrefab;
    public GameObject heartPrefab;

    public GameObject skeletonPrefab;

    public int floorCount;

    private int structure = 2;
    public int higestFloor;
    public int bestFloor;
    public int enemyMultiplier = 1;
    public int floorcount = 0;

    public float tileSpacing;

    public int columnsCount;
    public int enemiesCount;
    public bool floorSetDone;
    public int destroyedColumns;

    public Color GetIndexColor(int i) {
        int a = Mathf.FloorToInt((i % 5)/5);
        return LerpColors(indexcolors[a], indexcolors[a+1], ((float)i % 5)/5);
    }

    private Color LerpColors(Color A, Color B, float t) {
        return new Color(Mathf.Lerp(A.r, B.r, t), Mathf.Lerp(A.g, B.g, t), Mathf.Lerp(A.b, B.b, t), 1f);
    }

    void GenerateFloors()
    {
        List<Floor> floors = new List<Floor>();
        
    }

    public void DemolishChunk(int nr)
    {
        List<Vector2> entitiesToDrop = new List<Vector2>();
        for (int x = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                if (floors[0].chunksMap[x, y] == nr) 
                {
                    GameObject.Find("[" + x + "," + y + "]").GetComponent<Tile>().initDeath();
                    entitiesToDrop.Add(new Vector2(x, y));
                }
                
            }

        floors[0].RewriteEntities(entitiesToDrop, floors[1]);

        foreach (Floor floor in floors) floor.ToDebug();
    }

    public void GenerateTiles(Floor floor, bool columnsOnly = false)
    {
        GameObject floorRoot = new GameObject();
        float delayVal = 0.0f;
        floorRoot.transform.parent = this.transform;
        floorRoot.name = "Floor root";
        Color col = GetIndexColor(floor.nr);
        for (int x = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                if (!columnsOnly)
                {
                    GameObject tile = Instantiate(tilePrefab, new Vector3((x * tileSpacing) - mapSize.x / 2, (y * tileSpacing) - mapSize.y / 2, 0f), Quaternion.identity) as GameObject;
                    tile.transform.parent = floorRoot.transform;
                    tile.name = "[" + x.ToString() + "," + y.ToString() + "]";
                    tile.GetComponentInChildren<SpriteRenderer>().color = col;
                    tile.GetComponent<Tile>().location = new Vector2(x, y);
                    tile.GetComponent<Tile>().doNotTween = true;
                }
                if (floor.columnsMap[x, y] > 0)
                {
                    GameObject column = Instantiate(columnPrefab, new Vector3((x * tileSpacing) - mapSize.x / 2 - (tileSpacing * (floor.a % 2))
                                                                             , (y * tileSpacing) - mapSize.y / 2 - (tileSpacing * (floor.a % 2))
                                                                             , -1f),
                                                                             Quaternion.identity) as GameObject;
                    column.transform.parent = floorRoot.transform;
                    column.name = "Column[" + floor.columnsMap[x, y].ToString() + "]";
                    column.GetComponent<Column>().ID = floor.columnsMap[x, y];
                    //column.GetComponent<Column>().doNotTween = true;
                    column.GetComponent<Column>().delayTween = true;
                    column.GetComponent<Column>().delayVal = delayVal;
                    column.GetComponent<Column>().finalPosition = column.transform.position;
                    delayVal += 0.1f;
                    columnsCount++;
                   // column.GetComponentInChildren<SpriteRenderer>().color = HexToColor(indexcolors[Convert.ToInt32(floor.columnsMap[x, y])]);
                }
            }
    }

    // Use this for initialization
    void Start () { 
        instance = this;
        bestFloor = 0;
        floorSetDone = false;
        higestFloor = 0;
        GenerateFloors();
        floors = new List<Floor>();
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure, true));
        GenerateTiles(floors[0]);
    }
    
    void NextLevelFloors()
    {
        //Remove k
        structure++;
        if (structure > 4) structure = 4;

        GameObject oldFloorRoot = GameObject.Find("Floor root");
        floors.Clear();
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure));
        floors.Add(new Floor(structure, true));
        GenerateTiles(floors[0]);
        Destroy(oldFloorRoot);
        floorSetDone = false;
    }

    // Update is called once per frame
    void Update () {
        MainDebug.WriteLine("Columns Count: " + columnsCount);
        MainDebug.WriteLine("Best Floor: " + bestFloor);
        MainDebug.WriteLine("Enemies Count: " + enemiesCount);
        if (columnsCount == 0 && !floorSetDone)
        {
            Invoke("NextLevelFloors", 3f);
            floorSetDone = true;
        }
    }
}
