using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour, IShell
{
    private EnemyControl _target;

    private float _damag;
    private bool _isIgnotArmor;

    void FixedUpdate()
    {
        if (((Vector2)transform.position - (Vector2)_target.transform.position).magnitude >= 0.2f)
        {
            Quaternion rot = Quaternion.LookRotation(_target.transform.position - transform.position);
            rot.eulerAngles = new Vector3(0, 0, -rot.eulerAngles.x);
            transform.rotation = rot;
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, 2);
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
