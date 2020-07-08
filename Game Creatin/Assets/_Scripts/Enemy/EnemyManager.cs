﻿using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _ground, _flying;
    [SerializeField]
    private Transform[] _spawnPoint;
    private List<string> _enemyControls = new List<string>();
    [SerializeField]
    private List<HeroControl> _listHero = new List<HeroControl>();
    private List<EnemyControl> _listEnemyControls = new List<EnemyControl>();
    private Dictionary<string, GameObject> _enemies = new Dictionary<string, GameObject>();
    private Dictionary<string, int> _enemiesCount = new Dictionary<string, int>();

    [SerializeField]
    [Range(0, 100)]
    private int _maxQuantityGround, _maxQuantitFlying;
    private int _namberPointSpawn = 0;

    void Start()
    {
        _enemies["flying"] = _flying;
        _enemies["ground"] = _ground;

        _enemiesCount["flying"] = _maxQuantitFlying;
        _enemiesCount["ground"] = _maxQuantityGround;

        if (_maxQuantitFlying > 0)
            _enemyControls.Add("flying");
        if (_maxQuantityGround > 0)
            _enemyControls.Add("ground");

        foreach (var item in _listHero)
        {
            item.Initialization(this);
        }

        if (_enemyControls.Count>0)
        {
            StartCoroutine(Production());
        }
        else
        {
            Debug.LogError("Incorrect number of units(_maxQuantityEnemy)");
        }
    }
    private IEnumerator Production()
    {
        yield return new WaitForSeconds(0.1f);
        int n = 0;

        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                n++;

                if (_enemyControls.Count > 0)
                {
                    string name = _enemyControls[Random.Range(0, _enemyControls.Count)];

                    EnemyControl Enemy = Instantiate(_enemies[name], _spawnPoint[_namberPointSpawn].position, Quaternion.identity).GetComponent<EnemyControl>();
                    Enemy.gameObject.name = name;
                    _enemiesCount[name]--;
                    if (_enemiesCount[name]<=0)
                    {
                        _enemyControls.Remove(name);
                    }
                    Enemy.First(this);
                    GoalSelection(Enemy,name);

                    if (_namberPointSpawn != _spawnPoint.Length - 1)
                    {
                        _namberPointSpawn++;
                    }
                    else
                    {
                        _namberPointSpawn = 0;
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
            if (_enemyControls.Count != 0)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }
    private HeroControl GetNearestHero(HexagonControl hexagon)
    {
        HeroControl heroControl = null;
        float Magnitude = float.PositiveInfinity;

        for (int i = 0; i < _listHero.Count-1; i++)
        {
            List<HexagonControl> listHex = new List<HexagonControl>();
            listHex.AddRange(hexagon.GetWay(_listHero[i].HexagonMain()));
            float magnitude = 0;

            for (int j = 0; j < listHex.Count - 1; j++)
            {
                magnitude += (listHex[j].position - listHex[j + 1].position).magnitude;
            }

            if (Magnitude > magnitude)
            {
                Magnitude = magnitude;
                heroControl = _listHero[i];
            }
        }
        return heroControl;
    }
    private HeroControl GetNearestHeroMag(HexagonControl hexagon)
    {
        HeroControl heroControl = null;
        float Magnitude = float.PositiveInfinity;

        for (int i = 0; i < _listHero.Count-1; i++)
        {
            float magnitude = 0;

            magnitude += (hexagon.position - (Vector2)_listHero[i].transform.position).magnitude;

            if (Magnitude > magnitude)
            {
                Magnitude = magnitude;
                heroControl = _listHero[i];
            }
        }
        return heroControl;
    }
    public void GoalSelection(EnemyControl enemy,string name)
    {
        if (_listHero.Count <= 0)
        {
            StaticLevelManager.IsGameFlove = false;
            return;
        }

        HeroControl hero;

        if (name== "ground")
        {
            hero = GetNearestHero(enemy.HexagonMain());
        }
        else
        {
            hero = GetNearestHeroMag(enemy.HexagonMain());
        }

        if (hero == null)
        {
            Debug.LogError("No free hero");
            return;
        }

        hero.AddNewEnemy(enemy);
        enemy.HeroTarget = hero;
        enemy.StartWay(hero);
    }
    public HeroControl GetHero(IMove move)
    {
        for (int i = 0; i < _listHero.Count; i++)
        {
            if (_listHero[i].IMoveMain == move)
            {
                return _listHero[i];
            }
        }
        return null;
    }
    public void RemoveHero(HeroControl heroControl)
    {
        _listHero.Remove(heroControl);
    }
    public void InitializationList(HeroControl[] heroes)
    {
        _listHero.AddRange(heroes);
    }
}
