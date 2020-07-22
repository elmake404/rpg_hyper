using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove
{
    void DebuffSpeed(float debuff);
    float GetSpeed();
    GameObject GetObjMain();
    void StopMove(HexagonControl CollcionHex);
    void StopMoveTarget();
    void StopSpeedAtack(float timeStop);
    void StartWay(HexagonControl hexagonFinish, IMove EnemyTarget);
    bool IsGo();
    bool IsFlight();
    bool FreeSpaceCheck(bool Flight);
    HexagonControl HexagonMain();
    List<HexagonControl> GetSurroundingHexes();
}
