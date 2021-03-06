﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    [SerializeField]
    private NavSurface _navSurface;
    [SerializeField]
    private EnemyManager enemyManager;
    [SerializeField]
    private Transform[] hexagons;

    private static Vector2 _mapPos;

    [HideInInspector]
    public static float X, Y;
    [HideInInspector]
    public static HexagonControl[,] MapNav = new HexagonControl[9, 20];//масив содержащий все 6-ти угольники
    void Awake()//измени
    {
        //enemyManager.InitializationList(_heroControls);
        _mapPos = transform.position;
        for (int i = 0; i < hexagons.Length; i++)
        {
            hexagons[i].name = i.ToString();
            for (int j = 0; j < hexagons[i].childCount; j++)
            {
                HexagonControl hexagon = hexagons[i].GetChild(j).GetComponent<HexagonControl>();
                MapNav[i, j] = hexagon;
                hexagons[i].GetChild(j).name = j.ToString();
                hexagon.NamberHex();
            }
        }

    }
    public void DataRecords()
    {
        _mapPos = transform.position;
        
        for (int i = 0; i < hexagons.Length; i++)
        {
            hexagons[i].name = i.ToString();
            for (int j = 0; j < hexagons[i].childCount; j++)
            {
                HexagonControl hexagon = hexagons[i].GetChild(j).GetComponent<HexagonControl>();
                MapNav[i, j] = hexagon;
                hexagons[i].GetChild(j).name = j.ToString();
                hexagon.NamberHex();
                hexagon.GetHexagonMain().NamberHex();
                if (hexagon.GetHexagonMain().TypeHexagon != 1)
                    _navSurface.ListHexagonControls.Add(hexagon.GetHexagonMain());
            }
        }
    }
    public void DataRemove()
    {        
        for (int i = 0; i < hexagons.Length; i++)
        {
            hexagons[i].name = i.ToString();
            for (int j = 0; j < hexagons[i].childCount; j++)
            {
                
                HexagonControl hexagon = hexagons[i].GetChild(j).GetComponent<HexagonControl>();
                hexagon.GetHexagonMain().DataRemove();
                hexagon.DataRemove();
            }
        }
    }
    private static void OwnershipCheck(int _row, int _column, out float Row, out int Column, Vector2 pos)
    {
        if (_row > MapNav.GetLength(0) - 1)
        {
            _row = MapNav.GetLength(0) - 1;
        }
        else if (_row < 0)
        {
            _row = 0;
        }

        if (_column > MapNav.GetLength(1) - 1)
        {
            _column = MapNav.GetLength(1) - 1;
        }
        else if (_column < 0)
        {
            _column = 0;
        }


        List<HexagonControl> hexagons = new List<HexagonControl>();

        int _columbias = (_row % 2) == 0 ? 1 : -1;

        if ((MapNav[_row, _column].position - pos).magnitude <= 1.8f)
        {
            Row = _row;
            Column = _column;
            return;
        }

        #region AddToList
        hexagons.Add(MapNav[_row, _column]);
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {
            if (mag > (hexagons[i].position - pos).magnitude)
            {
                mag = (hexagons[i].position - pos).magnitude;
                hexagon = hexagons[i];
                if (mag <= 1.8f)
                {
                    Row = hexagons[i].Row;
                    Column = hexagons[i].Column;
                    return;
                }

            }
        }
        Row = hexagon.Row;
        Column = hexagon.Column;
        return;
    }
    private static HexagonControl OwnershipCheck(int _row, int _column, Vector2 pos, int Layer)
    {
        List<HexagonControl> hexagons = new List<HexagonControl>();
        int _columbias = (_row % 2) == 0 ? 1 : -1;
        if ((MapNav[_row, _column].position - pos).magnitude <= 1.8f)
        {
            if (MapNav[_row, _column].gameObject.layer == Layer && (MapNav[_row, _column].TypeHexagon != 1 || Layer == 12) || (Layer == 12 && MapNav[_row, _column].TypeHexagon == 3))
                return MapNav[_row, _column];
        }

        #region AddToList
        hexagons.Add(MapNav[_row, _column]);
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {

            if (hexagons[i].gameObject.layer == Layer && (hexagons[i].TypeHexagon != 1 || Layer == 12) || (Layer == 12 && hexagons[i].TypeHexagon == 3))
            {

                if (mag > (hexagons[i].position - pos).magnitude)
                {
                    mag = (hexagons[i].position - pos).magnitude;
                    hexagon = hexagons[i];
                    if (mag <= 1.8f)
                    {
                        return hexagons[i];
                    }
                }
            }
        }

        return hexagon;
    }
    private static HexagonControl OwnershipCheckadditional(int _row, int _column, Vector2 pos)
    {
        List<HexagonControl> hexagons = new List<HexagonControl>();
        int _columbias = (_row % 2) == 0 ? 1 : -1;

        #region AddToList
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {
            if (mag > (hexagons[i].position - pos).magnitude)
            {
                mag = (hexagons[i].position - pos).magnitude;
                hexagon = hexagons[i];
                if (mag <= 1.8f)
                {
                    return hexagons[i];
                }

            }
        }
        return hexagon;
    }
    public static HexagonControl[] GetPositionOnTheMap(Vector2 Target, Vector2 Position) // возвращает гексагон через позицию (может определить два блока)
    {


        Vector2 Normalized = (Target - Position).normalized;
        float XTarget = (float)System.Math.Round(Normalized.x, 1);
        float YTarget = (float)System.Math.Round(Normalized.y, 1);
        float Y = (Position.y - _mapPos.y) / -3f;
        float YOld = (Position.y - _mapPos.y) / -3f;

        int YMax = Mathf.Abs(Mathf.RoundToInt(Y));
        float Difference = 0;
        float f = ((Position.x - (_mapPos.x - 3.46f)) / 1.73f);
        float R = Mathf.Floor(f) * 1.73f - (Position.x - (_mapPos.x - 3.46f));
        int G = (int)f % 2 == 0 ? 0 : 1;

        if (Mathf.Abs(YMax - Mathf.Abs(Y)) < Mathf.Abs(G - ((0.333f + ((0.333f / 1.73f) * Mathf.Abs(R))))))
        {
            Y = Mathf.Round(Mathf.Abs(Y));
        }
        else
        {
            if (YMax - Mathf.Abs(Y) > 0)
            {
                Y = Mathf.Floor(Mathf.Abs(Y));
            }
            else
            {
                Y = Mathf.Ceil(Mathf.Abs(Y));
            }
        }

        if ((Y % 2) != 0)
        {
            Difference = 0.5f;
        }

        float factor = (MapNav[0, 1].position.x - MapNav[0, 0].position.x);
        //Debug.Log(factor);
        float X = ((Position.x - _mapPos.x) / factor) + Difference;

        X = X > 0 ? X : 0;
        int XInt;

        if (X == 0.5)
        {
            XInt = 1;
        }
        else
        {
            XInt = Mathf.RoundToInt(X);
        }

        OwnershipCheck((int)Y, XInt, out Y, out XInt, Position);

        List<HexagonControl> hexagonsList = new List<HexagonControl>();

        if (Mathf.Abs((float)System.Math.Round((XInt - X), 2)) == 0.5f && XTarget == 0)
        {
            if (XInt <= 19)
            {
                hexagonsList.Add(MapNav[(int)Y, XInt]);
            }

            if (XInt > 0 && (Position - MapNav[(int)Y, XInt - 1].position).magnitude <= 1.8f)
            {
                hexagonsList.Add(MapNav[(int)Y, XInt - 1]);
            }

            HexagonControl[] hexagons = new HexagonControl[hexagonsList.Count];

            for (int i = 0; i < hexagonsList.Count; i++)
            {
                hexagons[i] = hexagonsList[i];
            }

            return hexagons;
        }
        else if (Mathf.Abs((float)System.Math.Round((XInt - X), 3)) < 0.5f && (Mathf.Abs((float)System.Math.Round((YMax - YOld), 3)) < 0.51 && Mathf.Abs((float)System.Math.Round((YMax - YOld), 3)) > 0.33) && ((Mathf.Abs(XTarget) == 0.9f) && (Mathf.Abs(YTarget) == 0.5f)))
        {
            hexagonsList.Add(OwnershipCheckadditional((int)Y, XInt, Position));
            hexagonsList.Add(MapNav[(int)Y, XInt]);

            HexagonControl[] hexagons = new HexagonControl[hexagonsList.Count];

            for (int i = 0; i < hexagonsList.Count; i++)
            {
                hexagons[i] = hexagonsList[i];
            }

            return hexagons;

        }
        else
        {
            HexagonControl[] hexagons = new HexagonControl[1];
            hexagons[0] = MapNav[(int)Y, XInt];

            return hexagons;
        }
    }
    public static HexagonControl GetPositionOnTheMap(Vector2 Position, int Layer)// возвращает гексагон через позицию
    {
        float Y = (Position.y - _mapPos.y) / -3f;
        int YMax = Mathf.RoundToInt(Y);
        float Difference = 0;
        float f = ((Position.x - (_mapPos.x - 3.46f)) / 1.73f);
        float R = Mathf.Floor(f) * 1.73f - (Position.x - (_mapPos.x - 3.46f));
        int G = (int)f % 2 == 0 ? 0 : 1;

        if (YMax - Mathf.Abs(Y) < Mathf.Abs(G - ((0.333f + ((0.333f / 1.73f) * Mathf.Abs(R))))))
        {
            Y = Mathf.Round(Y);
        }
        else
        {
            if (YMax - Y > 0)
            {
                Y = Mathf.Floor(Y);
            }
            else
            {
                Y = Mathf.Ceil(Y);
            }
        }

        if (Y >= MapNav.GetLength(0))
        {
            Y = MapNav.GetLength(0) - 1;
        }
        else if (Y < 0)
        {
            Y = 0;
        }

        if ((Y % 2) != 0)
        {
            Difference = 0.5f;
        }

        float factor = (MapNav[0, 1].transform.position.x - MapNav[0, 0].transform.position.x);
        float X = ((Position.x - _mapPos.x) / factor) + Difference;

        X = X > 0 ? X : 0;

        int XInt = Mathf.RoundToInt(X);

        if (XInt >= MapNav.GetLength(1))
        {
            XInt = MapNav.GetLength(1) - 1;
        }
        else if (XInt < 0)
        {
            XInt = 0;
        }

        return OwnershipCheck((int)Y, XInt, Position, Layer);
    }
    public static HexagonControl FieldPosition(int layer, Vector2 position)//гексагон к которому принадлежит герой
    {
        Vector2 difference = layer == 8 ? Vector2.zero : new Vector2(X, Y);
        int layerHex = layer == 8 ? 9 : 12;
        HexagonControl hexagon = GetPositionOnTheMap(position - difference, layerHex);

        if (hexagon == null)
        {
            Debug.Log(layerHex);
        }

        return hexagon.GetHexagonMain();//нужный 6-ти угольник  
    }

    #region BeforeStart
    private static HexagonControl OwnershipCheckMapBeforeStart(int _row, int _column, Vector2 pos)
    {
        List<HexagonControl> hexagons = new List<HexagonControl>();
        int _columbias = (_row % 2) == 0 ? 1 : -1;
        if ((MapNav[_row, _column].position - pos).magnitude <= 1.8f)
        {
            if (MapNav[_row, _column].GetHexagonMain().TypeHexagon != 1 && MapNav[_row, _column].GetHexagonMain().IsFree)
                return MapNav[_row, _column];
        }

        #region AddToList
        hexagons.Add(MapNav[_row, _column]);
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {
            if (hexagons[i].GetHexagonMain().TypeHexagon != 1 && hexagons[i].GetHexagonMain().IsFree)
            {
                if (mag > (hexagons[i].position - pos).magnitude)
                {

                    mag = (hexagons[i].position - pos).magnitude;
                    hexagon = hexagons[i];
                    if (mag <= 1.8f)
                    {
                        return hexagons[i];
                    }
                }
            }
        }

        return hexagon;
    }
    public static HexagonControl GetPositionOnTheMapBeforeStart(Vector2 Position)// возвращает гексагон через позицию
    {
        float Y = (Position.y - _mapPos.y) / -3f;
        int YMax = Mathf.Abs(Mathf.RoundToInt(Y));
        float Difference = 0;
        float f = ((Position.x - (_mapPos.x - 3.46f)) / 1.73f);
        float R = Mathf.Floor(f) * 1.73f - (Position.x - (_mapPos.x - 3.46f));
        int G = (int)f % 2 == 0 ? 0 : 1;

        if (YMax - Y < Mathf.Abs(G - ((0.333f + ((0.333f / 1.73f) * Mathf.Abs(R))))))
        {
            Y = Mathf.Round(Y);
        }
        else
        {
            if (YMax - Y > 0)
            {
                Y = Mathf.Floor(Y);
            }
            else
            {
                Y = Mathf.Ceil(Y);
            }
        }

        if ((Y % 2) != 0)
        {
            Difference = 0.5f;
        }

        float factor = (MapNav[0, 1].transform.position.x - MapNav[0, 0].transform.position.x);
        float X = ((Position.x - _mapPos.x) / factor) + Difference;

        X = X > 0 ? X : 0;
        int XInt = Mathf.RoundToInt(X);

        if (XInt >= MapNav.GetLength(1) || (XInt < 0) || (Y >= MapNav.GetLength(0)) || (Y < 0))
        {
            return null;
        }
        else
            return OwnershipCheckMapBeforeStart((int)Y, XInt, Position);
    }
    public static HexagonControl FieldPositionMapBeforeStart(int layer, Vector2 position)//гексагон к которому принадлежит герой
    {
        Vector2 difference = layer == 8 ? Vector2.zero : new Vector2(X, Y);
        HexagonControl hexagon = GetPositionOnTheMapBeforeStart(position - difference);

        return hexagon;//нужный 6-ти угольник  
    }
    #endregion
    #region Cast
    private static HexagonControl OwnershipCheckMapCast(int _row, int _column, Vector2 pos)
    {
        List<HexagonControl> hexagons = new List<HexagonControl>();
        int _columbias = (_row % 2) == 0 ? 1 : -1;
        if ((MapNav[_row, _column].position - pos).magnitude <= 1.8f)
        {
            return MapNav[_row, _column];
        }

        #region AddToList
        hexagons.Add(MapNav[_row, _column]);
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {
            if (mag > (hexagons[i].position - pos).magnitude)
            {

                mag = (hexagons[i].position - pos).magnitude;
                hexagon = hexagons[i];
                if (mag <= 1.8f)
                {
                    return hexagons[i];
                }
            }
        }

        return hexagon;
    }
    public static HexagonControl GetPositionOnTheMapCast(Vector2 Position)// возвращает гексагон через позицию
    {
        float Y = (Position.y - _mapPos.y) / -3f;
        int YMax = Mathf.Abs(Mathf.RoundToInt(Y));
        float Difference = 0;
        float f = ((Position.x - (_mapPos.x - 3.46f)) / 1.73f);
        float R = Mathf.Floor(f) * 1.73f - (Position.x - (_mapPos.x - 3.46f));
        int G = (int)f % 2 == 0 ? 0 : 1;

        if (YMax - Y < Mathf.Abs(G - ((0.333f + ((0.333f / 1.73f) * Mathf.Abs(R))))))
        {
            Y = Mathf.Round(Y);
        }
        else
        {
            if (YMax - Y > 0)
            {
                Y = Mathf.Floor(Y);
            }
            else
            {
                Y = Mathf.Ceil(Y);
            }
        }

        if ((Y % 2) != 0)
        {
            Difference = 0.5f;
        }

        float factor = (MapNav[0, 1].transform.position.x - MapNav[0, 0].transform.position.x);
        float X = ((Position.x - _mapPos.x) / factor) + Difference;

        X = X > 0 ? X : 0;
        int XInt = Mathf.RoundToInt(X);

        if (XInt >= MapNav.GetLength(1) || (XInt < 0) || (Y >= MapNav.GetLength(0)) || (Y < 0))
        {
            return null;
        }
        else
            return OwnershipCheckMapCast((int)Y, XInt, Position);
    }
    public static HexagonControl FieldPositionMapCast(Vector2 position)//гексагон к которому принадлежит герой
    {
        HexagonControl hexagon = GetPositionOnTheMapCast(position);

        return hexagon;//нужный 6-ти угольник  
    }
    #endregion
    #region Fly
    private static HexagonControl OwnershipCheckFly(int _row, int _column, Vector2 pos)
    {
        List<HexagonControl> hexagons = new List<HexagonControl>();
        int _columbias = (_row % 2) == 0 ? 1 : -1;
        if ((MapNav[_row, _column].position - pos).magnitude <= 1.8f)
        {
            return MapNav[_row, _column];
        }

        #region AddToList
        hexagons.Add(MapNav[_row, _column]);
        if (_column < MapNav.GetLength(1) - 1)
            hexagons.Add(MapNav[_row, _column + 1]);

        if (_column > 0)
            hexagons.Add(MapNav[_row, _column - 1]);

        if (_row < MapNav.GetLength(0) - 1)
        {
            hexagons.Add(MapNav[_row + 1, _column]);

            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row + 1, _column + _columbias]);
        }

        if (_row > 0)
        {
            hexagons.Add(MapNav[_row - 1, _column]);
            if (_column + _columbias > 0 && _column + _columbias < MapNav.GetLength(1) - 1)
                hexagons.Add(MapNav[_row - 1, _column + _columbias]);
        }
        #endregion

        float mag = float.PositiveInfinity;
        HexagonControl hexagon = null;
        for (int i = 0; i < hexagons.Count; i++)
        {

            if (mag > (hexagons[i].position - pos).magnitude)
            {
                mag = (hexagons[i].position - pos).magnitude;
                hexagon = hexagons[i];
                if (mag <= 1.8f)
                {
                    return hexagons[i];
                }
            }
        }

        return hexagon;
    }
    public static HexagonControl GetPositionOnTheMapFly(Vector2 Position)// возвращает гексагон через позицию
    {
        float Y = (Position.y - _mapPos.y) / 3f;
        int YMax = Mathf.Abs(Mathf.RoundToInt(Y));
        float Difference = 0;
        float f = ((Position.x - (_mapPos.x - 3.46f)) / 1.73f);
        float R = Mathf.Floor(f) * 1.73f - (Position.x - (_mapPos.x - 3.46f));
        int G = (int)f % 2 == 0 ? 0 : 1;

        if (Mathf.Abs(YMax - Mathf.Abs(Y)) < Mathf.Abs(G - ((0.333f + ((0.333f / 1.73f) * Mathf.Abs(R))))))
        {
            Y = Mathf.Round(Mathf.Abs(Y));
        }
        else
        {
            if (YMax - Mathf.Abs(Y) > 0)
            {
                Y = Mathf.Floor(Mathf.Abs(Y));
            }
            else
            {
                Y = Mathf.Ceil(Mathf.Abs(Y));
            }
        }

        if (Y >= MapNav.GetLength(0))
        {
            Y = MapNav.GetLength(0) - 1;
        }
        else if (Y < 0)
        {
            Y = 0;
        }


        if ((Y % 2) != 0)
        {
            Difference = 0.5f;
        }

        float factor = (MapNav[0, 1].transform.position.x - MapNav[0, 0].transform.position.x);
        float X = ((Position.x - _mapPos.x) / factor) + Difference;

        X = X > 0 ? X : 0;
        int XInt = Mathf.RoundToInt(X);

        if (XInt >= MapNav.GetLength(1))
        {
            XInt = MapNav.GetLength(1) - 1;
        }
        else if (XInt < 0)
        {
            XInt = 0;
        }


        return OwnershipCheckFly((int)Y, XInt, Position);
    }
    public static HexagonControl FieldPositionFly(int layer, Vector2 position)//гексагон к которому принадлежит герой
    {
        Vector2 difference = layer == 8 ? Vector2.zero : new Vector2(X, Y);
        HexagonControl hexagon = GetPositionOnTheMapFly(position - difference);

        return hexagon.GetHexagonMain();//нужный 6-ти угольник  
    }
    #endregion
}
