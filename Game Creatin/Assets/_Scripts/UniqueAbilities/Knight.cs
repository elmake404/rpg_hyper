using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : MonoBehaviour, IAbilities
{
    private Image _imageAbiliti;

    private bool _isCriticalHit;
    [SerializeField]
    private float _critPower, _timeForCrit;
    private float _timeForCritConst, _fillAmountTime;
    [SerializeField]
    private int _blockСhance;
    private List<bool> _listProtectionOptionsArmor = new List<bool>();
    void Start()
    {
        _timeForCritConst = _timeForCrit;
        _timeForCrit = 0;
        _fillAmountTime = Time.fixedDeltaTime / _timeForCritConst;
        RandomFilling();
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
    private void RandomFilling()
    {
        int Positiv = 0;
        for (int i = 0; i < 100; i++)
        {
            _listProtectionOptionsArmor.Add(false);
        }

        List<int> Positivelement = new List<int>();

        while (Positiv < _blockСhance)
        {
            int possibility = Random.Range(0, _listProtectionOptionsArmor.Count);
            if (!Positivelement.Contains(possibility))
            {
                _listProtectionOptionsArmor[possibility] = true;
                Positiv++;
            }
        }

    }
    public void Atack(float AtackPower, out float Atack, out bool ignoreArmor)
    {
        ignoreArmor = false;

        if (_isCriticalHit)
        {
            _isCriticalHit = false;

            Atack = AtackPower * (_critPower / 100);
            _timeForCrit = _timeForCritConst;
            _imageAbiliti.fillAmount = 1;
        }
        else
        {
            Atack = AtackPower;
        }
    }
    public float Armor(float DamagPower)
    {
        int rnd = Random.Range(0,100);
        if (_listProtectionOptionsArmor[rnd])
        {
            return 0;
        }
        else
        {
            return DamagPower;
        }
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
