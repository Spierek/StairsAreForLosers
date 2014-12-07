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
    public int[,] colorMap;
   
    public Floor(int a, bool shopkeeper = false)
    {
        this.a = a;
        int maxX =(int)Map.instance.mapSize.x;
        int maxY =(int)Map.instance.mapSize.y;
        chunksMap = new int[maxX,maxY];
        columnsMap = new int[maxX,maxY];
        entitiesMap = new int[maxX, maxY];
        colorMap = new int[maxX, maxY];
        List<Vector2> possibleSpawns = new List<Vector2>();
        this.nr = Map.floors.Count;


        int halfChunk = Mathf.FloorToInt((maxY / a)/2);


        for (int x = 0; x < maxX; x++)
           for(int y = 0; y < maxY; y++)
           {
              chunksMap[x, y] = (y / (maxY / a) + (x / (maxX / a)) * a )+1;
                  if ((x % (maxX / a) == halfChunk) && (y % (maxY / a) == halfChunk) && !shopkeeper)
                      columnsMap[x, y] = chunksMap[x, y];
                  else columnsMap[x, y] = 0;
                  if (columnsMap[x, y] > 0 && !shopkeeper) entitiesMap[x, y] = 1;
              else
              {
                  entitiesMap[x, y] = 0;
                  possibleSpawns.Add(new Vector2(x, y));
              }

              colorMap[x, y] = this.nr;
           }

        if(nr>0)
        {
            for(int i=0;i<12;i++)
            {
                Vector2 pos = possibleSpawns[UnityEngine.Random.Range(0, possibleSpawns.Count - 1)];
                entitiesMap[(int)pos.x, (int)pos.y] = 2;
                possibleSpawns.Remove(pos);
            }
        }
        if(nr>1)
        {
            for (int i = 0; i < 24; i++)
            {
                Vector2 pos = possibleSpawns[UnityEngine.Random.Range(0, possibleSpawns.Count - 1)];
                entitiesMap[(int)pos.x, (int)pos.y] = 3;
                possibleSpawns.Remove(pos);
            }
            for (int i = 0; i < 6; i++)
            {
                Vector2 pos = possibleSpawns[UnityEngine.Random.Range(0, possibleSpawns.Count - 1)];
                entitiesMap[(int)pos.x, (int)pos.y] = 5;
                possibleSpawns.Remove(pos);
            }
        }
        if (nr > 2)
        {
            for (int i = 0; i < 12; i++)
            {
                Vector2 pos = possibleSpawns[UnityEngine.Random.Range(0, possibleSpawns.Count - 1)];
                entitiesMap[(int)pos.x, (int)pos.y] = 4;
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
                this.colorMap[(int)entity.x, (int)entity.y] = floor.colorMap[(int)entity.x, (int)entity.y];
               
            }
            if (Map.floors.Count > floor.nr+1)
            {
                floor.RewriteEntities(entities, Map.floors[floor.nr+1]);
            }
            else
            {
                floor.RewriteEntities(entities, null);
            }
        }
        else
        {
            foreach (Vector2 entity in entities)
            {
                this.entitiesMap[(int)entity.x, (int)entity.y] = -1;
            }
        }
    }

    public void ToDebug()
    {
        int maxX = (int)Map.instance.mapSize.x;
        int maxY = (int)Map.instance.mapSize.y;
        MainDebug.WriteLine("Floor["+this.nr.ToString()+"]---------------------------", 15f);      
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
