using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireMage : MonoBehaviour, IAbilities
{
    private Image _imageAbiliti;

    [SerializeField]
    private float _extraDamage, _timeExtraDamage;
    private float _timeExtraDamageConst, _fillAmountTime;
    private bool _boostDamage;

    void Start()
    {
        _timeExtraDamageConst = _timeExtraDamage;
        _timeExtraDamage = 0;
        _fillAmountTime = Time.fixedDeltaTime / _timeExtraDamageConst;
    }
    void FixedUpdate()
    {
        if (StaticLevelManager.IsGameFlove && _imageAbiliti != null)
        {
            if (_timeExtraDamage <= 0 && !_boostDamage)
            {
                _boostDamage = true;
            }

            if (_timeExtraDamage > 0)
            {
                _timeExtraDamage -= Time.deltaTime;
                _imageAbiliti.fillAmount -= _fillAmountTime;
            }
        }
    }
    public float Armor(float DamagPower)
    {
        return DamagPower;
    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor, Vector3 posTarget)
    {
        Atack = AtackPower;
        ignoreArmor = false;
    }
    public float AtackSpeed()
    {
        return 1;
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
            _imageAbiliti.fillAmount = 1;
        }
        else
            shell.Initialization(enemy, damag, isIgnorArmor);

    }
    public void Initialization(Image image)
    {
        _imageAbiliti = image;
        _imageAbiliti.fillAmount = 0;
    }

}
