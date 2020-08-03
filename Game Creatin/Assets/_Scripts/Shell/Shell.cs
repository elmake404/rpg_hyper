using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IShell
{
    private EnemyControl _target;

    private float _damag;
    private bool _isIgnotArmor;

    void Start()
    {

    }
    void FixedUpdate()
    {
        if (((Vector2)transform.position - _target.HexagonMain().position).magnitude >= 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.HexagonMain().position, 2);
        }
        else
        {
            if (_target!=null)
            {
                _target.Damage(_damag, _isIgnotArmor);
            }
            Destroy(gameObject);
        }
    }
    public void Initialization(EnemyControl enemy, float damag, bool isIgnorArmor)
    {
        _target = enemy;
        _damag = damag;
        _isIgnotArmor = isIgnorArmor;
    }
    public void ShellAbility()
    {
    }
}
