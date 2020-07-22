using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControl 
{
    void Collision(Vector2 NextPos);
    void CollisionDebuff(Vector2 NextPos);
    float GetHealthProcent();
    HexagonControl HexagonMain();
    IMove Target();
    List<HexagonControl> GetSurroundingHexes();

}
