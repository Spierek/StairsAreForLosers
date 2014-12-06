using UnityEngine;
using System;
using System.Collections;

public class Floor {

    public int nr;
    public int[,] chunksMap;
    public int[,] columnsMap;

    public Floor(int a, bool shopkeeper = false)
    {
        int maxX =(int)Map.instance.mapSize.x;
        int maxY =(int)Map.instance.mapSize.y;
        chunksMap = new int[maxX,maxY];
        columnsMap = new int[maxX,maxY];

        int halfChunk = Mathf.FloorToInt((maxY / a)/2);


        for (int x = 0; x < maxX; x++)
           for(int y = 0; y < maxY; y++)
           {
              chunksMap[x, y] = (y / (maxY / a) + (x / (maxX / a)) * a )+1;
              if ((x % (maxX / a) == halfChunk) && (y % (maxY / a) == halfChunk ))
                  columnsMap[x, y] = chunksMap[x,y];
              else columnsMap[x, y] = 0;
           }
        ToDebug();
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
                tmp += columnsMap[x, y].ToString() + ';';
            }
            MainDebug.WriteLine(tmp,15f);            
        }
     
    }
}
