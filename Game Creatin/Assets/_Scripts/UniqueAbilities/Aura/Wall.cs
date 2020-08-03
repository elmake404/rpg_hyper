using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private HexagonControl _hexagon;
    private float _duration, _deceleration;
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (_duration<=0)
        {
            _hexagon.DebuffHexEnemy.Speed -= _deceleration;
            _hexagon.Sprite.color = new Color(1,1,1,1);
            Destroy(this);
        }
        else
        {
            _duration -= Time.deltaTime;
        }
    }

    public void Initialization(HexagonControl hexagon, Color color, float duration, float deceleration)
    {
        _deceleration = deceleration;
        _hexagon = hexagon;
        _hexagon.Sprite.color = color;
        _duration = duration;
        hexagon.DebuffHexEnemy.Speed += _deceleration;
    }
}
