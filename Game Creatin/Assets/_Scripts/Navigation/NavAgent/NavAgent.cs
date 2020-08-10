using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NavAgent : MonoBehaviour, IMove
{
    private List<HexagonControl> _wayList = new List<HexagonControl>();
    private HexagonControl _targetHexagon;
    private HexagonControl _currentPos;
    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private bool _isMove, _isClever;
    private bool _isBypass;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _speedMove, _debuffSpeed;

    public IControl Control;
    private void Awake()
    {
        Control = GetComponent<IControl>();
        _speedMove = _speed;
        _isBypass = true;
    }
    private void FixedUpdate()
    {
        if (_animator != null)
        {
            if (!_animator.GetBool("Go") && _isMove)
            {
                _animator.SetBool("Go", _isMove);
            }
            else if (_animator.GetBool("Go") && !_isMove)
            {
                _animator.SetBool("Go", _isMove);
            }
        }

        if (_wayList.Count <= 0)
        {
            _isMove = false;
        }


        if (StaticLevelManager.IsGameFlove)
        {
            Control.CollisionDebuff(transform.position);

            if (_isMove)
            {
                _targetHexagon = _wayList[0];

                if (_targetHexagon.TypeHexagon == 2 && gameObject.layer == 8)
                {
                    gameObject.layer = 11;
                }
                else if (((_targetHexagon.TypeHexagon == 3 && HexagonMain().TypeHexagon == 3) || _targetHexagon.TypeHexagon == 0)
                          && gameObject.layer == 11)

                {
                    gameObject.layer = 8;
                }

                transform.position = Vector2.MoveTowards(transform.position, _targetHexagon.transform.position, _speedMove + _debuffSpeed);

                Vector2 NextPos = (Vector2)transform.position + (Vector2)(_targetHexagon.transform.position - transform.position).normalized * 1.8f;

                Control.Collision(NextPos);

                if (_wayList.Count > 0)
                {
                    if (((Vector2)transform.position - (Vector2)_wayList[0].transform.position).magnitude <= 0.01f)
                    {
                        _wayList.Remove(_wayList[0]);
                    }
                }
            }
            else
            {
                if (((Vector2)transform.position - Control.HexagonMain().position).magnitude >= 0.01f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, Control.HexagonMain().position, _speed + _debuffSpeed);
                }

                if (_isBypass)
                {
                    if (_wayList.Count > 0)
                    {
                        IMove move = _currentPos.ObjAbove;
                        if ((move == null))
                        {
                            if (_currentPos.IsFree)
                            {
                                _isMove = true;
                            }
                        }
                        else if (!move.IsGo())
                        {
                            IMove enemy = Control.Target() != null ? Control.Target() : null;
                            WayBypass(_wayList[_wayList.Count - 1], enemy, _currentPos.ObjAbove);
                        }
                    }
                }
            }
        }
    }
    private IEnumerator StopSpeed(float time)
    {
        _speedMove = 0;
        yield return new WaitForSeconds(time);
        _speedMove = _speed;
    }
    private IEnumerator StopBypass()
    {
        _isBypass = false;
        yield return new WaitForSeconds(0.5f);
        _isBypass = true;
    }
    private void Way(HexagonControl hexagonFinish, IMove EnemyTarget)
    {
        List<HexagonControl> Way;

        Way = Control.HexagonMain().GetWay(hexagonFinish);

        if (Way != null)
        {
            if (_isClever)
            {
                Way = NavStatic.PathCheck(Way, EnemyTarget, this);
            }
            else
            {
                Way.Remove(Way[0]);
            }

            _wayList.AddRange(Way);
            _isMove = true;
        }

    }
    private void WayBypass(HexagonControl hexagonFinish, IMove EnemyTarget, IMove Collision)
    {
        List<HexagonControl> Way;

        Way = Control.HexagonMain().GetWay(hexagonFinish);

        if (Way != null)
        {
            Way = NavStatic.PathCheckBypass(Way, Collision, EnemyTarget, this);

            if (Way.Count > 0)
            {
                _wayList.Clear();

                _wayList.AddRange(Way);
                _isMove = true;
            }
            else
            {
                StartCoroutine(StopBypass());
            }
        }

    }

    #region interface 
    public void DebuffSpeed(float debuff)
    {
        if (debuff < -_speedMove)
        {
            _debuffSpeed = -_speedMove;
        }
        else
        {
            _debuffSpeed = debuff;
        }
    }
    public float GetSpeed()
    {
        return _speed;
    }
    public void StartWay(HexagonControl hexagonFinish, IMove EnemyTarget)
    {
        _wayList.Clear();


        Way(hexagonFinish, EnemyTarget);
    }
    public void StopMove(HexagonControl CollcionHex)
    {
        _currentPos = CollcionHex;
        _isMove = false;
    }
    public void StopMoveTarget()
    {
        _isMove = false;
        _wayList.Clear();
    }
    public void StopSpeedAtack(float timeStop)
    {
        StopMoveTarget();
        StartCoroutine(StopSpeed(timeStop));
    }
    public bool IsGo()
    {
        return _isMove;
    }
    public EnemyControl GetEnemy()
    {
        return null;
    }
    public List<HexagonControl> GetSurroundingHexes()
    {
        return Control.GetSurroundingHexes();
    }
    public bool FreeSpaceCheck(bool Flight)
    {
        bool free = false;
        List<HexagonControl> hexagons = Control.GetSurroundingHexes();
        if (Flight)
        {
            for (int i = 0; i < hexagons.Count; i++)
            {
                if (hexagons[i].IsFreeFly)
                {
                    free = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < hexagons.Count; i++)
            {
                if (hexagons[i].IsFree)
                {
                    free = true;
                    break;
                }
            }
        }

        return free;
    }
    public bool IsFlight()
    {
        return false;
    }
    public GameObject GetObjMain()
    {
        return gameObject;
    }
    public HexagonControl HexagonMain()
    {
        return Control.HexagonMain();
    }
    #endregion
}
