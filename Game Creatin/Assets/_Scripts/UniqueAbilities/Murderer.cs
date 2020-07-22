using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Murderer : MonoBehaviour, IAbilities
{
    private CanvasManager _canvasManager;

    private bool _isCriticalHit;
    [SerializeField]
    private float _critPower, _timeForCrit;
    private float _timeForCritConst;
    void Start()
    {
        _timeForCritConst = _timeForCrit;
    }
    void FixedUpdate()
    {
        if (_timeForCrit <= 0 && !_isCriticalHit)
        {
            _isCriticalHit = true;
            _timeForCrit = _timeForCritConst;
        }

        if (_timeForCrit > 0)
        {
            _timeForCrit -= Time.deltaTime;
        }
    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor)
    {

        if (_isCriticalHit)
        {
            ignoreArmor = true;

            _isCriticalHit = false;

            Atack = AtackPower * (_critPower / 100);
        }
        else
        {
            ignoreArmor = false;

            Atack = AtackPower;
        }
    }
    public float Armor(float DamagPower)
    {
        return DamagPower;
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
    }
    public void Initialization(CanvasManager canvasManager)
    {
        _canvasManager = canvasManager;
    }

}
