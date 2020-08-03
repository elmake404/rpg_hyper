using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyControl : MonoBehaviour, IControl
{
    private EnemyManager _enemyManager;
    [SerializeField]
    private HexagonControl _hexagonMain;

    [SerializeField]
    private float _healthPoints, _atackSpeed, _attackPower, _atackDistens, _powerRegeneration, _armor;
    private float _atackDistensConst, _healthPointsConst, _regeneration, _debuffHealth, _debuffAtackSpeed, _damagEnvironment;

    [HideInInspector]
    public List<HexagonControl> AnApproac = new List<HexagonControl>();
    [HideInInspector]
    public HeroControl HeroTarget;
    [HideInInspector]
    public List<HeroControl> Pursuer = new List<HeroControl>();

    [HideInInspector]
    public bool IsAttack;
    public IMove IMoveMain;
    public IControl IControlMain;

    private void Awake()
    {
        _atackDistensConst = (1.73f * (_atackDistens * 2)) + 0.1f;
        _healthPointsConst = _healthPoints;
        _regeneration = (_healthPointsConst * _powerRegeneration / 100f) / 60;

        IMoveMain = GetComponent<IMove>();
        IControlMain = this;
    }
    private void Update()
    {
        if (_healthPoints <= 0)
        {
            if (HeroTarget != null)
            {
                HeroTarget.RemoveEnemy(this);
            }
            if (IMoveMain.IsFlight())
                _hexagonMain.GapFly();
            else
                _hexagonMain.Gap();

            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        Damage(_damagEnvironment,false);

        if (_healthPoints < _healthPointsConst)
        {
            _healthPoints += _regeneration;
            if (_healthPoints > _healthPointsConst)
            {
                _healthPoints = _healthPointsConst;
            }
        }

        _healthPoints += _debuffHealth;

        if (StaticLevelManager.IsGameFlove)
        {
            if (HeroTarget != null)
            {
                if (IMoveMain.IsFlight())
                {
                    Vector2 differenceHero = Vector2.zero;
                    Vector2 differenceMain = Vector2.zero;

                    if (gameObject.layer == 8 && HeroTarget.gameObject.layer == 11)
                    {
                        differenceHero = new Vector2(MapControl.X, MapControl.Y);
                    }

                    if (gameObject.layer == 11 && HeroTarget.gameObject.layer == 8)
                    {
                        differenceMain = new Vector2(MapControl.X, MapControl.Y);
                    }

                    if ((((Vector2)HeroTarget.transform.position - differenceHero) - ((Vector2)transform.position - differenceMain)).magnitude <= _atackDistensConst)
                    {
                        if (IMoveMain.IsGo())
                        {
                            IMoveMain.StopMoveTarget();
                        }

                        if (!IsAttack)
                        {
                            StartCoroutine(Atack());
                        }
                    }

                }
                else
                {
                    if (((Vector2)HeroTarget.transform.position - (Vector2)transform.position).magnitude <= _atackDistensConst)
                    {
                        if (IMoveMain.IsGo())
                        {
                            IMoveMain.StopMoveTarget();
                        }

                        if (!IsAttack)
                        {
                            StartCoroutine(Atack());
                        }
                    }
                }
            }
            else
            {
                _enemyManager.GoalSelection(this, name);
            }
        }
    }
    private IEnumerator Atack()
    {
        IsAttack = true;
        HeroTarget.Damage(_attackPower, false);
        IMoveMain.StopSpeedAtack(_atackSpeed + _debuffAtackSpeed);
        yield return new WaitForSeconds(_atackSpeed + _debuffAtackSpeed);
        IsAttack = false;
    }
    private void RecordApproac()
    {
        bool elevation = gameObject.layer != 8;
        HexagonControl Hex = _hexagonMain;
        HexagonControl hexagon = Hex.Floor != null ? Hex.Floor : Hex;
        AnApproac.Clear();

        //hexagon.Flag();
        if (IMoveMain.IsFlight())
        {
            if (hexagon.Row != 0)
            {
                HexagonControl hexagonCon = MapControl.MapNav[hexagon.Row - 1, hexagon.Column].GetHexagonMain();

                if (hexagonCon != null)
                    AnApproac.Add(hexagonCon);

                if ((hexagon.Row % 2) == 0)//1
                {
                    if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row - 1, hexagon.Column + 1].GetHexagonMain();
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }
                }
                else
                {
                    if (hexagon.Column > 0)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row - 1, hexagon.Column - 1].GetHexagonMain();
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }

                }
            }

            if (hexagon.Column > 0)
            {
                HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row, hexagon.Column - 1].GetHexagonMain();
                if (hexagonControl != null)
                    AnApproac.Add(hexagonControl);
            }

            if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)
            {
                HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row, hexagon.Column + 1].GetHexagonMain();
                if (hexagonControl != null)
                    AnApproac.Add(hexagonControl);
            }

            if (hexagon.Row < MapControl.MapNav.GetLength(0) - 1)
            {
                HexagonControl hexagonCon = MapControl.MapNav[hexagon.Row + 1, hexagon.Column].GetHexagonMain();
                if (hexagonCon != null)
                    AnApproac.Add(hexagonCon);

                if ((hexagon.Row % 2) == 0)
                {
                    if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row + 1, hexagon.Column + 1].GetHexagonMain();
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }
                }
                else
                {
                    if (hexagon.Column > 0)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row + 1, hexagon.Column - 1].GetHexagonMain();
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);

                    }

                }
            }


        }
        else
        {
            if (hexagon.Row != 0)
            {
                HexagonControl hexagonCon = MapControl.MapNav[hexagon.Row - 1, hexagon.Column].GetHexagonMain(elevation);

                if (hexagonCon != null)
                    AnApproac.Add(hexagonCon);

                if ((hexagon.Row % 2) == 0)//1
                {
                    if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row - 1, hexagon.Column + 1].GetHexagonMain(elevation);
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }
                }
                else
                {
                    if (hexagon.Column > 0)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row - 1, hexagon.Column - 1].GetHexagonMain(elevation);
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }

                }
            }

            if (hexagon.Column > 0)
            {
                HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row, hexagon.Column - 1].GetHexagonMain(elevation);
                if (hexagonControl != null)
                    AnApproac.Add(hexagonControl);
            }

            if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)
            {
                HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row, hexagon.Column + 1].GetHexagonMain(elevation);
                if (hexagonControl != null)
                    AnApproac.Add(hexagonControl);
            }

            if (hexagon.Row < MapControl.MapNav.GetLength(0) - 1)
            {
                HexagonControl hexagonCon = MapControl.MapNav[hexagon.Row + 1, hexagon.Column].GetHexagonMain(elevation);
                if (hexagonCon != null)
                    AnApproac.Add(hexagonCon);

                if ((hexagon.Row % 2) == 0)
                {
                    if (hexagon.Column < MapControl.MapNav.GetLength(1) - 1)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row + 1, hexagon.Column + 1].GetHexagonMain(elevation);
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);
                    }
                }
                else
                {
                    if (hexagon.Column > 0)//2
                    {
                        HexagonControl hexagonControl = MapControl.MapNav[hexagon.Row + 1, hexagon.Column - 1].GetHexagonMain(elevation);
                        if (hexagonControl != null)
                            AnApproac.Add(hexagonControl);

                    }

                }
            }

        }
    }
    private void NewTargetHero(IMove MoveHero)
    {
        HeroControl hero = _enemyManager.GetHero(MoveHero);
        if (hero != null)
        {
            HeroTarget.RemoveEnemy(this);

            hero.AddNewEnemy(this);
            HeroTarget = hero;
            StartWay(hero);

        }
    }
    private void CollisionMain(Vector2 NextPos)
    {
        HexagonControl hex;

        if (IMoveMain.IsFlight())
        {
            hex = MapControl.FieldPositionFly(gameObject.layer, NextPos);
        }
        else
            hex = MapControl.FieldPosition(gameObject.layer, NextPos);

        if (_hexagonMain != hex)
        {
            int PriorityAgr = int.MaxValue;
            IMove hero = null;

            foreach (var Item in hex.ObjAgrDictionary)
            {
                if (Item.Key<PriorityAgr)
                {
                    PriorityAgr = Item.Key;
                    hero = Item.Value;
                }
            }

            if (hero != HeroTarget.IMoveMain)
            {
                NewTargetHero(hero);
            }

            if (!hex.GetFree(IMoveMain.IsFlight()))
            {
                if ((HeroTarget != null) && hex.ObjAbove == HeroTarget.IMoveMain)
                {
                    IMoveMain.StopMoveTarget();
                }
                else
                    IMoveMain.StopMove(hex);
            }
            else
            {
                if (IMoveMain.IsFlight())
                {
                    _hexagonMain.GapFly();
                    _hexagonMain = hex;
                    _hexagonMain.ContactFly(IMoveMain);
                }
                else
                {
                    _hexagonMain.Gap();
                    _hexagonMain = hex;
                    _hexagonMain.Contact(IMoveMain);
                }

                RecordApproac();
                TravelMessage();
            }
        }
    }
    private void TravelMessage()
    {
        for (int i = 0; i < Pursuer.Count; i++)
        {
            Pursuer[i].StartWayEnemy(this);
        }
    }
    public void First(EnemyManager manager)
    {
        //_navigationBot.Control = this;
        _enemyManager = manager;

        if (IMoveMain.IsFlight())
        {
            _hexagonMain = MapControl.FieldPositionFly(gameObject.layer, transform.position);
        }
        else
            _hexagonMain = MapControl.FieldPosition(gameObject.layer, transform.position);

        if (IMoveMain.IsFlight())
        {
            _hexagonMain.ContactFly(IMoveMain);
        }
        else
        {
            _hexagonMain.Contact(IMoveMain);
        }
        RecordApproac();
    }
    public void StartWay(HeroControl hero)
    {
        IMoveMain.StartWay(hero.IControlMain.HexagonMain(), hero.IMoveMain);
    }
    public void AddNewHero(HeroControl hero)
    {
        if (Pursuer.IndexOf(hero) == -1)
        {
            Pursuer.Add(hero);
        }
    }
    public void RemoveHero(HeroControl hero)
    {
        Pursuer.Remove(hero);
    }
    public void Damage(float atack, bool ignoreArmor)
    {
        float Protection = atack * _armor / 100f;

        if (!ignoreArmor)
        {
            _healthPoints -= (atack - Protection);
        }
        else
        {
            _healthPoints -= atack;

        }
    }
    #region Interface
    public void Collision(Vector2 next)
    {
        CollisionMain(next);
    }
    public float GetHealthProcent()
    {
        return _healthPoints / (_healthPointsConst / 100);
    }
    public void CollisionDebuff(Vector2 NextPos)
    {
        HexagonControl hex;

        if (!IMoveMain.IsFlight())
        {
            hex = MapControl.FieldPosition(gameObject.layer, NextPos);
            _debuffHealth = ((_healthPointsConst / 100f) * hex.DebuffHexEnemy.Health) / 50f;
            _debuffAtackSpeed = (_atackSpeed / 100f) * hex.DebuffHexEnemy.AtackSpeed;
            _damagEnvironment = hex.DebuffHexEnemy.Damag / 50f;
           float DeSpeed = (IMoveMain.GetSpeed() / 100f) * hex.DebuffHexEnemy.Speed;

            hex = MapControl.FieldPosition(gameObject.layer, NextPos);
            _debuffHealth += ((_healthPointsConst / 100) * hex.DebuffHex.Health) / 60;
            _debuffAtackSpeed += (_atackSpeed / 100) * hex.DebuffHex.AtackSpeed;
            _damagEnvironment += hex.DebuffHex.Damag / 50f;
            IMoveMain.DebuffSpeed(DeSpeed+((IMoveMain.GetSpeed() / 100) * hex.DebuffHex.Speed));
        }
        else
        {
            hex = MapControl.FieldPosition(gameObject.layer, NextPos);
            _debuffHealth = ((_healthPointsConst / 100f) * hex.DebuffHexEnemyFly.Health) / 50f;
            _damagEnvironment = hex.DebuffHexEnemyFly.Damag / 50f;
            _debuffAtackSpeed = (_atackSpeed / 100f) * hex.DebuffHexEnemyFly.AtackSpeed;
            IMoveMain.DebuffSpeed((IMoveMain.GetSpeed() / 100f) * hex.DebuffHexEnemyFly.Speed);
        }
    }
    public HexagonControl HexagonMain()
    {
        return _hexagonMain;
    }
    public IMove Target()
    {
        return HeroTarget.IMoveMain;
    }
    public List<HexagonControl> GetSurroundingHexes()
    {
        //RecordApproac();
        return AnApproac;
    }

    #endregion
}
