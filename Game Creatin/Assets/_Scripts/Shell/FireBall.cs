using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IShell
{
    [SerializeField]
    private GameObject _fair;
    [SerializeField]
    private float _fireRange;
    private EnemyControl _target;

    private float _damag;
    private bool _isIgnotArmor;
    private void Start()
    {
        _fireRange = (1.73f * (_fireRange * 2)) + 0.1f;
    }
    void FixedUpdate()
    {
        if (((Vector2)transform.position - (Vector2)_target.transform.position).magnitude >= 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _target.transform.position, 2);
        }
        else
        {
            List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
            ContactFilter2D contactFilter2D = new ContactFilter2D();
            contactFilter2D.useTriggers = true;
            //CastFair();
            Physics2D.CircleCast(transform.position, _fireRange, Vector2.zero, contactFilter2D, hits2D);
            for (int i = 0; i < hits2D.Count; i++)
            {
                EnemyControl enemy = hits2D[i].collider.GetComponent<EnemyControl>();
                if (enemy != null)
                {
                    enemy.Damage(_damag, _isIgnotArmor);
                }
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
        _fireRange += 1;
    }
    public void CastFair()
    {
        int Angles = 0;
        for (int i = 0; i < 8; i++)
        {
            FairGo fair = Instantiate(_fair, transform.position, Quaternion.identity).GetComponent<FairGo>();
            fair.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Angles));
            Vector2 pos = fair.transform.position;
            fair.transform.Translate(Vector2.right * _fireRange);
            fair.Target = fair.transform.position;
            fair.transform.position = pos;
            Angles += 45;
        }
    }
}
