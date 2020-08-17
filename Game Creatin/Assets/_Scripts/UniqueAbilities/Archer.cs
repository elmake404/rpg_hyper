using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour, IAbilities
{
    [SerializeField]
    private GameObject _crit;

    private Image _imageAbiliti;
    private List<bool> _listProtectionOptionsCrit = new List<bool>();

    [SerializeField]
    private float _critPower, _timeBoost, _timeForBoost;
    private float _timeForBoostConst, _timeBoostConst, _fillAmountTime;
    [SerializeField]
    private int _critСhance, _attackAcceleration;
    private bool _boostSpeed, _activationBoost;

    void Start()
    {
        _timeForBoostConst = _timeForBoost;
        _timeBoostConst = _timeBoost;
        _timeForBoost = 0;
        _fillAmountTime = Time.fixedDeltaTime / _timeForBoostConst;
        RandomFilling();
    }
    void FixedUpdate()
    {
        if (StaticLevelManager.IsGameFlove && _imageAbiliti != null)
        {
            if (_timeForBoost <= 0 && !_boostSpeed)
            {
                _boostSpeed = true;
            }

            if (_timeForBoost > 0)
            {
                _timeForBoost -= Time.deltaTime;
                _imageAbiliti.fillAmount -= _fillAmountTime;
            }

            if (_activationBoost)
            {
                _timeBoost -= Time.deltaTime;
                if (_timeBoost <= 0)
                {
                    _activationBoost = false;
                    _timeBoost = _timeBoostConst;
                }
            }
        }
    }
    private void RandomFilling()
    {
        int Positiv = 0;
        for (int i = 0; i < 100; i++)
        {
            _listProtectionOptionsCrit.Add(false);
        }

        List<int> Positivelement = new List<int>();

        while (Positiv < _critСhance)
        {
            int possibility = Random.Range(0, _listProtectionOptionsCrit.Count);
            if (!Positivelement.Contains(possibility))
            {
                _listProtectionOptionsCrit[possibility] = true;
                Positiv++;
            }
        }
    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor, Vector3 posTarget)
    {
        ignoreArmor = false;
        int rnd = Random.Range(0, 100);

        if (_listProtectionOptionsCrit[rnd])
        {
            Vector3 vector = new Vector3(0, 1.5f, -4);
            Instantiate(_crit, posTarget + vector, Quaternion.identity);

            Atack = AtackPower * (_critPower / 100);
        }
        else
        {
            Atack = AtackPower;
        }
    }
    public float Armor(float DamagPower)
    {
        return DamagPower;
    }
    public float AtackSpeed()
    {
        if (_boostSpeed)
        {
            _activationBoost = true;
            _boostSpeed = false;
            _timeForBoost = _timeForBoostConst;
            _imageAbiliti.fillAmount = 1;
        }

        if (_activationBoost)
        {
            return  _attackAcceleration;
        }
        else
        {
            return 1;
        }
    }
    public void StartAbility()
    {
    }
    public void DethAbility()
    {
    }
    public void AtackСorrection(IShell shell, EnemyControl enemy, float damag, bool isIgnorArmor)
    {
        shell.Initialization(enemy, damag, isIgnorArmor);
    }
    public void Initialization(Image image)
    {
        _imageAbiliti = image;
        _imageAbiliti.fillAmount = 0;
    }

}
