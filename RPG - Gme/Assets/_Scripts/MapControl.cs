using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    public static PointControl[,] MapPoint = new PointControl[20, 10];
    void Start()
    {
        for (int x = 0; x < MapPoint.GetLength(0); x++)
        {
            for (int y = 0; y < MapPoint.GetLength(1); y++)
            {
                MapPoint[x, y] = transform.GetChild(x).transform.GetChild(y).GetComponent<PointControl>();
            }
        }
    }

    void Update()
    {
        
    }
    public static PointControl GetPositionOntheMap(int X , int Y)
    {
        return MapPoint[X, Y];
    }
}
