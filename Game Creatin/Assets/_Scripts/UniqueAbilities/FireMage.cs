using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMage : MonoBehaviour, IAbilities
{
    private CanvasManager _canvasManager;

    [SerializeField]
    private float _extraDamage, _timeExtraDamage;
    private float _timeExtraDamageConst;
    private bool _boostDamage;

    void Start()
    {
        _timeExtraDamageConst = _timeExtraDamage;
    }

    void FixedUpdate()
    {
        if (StaticLevelManager.IsGameFlove)
        {
            if (_timeExtraDamage <= 0 && !_boostDamage)
            {
                _boostDamage = true;
            }

            if (_timeExtraDamage > 0)
            {
                _timeExtraDamage -= Time.deltaTime;
            }
        }
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
    }
    public void DethAbility()
    {
    }
    public void AtackСorrection(IShell shell, EnemyControl enemy, float damag, bool isIgnorArmor)
    {
        if (_boostDamage)
        {
            _boostDamage = false;

            shell.Initialization(enemy, damag * (_extraDamage / 100), isIgnorArmor);
            shell.ShellAbility();
            _timeExtraDamage = _timeExtraDamageConst;
        }
        else
            shell.Initialization(enemy, damag, isIgnorArmor);

    }
    public void Initialization(CanvasManager canvasManager)
    {
        _canvasManager = canvasManager;
    }

}
