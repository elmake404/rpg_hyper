using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour, IAbilities
{
    private CanvasManager _canvasManager;

    private bool _isCriticalHit;
    [SerializeField]
    private float _critPower, _timeForCrit;
    private float _timeForCritConst;
    [SerializeField]
    private int _blockСhance;
    private List<bool> _listProtectionOptionsArmor = new List<bool>();
    void Start()
    {
        _timeForCritConst = _timeForCrit;
        RandomFilling();
    }
    void FixedUpdate()
    {
        if (_timeForCrit <= 0 && !_isCriticalHit)
        {
            _isCriticalHit = true;
            _timeForCrit = _timeForCritConst;
        }

        if (_timeForCrit>0)
        {
            _timeForCrit -= Time.deltaTime;
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
    public void Initialization(CanvasManager canvasManager)
    {
        _canvasManager = canvasManager;
    }

}
