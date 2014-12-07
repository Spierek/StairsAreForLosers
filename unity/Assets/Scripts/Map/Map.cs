using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour {

    public String[] indexcolors = new String[]{
        "000000", "FFFF00", "1CE6FF", "FF34FF", "FF4A46", "008941", "006FA6", "A30059",
        "FFDBE5", "7A4900", "0000A6", "63FFAC", "B79762", "004D43", "8FB0FF", "997D87",
        "5A0007", "809693", "FEFFE6", "1B4400", "4FC601", "3B5DFF", "4A3B53", "FF2F80",
        "61615A", "BA0900", "6B7900", "00C2A0", "FFAA92", "FF90C9", "B903AA", "D16100",
        "DDEFFF", "000035", "7B4F4B", "A1C299", "300018", "0AA6D8", "013349", "00846F",
        "372101", "FFB500", "C2FFED", "A079BF", "CC0744", "C0B9B2", "C2FF99", "001E09",
        "00489C", "6F0062", "0CBD66", "EEC3FF", "456D75", "B77B68", "7A87A1", "788D66",
        "885578", "FAD09F", "FF8A9A", "D157A0", "BEC459", "456648", "0086ED", "886F4C",

        "34362D", "B4A8BD", "00A6AA", "452C2C", "636375", "A3C8C9", "FF913F", "938A81",
        "575329", "00FECF", "B05B6F", "8CD0FF", "3B9700", "04F757", "C8A1A1", "1E6E00",
        "7900D7", "A77500", "6367A9", "A05837", "6B002C", "772600", "D790FF", "9B9700",
        "549E79", "FFF69F", "201625", "72418F", "BC23FF", "99ADC0", "3A2465", "922329",
        "5B4534", "FDE8DC", "404E55", "0089A3", "CB7E98", "A4E804", "324E72", "6A3A4C",
        "83AB58", "001C1E", "D1F7CE", "004B28", "C8D0F6", "A3A489", "806C66", "222800",
        "BF5650", "E83000", "66796D", "DA007C", "FF1A59", "8ADBB4", "1E0200", "5B4E51",
        "C895C5", "320033", "FF6832", "66E1D3", "CFCDAC", "D0AC94", "7ED379", "012C58"
};

    public Vector2 mapSize;

    public static Map instance;
    public static List<Floor> floors;

    public GameObject tilePrefab;
    public GameObject columnPrefab;

    public GameObject skeletonPrefab;

    public int floorCount;

    public float tileSpacing;

    void Awake()
    {
        
    }

    Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
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
        floors[0].ToDebug();
    }

    public void GenerateTiles(Floor floor)
    {
        GameObject floorRoot = new GameObject();
        floorRoot.transform.parent = this.transform;
        floorRoot.name = "Floor[" + floor.nr.ToString() + "]";
        for (int x = 0; x < mapSize.x; x++)
            for (int y = 0; y < mapSize.y; y++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3((x * tileSpacing) - mapSize.x/2, (y * tileSpacing) - mapSize.y/2, 0f), Quaternion.identity) as GameObject;
                tile.transform.parent = floorRoot.transform;
                tile.name = "[" + x.ToString() + "," + y.ToString() + "]";
                tile.GetComponent<SpriteRenderer>().color = HexToColor(indexcolors[Convert.ToInt32(floor.chunksMap[x, y])]);
                tile.GetComponent<Tile>().doNotTween = true;
                if (floor.columnsMap[x, y] > 0)
                {
                    GameObject column = Instantiate(columnPrefab, new Vector3((x * tileSpacing) - mapSize.x / 2 - (tileSpacing * (floor.a % 2))
                                                                             , (y * tileSpacing) - mapSize.y / 2 - (tileSpacing * (floor.a % 2))
                                                                             , -1f),
                                                                             Quaternion.identity) as GameObject;
                    column.transform.parent = floorRoot.transform;
                    column.name = "Column[" + floor.columnsMap[x, y].ToString() + "]";
                    column.GetComponent<Column>().ID = floor.columnsMap[x, y];
                    column.GetComponentInChildren<SpriteRenderer>().color = HexToColor(indexcolors[Convert.ToInt32(floor.columnsMap[x, y])]);
                }
            }
    }

	// Use this for initialization
	void Start () {
        instance = this;
        GenerateFloors();
        floors = new List<Floor>();
        floors.Add(new Floor(2));
        floors.Add(new Floor(2));
        GenerateTiles(floors[0]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
