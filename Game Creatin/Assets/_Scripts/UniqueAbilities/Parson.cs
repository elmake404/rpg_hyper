using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parson : MonoBehaviour, IAbilities
{
    [SerializeField]
    private float _healthRecoveryPercentage, _timeForHeal, _ranjeAura;
    private float _timeForHealConst;
    private CanvasManager _canvasManager;
    private EnemyManager _manager;
    private List<HexagonControl> _ListHexAura = new List<HexagonControl>();
    private List<Heal> _ListHexHeal = new List<Heal>();

    void Start()
    {
        _manager = FindObjectOfType<EnemyManager>();
        _timeForHealConst = _timeForHeal;
    }

    void FixedUpdate()
    {
        if (StaticLevelManager.IsGameFlove)
        {
            if (_timeForHeal <= 0)
            {
                Heal();
                _timeForHeal = _timeForHealConst;
            }
            else
            {
                _timeForHeal -= Time.deltaTime;
            }
        }
    }

    private void Heal()
    {
        HeroControl hero = _manager.GetHeroWithMinimalHealth();
        hero.AdditionalTreatment((hero.GetMaxHealth() / 100f) * _healthRecoveryPercentage);
    }

    public float Armor(float DamagPower)
    {
        return DamagPower;
    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor)
    {
        Atack = AtackPower;
        ignoreArmor = false;
    }
    public float AtackSpeed(float Speed)
    {
        return Speed;
    }
    public void StartAbility()
    {
        _ranjeAura = (1.73f * (_ranjeAura * 2)) + 0.1f;

        List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        Physics2D.CircleCast(transform.position, _ranjeAura, Vector2.zero, contactFilter2D, hits2D);

        for (int i = 0; i < hits2D.Count; i++)
        {
            HexagonControl hex = hits2D[i].collider.GetComponent<HexagonControl>().GetHexagonMain();
            if (hex != null && hex.TypeHexagon != 1 && !_ListHexAura.Contains(hex))
            {
                _ListHexAura.Add(hex);
                Heal heal = hex.gameObject.AddComponent<Heal>();
                _ListHexHeal.Add(heal);
                heal.Initialization(hex, _manager);
            }
        }
    }
    public void DethAbility()
    {
        for (int i = 0; i < _ListHexHeal.Count; i++)
        {
            _ListHexHeal[i].Delete();
        }
    }
    public void AtackСorrection(IShell shell, EnemyControl enemy, float damag, bool isIgnorArmor)
    {
        shell.Initialization(enemy, damag, isIgnorArmor);
    }

    public void Initialization(CanvasManager canvasManager)
    {
        _canvasManager = canvasManager;
    }
}
