using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Floor {

    public int nr;
    public int a;
    public int[,] chunksMap;
    public int[,] columnsMap;
    public int[,] entitiesMap;
   
    public Floor(int a, int level = 0, bool shopkeeper = false)
    {
        this.a = a;
        int maxX =(int)Map.instance.mapSize.x;
        int maxY =(int)Map.instance.mapSize.y;
        chunksMap = new int[maxX,maxY];
        columnsMap = new int[maxX,maxY];
        entitiesMap = new int[maxX, maxY];
        List<Vector2> possibleSpawns = new List<Vector2>();
        this.nr = Map.floors.Count;


        int halfChunk = Mathf.FloorToInt((maxY / a)/2);


        for (int x = 0; x < maxX; x++)
           for(int y = 0; y < maxY; y++)
           {
              chunksMap[x, y] = (y / (maxY / a) + (x / (maxX / a)) * a )+1;
              if ((x % (maxX / a) == halfChunk) && (y % (maxY / a) == halfChunk ))
                  columnsMap[x, y] = chunksMap[x,y];
              else columnsMap[x, y] = 0;
              if (columnsMap[x, y] > 0) entitiesMap[x, y] = 1;
              else
              {
                  entitiesMap[x, y] = 0;
                  possibleSpawns.Add(new Vector2(x, y));
              }
           }

        if(nr>0)
        {
            for(int i=0;i<3;i++)
            {
                Vector2 pos = possibleSpawns[UnityEngine.Random.Range(0, possibleSpawns.Count - 1)];
                entitiesMap[(int)pos.x, (int)pos.y] = 2;
                possibleSpawns.Remove(pos);
            }
        }
        ToDebug();
    }

    public void RewriteEntities(List<Vector2> entities, Floor floor)
    {
        if(floor != null)        
        {
            foreach(Vector2 entity in entities)
            {
                this.entitiesMap[(int)entity.x, (int)entity.y] = floor.entitiesMap[(int)entity.x, (int)entity.y];
                if(Map.floors.Count < this.nr)
                {
                    floor.RewriteEntities(entities, Map.floors[nr]);
                }
            }
        }
    }

    public void ToDebug()
    {
        int maxX = (int)Map.instance.mapSize.x;
        int maxY = (int)Map.instance.mapSize.y;

        for (int x = 0; x < maxX; x++)
        {
            String tmp = "";
            for (int y = 0; y < maxY; y++)
            {
                tmp +=entitiesMap[x, y].ToString() + ';';
            }
            MainDebug.WriteLine(tmp,15f);            
        }
        MainDebug.WriteLine("-----------------------------------", 15f);      
     
    }
}
