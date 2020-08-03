using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Murderer : MonoBehaviour, IAbilities
{
    private Image _imageAbiliti;

    private bool _isCriticalHit;
    [SerializeField]
    private float _critPower, _timeForCrit;
    private float _timeForCritConst, _fillAmountTime;
    void Start()
    {
        _timeForCritConst = _timeForCrit;
        _timeForCrit = 0;
        _fillAmountTime = Time.fixedDeltaTime / _timeForCritConst;
    }
    void FixedUpdate()
    {
        if (StaticLevelManager.IsGameFlove && _imageAbiliti != null)
        {
            if (_timeForCrit <= 0 && !_isCriticalHit)
            {
                _isCriticalHit = true;
            }

            if (_timeForCrit > 0)
            {
                _timeForCrit -= Time.deltaTime;
                _imageAbiliti.fillAmount -= _fillAmountTime;
            }
        }
    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor)
    {
        if (_isCriticalHit)
        {
            ignoreArmor = true;
            _isCriticalHit = false;
            Atack = AtackPower * (_critPower / 100);

            _imageAbiliti.fillAmount = 1;
            _timeForCrit = _timeForCritConst;
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
    public void Initialization(Image image)
    {
        _imageAbiliti = image;
        _imageAbiliti.fillAmount = 0;
    }

}
