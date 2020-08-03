using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroControl : MonoBehaviour, IControl
{
    [System.Serializable]
    private struct MaxMin
    {
        public float Max;
        public float Min;
    }

    [SerializeField]
    private CanvasManager _canvasManager;
    [SerializeField]
    private Transform _awakePoint;
    [SerializeField]
    private GameObject _shell;
    private EnemyManager _enemyManager;
    private HexagonControl _hexagonMain;
    private List<HexagonControl> _ListHexAgr = new List<HexagonControl>();
    private List<HexagonControl> _ListHexAtack = new List<HexagonControl>();

    private IAbilities _iAbilities;

    [SerializeField]
    private MaxMin _atackPower;
    [SerializeField]
    private float _healthPoints, _atackSpeed, _atackDistens, _powerRegeneration, _agrDistens, _armor;
    private float _atackDistensConst, _healthPointsConst, _regeneration, _agrDistensConst;
    [SerializeField]
    private bool _isLongRangeAttack;
    [SerializeField]
    private int _namberTegAbiliti, _priorityAgr;

    public IMove IMoveMain;
    public IControl IControlMain;

    [HideInInspector]
    public EnemyControl EnemyTarget;
    [HideInInspector]
    public bool IsAttack, IsStopAtack;
    public Animator Animator;

    [HideInInspector]
    public List<HexagonControl> AnApproac = new List<HexagonControl>();
    [HideInInspector]
    public List<EnemyControl> Pursuer = new List<EnemyControl>();

    [SerializeField]
    public float BuffHealth, BuffAtackSpeed, BuffArmor, BuffAtackPower;

    private void Awake()
    {
        IMoveMain = GetComponent<IMove>();
        _iAbilities = GetComponent<IAbilities>();

        transform.position = new Vector3(_awakePoint.position.x, _awakePoint.position.y, transform.position.z);
        _atackDistensConst = (1.73f * (_atackDistens * 2)) + 0.1f;
        _agrDistensConst = (1.73f * (_agrDistens * 2)) + 0.1f;
        _healthPointsConst = _healthPoints;
        _regeneration = (_healthPointsConst * _powerRegeneration / 100f) / 60;
        //_navigationHero.Control = this;

        IControlMain = this;
    }
    private void Update()
    {
        if (_healthPoints <= 0)
        {
            _enemyManager.RemoveHero(this);

            for (int i = 0; i < _ListHexAgr.Count; i++)
            {
                _ListHexAgr[i].ObjAgrDictionary.Remove(_priorityAgr);
            }

            _iAbilities.DethAbility();

            if (EnemyTarget != null)
            {
                EnemyTarget.RemoveHero(this);
            }

            _hexagonMain.Gap();
            _canvasManager.DeactivationAbiliti(_namberTegAbiliti);
            Destroy(gameObject);
        }
    }
    private void FixedUpdate()
    {
        _healthPoints += BuffHealth;

        if (_healthPoints < _healthPointsConst)
        {
            _healthPoints += _regeneration;
        }

        if (_healthPoints > _healthPointsConst)
        {
            _healthPoints = _healthPointsConst;
        }

        if (EnemyTarget == null)
            EnemyTarget = EnemyChoice();


        if (EnemyTarget != null)
        {
            if (((Vector2)EnemyTarget.transform.position - (Vector2)transform.position).magnitude <= _atackDistensConst)
            {
                if (!IsAttack)
                {
                    StartCoroutine(Atack());
                }
            }
        }
    }
    private EnemyControl EnemyChoice()
    {
        for (int i = 0; i < _ListHexAtack.Count; i++)
        {
            if (_ListHexAtack[i].ObjAbove != null && _ListHexAtack[i].ObjAbove.GetObjMain().tag == "Enemy")
            {
                return _ListHexAtack[i].ObjAbove.GetObjMain().GetComponent<EnemyControl>();
            }
            if (_ListHexAtack[i].ObjAboveFly != null && _ListHexAtack[i].ObjAboveFly.GetObjMain().tag == "Enemy")
            {
                return _ListHexAtack[i].ObjAboveFly.GetObjMain().GetComponent<EnemyControl>();
            }
        }
        return null;
    }
    private IEnumerator Atack()
    {
        IsAttack = true;
        _iAbilities.Atack(Random.Range(_atackPower.Min + BuffAtackPower, _atackPower.Max + BuffAtackPower), out float AtackPower, out bool IsIgnotArmor);

        if (_isLongRangeAttack)
        {
            IShell shell = Instantiate(_shell, transform.position, Quaternion.identity).GetComponent<IShell>();
            _iAbilities.AtackСorrection(shell, EnemyTarget, AtackPower, IsIgnotArmor);
        }
        else
        {
            EnemyTarget.Damage(AtackPower, IsIgnotArmor);
        }

        //IMoveMain.StopSpeedAtack(0.5f);
        yield return new WaitForSeconds(_iAbilities.AtackSpeed(_atackSpeed + BuffAtackSpeed));
        IsAttack = false;
    }
    private void RecordApproac()
    {
        bool elevation = gameObject.layer != 8;
        HexagonControl Hex = _hexagonMain;
        HexagonControl hexagon = Hex.Floor != null ? Hex.Floor : Hex;
        AnApproac.Clear();

        //hexagon.Flag();

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
    private void CollisionMain(Vector2 NextPos)
    {
        HexagonControl hex = MapControl.FieldPosition(gameObject.layer, NextPos);
        if (_hexagonMain != hex)
        {
            if (!hex.IsFree)
            {
                if ((EnemyTarget != null) && hex.ObjAbove == EnemyTarget.IMoveMain)
                {
                    IMoveMain.StopMoveTarget();
                }
                else
                    IMoveMain.StopMove(hex);
            }
            else
            {
                if (hex.TypeHexagon == 1)
                {
                    Debug.LogError("da");
                }

                _hexagonMain.Gap();
                _hexagonMain = hex;
                _hexagonMain.Contact(IMoveMain);
                RecordApproac();
                TravelMessage();
            }
        }
    }
    private void TravelMessage()
    {
        for (int i = 0; i < Pursuer.Count; i++)
        {
            Pursuer[i].StartWay(this);
        }
    }
    public void DisConectEnemy()
    {
        if (EnemyTarget != null)
        {
            EnemyTarget.RemoveHero(this);
            EnemyTarget = null;
        }
    }
    public void StartWayEnemy(EnemyControl enemy)
    {
        EnemyTarget = enemy;
        enemy.AddNewHero(this);
        IMoveMain.StartWay(enemy.IControlMain.HexagonMain(), enemy.IMoveMain);
    }
    public void StartWay(HexagonControl hexagonFinish)
    {
        IMoveMain.StartWay(hexagonFinish, null);
    }
    public void Initialization(EnemyManager enemyManager)
    {
        _enemyManager = enemyManager;
    }
    public void AddNewEnemy(EnemyControl enemy)
    {
        if (Pursuer.IndexOf(enemy) == -1)
        {
            Pursuer.Add(enemy);
        }
    }
    public void RemoveEnemy(EnemyControl enemy)
    {
        Pursuer.Remove(enemy);
    }
    public void InstallHero()
    {

        HexagonControl hex = MapControl.FieldPositionMapBeforeStart(gameObject.layer, transform.position);

        if (hex == null || _enemyManager.CheckHero())
        {
            transform.position = new Vector3(_awakePoint.position.x, _awakePoint.position.y, transform.position.z);
            _hexagonMain = null;
        }
        else
        {
            _hexagonMain = hex;
            _enemyManager.EnterTheGame(this);

            _hexagonMain = _hexagonMain.GetHexagonMain();
            transform.position = _hexagonMain.position;
            if (_hexagonMain.TypeHexagon == 2)
            {
                gameObject.layer = 11;
            }
            else if (_hexagonMain.TypeHexagon == 0)
            {
                gameObject.layer = 8;
            }

            _hexagonMain.Contact(IMoveMain);
        }
    }
    public void MoveTheHero()
    {
        if (_hexagonMain != null)
        {
            _enemyManager.EnteringTheGame(this);
            _hexagonMain.Gap();
        }
    }
    public void ActivationAbilities()
    {
        Image image;
        _canvasManager.ActivationAbiliti(_namberTegAbiliti, this, out image);
        _iAbilities.Initialization(image);
    }
    public void StartGame()
    {
        _hexagonMain = MapControl.FieldPosition(gameObject.layer, transform.position);
        _hexagonMain.Contact(IMoveMain);

        List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        Physics2D.CircleCast(transform.position, _agrDistensConst, Vector2.zero, contactFilter2D, hits2D);

        for (int i = 0; i < hits2D.Count; i++)
        {
            HexagonControl hex = hits2D[i].collider.GetComponent<HexagonControl>().GetHexagonMain();
            if (hex != null && hex.TypeHexagon != 1 && !_ListHexAgr.Contains(hex))
            {
                _ListHexAgr.Add(hex);
                hex.ObjAgrDictionary[_priorityAgr] = IMoveMain;
            }
        }

        hits2D.Clear();
        Physics2D.CircleCast(transform.position, _atackDistensConst, Vector2.zero, contactFilter2D, hits2D);

        for (int i = 0; i < hits2D.Count; i++)
        {
            HexagonControl hex = hits2D[i].collider.GetComponent<HexagonControl>().GetHexagonMain();

            if (hex != null && (((Vector2)transform.position - hex.position).magnitude <= _atackDistensConst) && !_ListHexAtack.Contains(hex))
            {
                _ListHexAtack.Add(hex);
            }
        }

        _iAbilities.StartAbility();

        RecordApproac();

        CollisionDebuff(transform.position);
    }
    public void ChangeOfPosition()
    {
        for (int i = 0; i < _ListHexAgr.Count; i++)
        {
            _ListHexAgr[i].ObjAgrDictionary.Remove(_priorityAgr);
        }
        _ListHexAgr.Clear();
        _ListHexAtack.Clear();
        _iAbilities.DethAbility();

        _hexagonMain.Gap();

        StartGame();

        TravelMessage();
    }
    public float GetAtackPowePrercent()
    {
        return ((_atackPower.Max + _atackPower.Min) / 2f) / 100f;
    }
    public void CircularAttack(float boostAtack, bool IsIgnotArmor)
    {
        for (int i = 0; i < AnApproac.Count; i++)
        {
            if (AnApproac[i].ObjAbove!=null && AnApproac[i].ObjAbove.GetObjMain().tag == "Enemy")
            {
                EnemyControl enemy = AnApproac[i].ObjAbove.GetObjMain().GetComponent<EnemyControl>();

                float AtackPower = Random.Range(_atackPower.Min + BuffAtackPower, _atackPower.Max + BuffAtackPower) * boostAtack;

                if (_isLongRangeAttack)
                {
                    IShell shell = Instantiate(_shell, transform.position, Quaternion.identity).GetComponent<IShell>();
                    _iAbilities.AtackСorrection(shell, enemy, AtackPower, IsIgnotArmor);
                }
                else
                {
                    enemy.Damage(AtackPower, IsIgnotArmor);
                }
            }
            if (AnApproac[i].ObjAboveFly != null && AnApproac[i].ObjAboveFly.GetObjMain().tag == "Enemy")
            {
                EnemyControl enemy = AnApproac[i].ObjAboveFly.GetObjMain().GetComponent<EnemyControl>();

                float AtackPower = Random.Range(_atackPower.Min + BuffAtackPower, _atackPower.Max + BuffAtackPower) * boostAtack;

                if (_isLongRangeAttack)
                {
                    IShell shell = Instantiate(_shell, transform.position, Quaternion.identity).GetComponent<IShell>();
                    _iAbilities.AtackСorrection(shell, enemy, AtackPower, IsIgnotArmor);
                }
                else
                {
                    enemy.Damage(AtackPower, IsIgnotArmor);
                }

            }
        }
    }

    #region Health
    public float GetRegeneration()
    {
        return _regeneration;
    }
    public void AdditionalTreatment(float pointHeal)
    {
        _healthPoints += pointHeal;

        if (_healthPoints > _healthPointsConst)
        {
            _healthPoints = _healthPointsConst;
        }
    }
    public float GetHealthProcent()
    {
        return _healthPoints / (_healthPointsConst / 100);
    }
    public float GetMaxHealth()
    {
        return _healthPoints;
    }
    public void Damage(float atack, bool ignoreArmor)
    {
        float Protection = atack * (_armor + BuffArmor) / 100;

        if (!ignoreArmor)
        {
            _healthPoints -= _iAbilities.Armor(atack - Protection);
        }
        else
        {
            _healthPoints -= _iAbilities.Armor(atack);
        }
    }

    #endregion

    #region Interface
    public void Collision(Vector2 next)
    {
        CollisionMain(next);
    }
    public void CollisionDebuff(Vector2 NextPos)
    {
        HexagonControl hex;

        hex = MapControl.FieldPosition(gameObject.layer, NextPos);

        BuffHealth = ((_healthPointsConst / 100) * hex.DebuffHexHero.Health) / 60;
        BuffAtackSpeed = (_atackSpeed / 100) * hex.DebuffHexHero.AtackSpeed;
        float DeSpeed = (IMoveMain.GetSpeed() / 100f) * hex.DebuffHexHero.Speed;

        hex = MapControl.FieldPosition(gameObject.layer, NextPos);
        BuffHealth += ((_healthPointsConst / 100) * hex.DebuffHex.Health) / 60;
        BuffAtackSpeed += (_atackSpeed / 100) * hex.DebuffHex.AtackSpeed;
        IMoveMain.DebuffSpeed(DeSpeed + ((IMoveMain.GetSpeed() / 100) * hex.DebuffHex.Speed));

    }
    public HexagonControl HexagonMain()
    {
        return _hexagonMain;
    }

    public IMove Target()
    {
        if (EnemyTarget == null)
        {
            return null;
        }
        else
            return EnemyTarget.IMoveMain;
    }
    public List<HexagonControl> GetSurroundingHexes()
    {
        //RecordApproac();
        return AnApproac;
    }
    #endregion
}
