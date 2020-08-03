using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeapOfFaith : MonoBehaviour, IActiveAbility
{
    private Image _imageAbiliti;
    private HeroControl _heroControl;
    private HexagonControl _hexagonCast;

    [SerializeField]
    private float  _cooldown,_damagBust;
    private float _width, _height, _cooldownConst, _fillAmountTime;
    private bool _isCast, _isReady;
    void Start()
    {
        _cooldownConst = _cooldown;
        _cooldown = 0;
        _fillAmountTime = Time.fixedDeltaTime / _cooldownConst;

        _isReady = true;

        RectTransform rectTransform = GetComponent<RectTransform>();
        _width = rectTransform.rect.width / 2;
        _height = rectTransform.rect.height / 2;
    }

    void Update()
    {
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

                    if (hex != null )
                    {
                        if (_hexagonCast != null)
                            _hexagonCast.GetHexagonMain().Sprite.color = new Color(1, 1, 1, 1);

                        _hexagonCast = hex;

                        if (_hexagonCast.IsFree || _hexagonCast.TypeHexagon == 1)
                        {
                            _hexagonCast.GetHexagonMain().Sprite.color = new Color(0.1154f, 0.3490566f, 0.07409222f, 1);
                        }
                        else
                        {
                            _hexagonCast.GetHexagonMain().Sprite.color = new Color(1, 0, 0, 1);
                        }
                    }

                    if (hex == null)
                    {
                        if (_hexagonCast != null)
                        {
                            _hexagonCast.GetHexagonMain().Sprite.color = new Color(1, 1, 1, 1);
                            _hexagonCast = null;
                        }
                    }
                }
                else if (_isCast && Input.GetMouseButtonUp(0))
                {
                    if(_hexagonCast!=null)
                    {
                        _hexagonCast.GetHexagonMain().Sprite.color = new Color(1, 1, 1, 1);

                        if (_hexagonCast.IsFree || _hexagonCast.TypeHexagon == 1)
                        {
                            int HeroLayer = _hexagonCast.layer == 9 ? 8 : 11;
                            Vector2 PosHex = _hexagonCast.GetHexagonMain().position;
                            _heroControl.transform.position =
                                new Vector3(PosHex.x, PosHex.y, _heroControl.transform.position.z);
                            _heroControl.gameObject.layer = HeroLayer;
                            _heroControl.ChangeOfPosition();
                            _heroControl.CircularAttack(_damagBust, true);

                            _isReady = false;
                            _cooldown = _cooldownConst;
                            _imageAbiliti.fillAmount = 1;
                        }

                    }

                    _hexagonCast = null;
                    _isCast = false;
                }
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
    }
    public void initializationHero(HeroControl hero, Image img)
    {
        _heroControl = hero;
        _imageAbiliti = img;
    }
}
