using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WallOfFire : MonoBehaviour, IActiveAbility
{
    private Image _imageAbiliti;
    private HeroControl _heroControl;
    private HexagonControl _hexagonCast;
    private List<HexagonControl> _listHexagonCast = new List<HexagonControl>();
    [SerializeField]
    private Color _color;
    private Color _oldColor;

    [SerializeField]
    private float _deceleration, _duration, _cooldown;
    private float _width, _height, _cooldownConst, _fillAmountTime;
    private bool _isCast, _isReady;

    void Start()
    {
        _oldColor = MapControl.MapNav[0, 0].Sprite.color;

        _isReady = true;
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

                    #region Add
                    _listHexagonCast.Add(hex.GetHexagonMain());

                    if (_heroControl.transform.position.x < posWorold.x && _heroControl.transform.position.y < posWorold.y
                        || _heroControl.transform.position.x > posWorold.x && _heroControl.transform.position.y > posWorold.y)
                    {
                        int X1 = hex.Row % 2 == 0 ? 1 : 0;
                        int X2 = hex.Row % 2 == 0 ? 0 : -1;

                        if (hex.Row + 1 < MapControl.MapNav.GetLength(0) && (hex.Column + X1 >= 0 && hex.Column + X1 < MapControl.MapNav.GetLength(1)))
                        {
                            _listHexagonCast.Add(MapControl.MapNav[hex.Row + 1, hex.Column + X1].GetHexagonMain());
                        }
                        if (hex.Row - 1 >= 0 && (hex.Column + X2 >= 0 && hex.Column + X2 < MapControl.MapNav.GetLength(1)))
                        {
                            _listHexagonCast.Add(MapControl.MapNav[hex.Row - 1, hex.Column + X2].GetHexagonMain());
                        }
                    }
                    else
                    {
                        int X1 = hex.Row % 2 == 0 ? 0 : -1;
                        int X2 = hex.Row % 2 == 0 ? 1 : 0;

                        if (hex.Row + 1 < MapControl.MapNav.GetLength(0) && (hex.Column + X1 >= 0 && hex.Column + X1 < MapControl.MapNav.GetLength(1)))
                        {
                            _listHexagonCast.Add(MapControl.MapNav[hex.Row + 1, hex.Column + X1].GetHexagonMain());
                        }
                        if (hex.Row - 1 >= 0 && (hex.Column + X2 >= 0 && hex.Column + X2 < MapControl.MapNav.GetLength(1)))
                        {
                            _listHexagonCast.Add(MapControl.MapNav[hex.Row - 1, hex.Column + X2].GetHexagonMain());
                        }
                    }
                    #endregion

                    for (int i = 0; i < _listHexagonCast.Count; i++)
                    {
                        _listHexagonCast[i].Sprite.color = new Color(0.1154f, 0.3490566f, 0.07409222f, 1);
                    }

                    //hex.Sprite.color = new Color(0, 1, 0, 1);
                    _hexagonCast = hex;
                }

                if (hex==null)
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
                if (_listHexagonCast.Count>0)
                {
                    _isReady = false;
                    _imageAbiliti.fillAmount = 1;
                    _cooldown = _cooldownConst; 
                }
                for (int i = 0; i < _listHexagonCast.Count; i++)
                {
                    _listHexagonCast[i].Sprite.color = _oldColor;
                    Wall wall = _listHexagonCast[i].gameObject.AddComponent<Wall>();
                    wall.Initialization(_listHexagonCast[i], _color, _duration, _deceleration);
                }
                _listHexagonCast.Clear();

                _hexagonCast = null;
                _isCast = false;
            }
        }
    }
    private void FixedUpdate()
    {
        if (_cooldown<=0&&!_isReady)
        {
            _isReady = true;
        }

        if (_cooldown>0)
        {
            _imageAbiliti.fillAmount -= _fillAmountTime;
            _cooldown -= Time.fixedDeltaTime;
        }
    }
    public void initializationHero(HeroControl hero,Image img)
    {
        _heroControl = hero;
        _imageAbiliti = img;
    }

}
