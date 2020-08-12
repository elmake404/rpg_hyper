using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquallOfArrows : MonoBehaviour, IActiveAbility
{
    private Image _imageAbiliti;
    private HeroControl _heroControl;
    private HexagonControl _hexagonCast;
    private List<HexagonControl> _listHexagonCast = new List<HexagonControl>();
    private Color _oldColor;

    [SerializeField]
    private float _cooldown, _damageProcent, _duration, _fireRange;
    private float _width, _height, _cooldownConst, _durationConst, _fillAmountTime, _damage;
    private bool _isCast,_isActivaAbiliti, _isReady;

    void Start()
    {
        _oldColor = MapControl.MapNav[0, 0].Sprite.color;

        _isReady = true;
        _fireRange = (1.73f * (_fireRange * 2)) + 0.1f;
        _damage = _heroControl.GetAtackPowePrercent()* _damageProcent;

        _durationConst = _duration;
        _duration = 0;

        _cooldownConst = _cooldown;
        _cooldown = 0;

        _fillAmountTime = Time.fixedDeltaTime / _cooldownConst;

        RectTransform rectTransform = GetComponent<RectTransform>();
        _width = rectTransform.rect.width / 2;
        _height = rectTransform.rect.height / 2;
    }

    void Update()
    {
        if (_isReady)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Mathf.Abs(transform.position.x - Input.mousePosition.x) <= _width
                    && Mathf.Abs(transform.position.y - Input.mousePosition.y) <= _height)
                {
                    _isCast = true;
                }
            }
            else if (_isCast && Input.GetMouseButton(0))
            {
                Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3);
                Vector3 posWorold = Camera.main.ScreenToWorldPoint(pos);
                HexagonControl hex = MapControl.FieldPositionMapCast(posWorold);

                if (hex != null && hex != _hexagonCast)
                {
                    for (int i = 0; i < _listHexagonCast.Count; i++)
                    {
                        _listHexagonCast[i].Sprite.color = _oldColor;
                    }
                    _listHexagonCast.Clear();

                    List<RaycastHit2D> hits2D = new List<RaycastHit2D>();
                    ContactFilter2D contactFilter2D = new ContactFilter2D();

                    Physics2D.CircleCast(hex.position, _fireRange, Vector2.zero, contactFilter2D, hits2D);
                    for (int i = 0; i < hits2D.Count; i++)
                    {
                        HexagonControl hexagon = hits2D[i].collider.GetComponent<HexagonControl>();
                        if (!_listHexagonCast.Contains(hexagon.GetHexagonMain())
                            && (hex.position - hexagon.GetArrayElement().position).magnitude <= _fireRange)
                        {
                            _listHexagonCast.Add(hexagon.GetHexagonMain());
                            hexagon.GetHexagonMain().Sprite.color = new Color(0.1154f, 0.3490566f, 0.07409222f, 1);
                        }
                    }

                    _hexagonCast = hex;
                }

                if (hex == null)
                {
                    for (int i = 0; i < _listHexagonCast.Count; i++)
                    {
                        _listHexagonCast[i].Sprite.color = _oldColor;
                    }
                    _hexagonCast = null;

                    _listHexagonCast.Clear();
                }

            }
            else if (_isCast && Input.GetMouseButtonUp(0))
            {
                if (_listHexagonCast.Count > 0)
                {
                    _isActivaAbiliti=true;
                    _duration = _durationConst;
                    _isReady = false;
                    _imageAbiliti.fillAmount = 1;
                    _cooldown = _cooldownConst;
                    for (int i = 0; i < _listHexagonCast.Count; i++)
                    {
                        _listHexagonCast[i].Sprite.color = new Color(0, 1, 1, 1);
                        _listHexagonCast[i].DebuffHexEnemy.Damag += _damage;
                        _listHexagonCast[i].DebuffHexEnemyFly.Damag += _damage;
                    }
                }

                _hexagonCast = null;
                _isCast = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (_cooldown <= 0 && !_isReady)
        {
            _isReady = true;
        }

        if (_cooldown > 0)
        {
            _imageAbiliti.fillAmount -= _fillAmountTime;
            _cooldown -= Time.fixedDeltaTime;
        }
        if (_isActivaAbiliti)
        {
            if (_duration>0)
            {
                _duration -= Time.fixedDeltaTime;
            }
            else
            {
                _isActivaAbiliti = false;
                for (int i = 0; i < _listHexagonCast.Count; i++)
                {
                    _listHexagonCast[i].Sprite.color = _oldColor;
                    _listHexagonCast[i].DebuffHexEnemy.Damag -= _damage;
                    _listHexagonCast[i].DebuffHexEnemyFly.Damag -= _damage;
                }

            }
        }

    }
    public void initializationHero(HeroControl hero, Image img)
    {
        _heroControl = hero;
        _imageAbiliti = img;
    }

}
