using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    private HeroControl _hero;
    private HexagonControl _hexagon;
    private EnemyManager _manager;

    private float _regeneration;
    void Start()
    {
        _hero = _manager.GetHero(_hexagon.ObjAbove);
        if(_hero!=null)
        _regeneration = _hero.GetRegeneration();
    }
    void FixedUpdate()
    {
        if (_hero != null)
            _hero.AdditionalTreatment(_regeneration);
    }
    public void Initialization(HexagonControl hexagon, EnemyManager enemyManager)
    {
        _hexagon = hexagon;
        _manager = enemyManager;
    }
    public void Delete()
    {
        Destroy(this);
    }
}
