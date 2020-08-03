using Assets.HeroEditor.Common.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Protection : MonoBehaviour,IActiveAbility
{
    private Image _imageAbiliti;
    private HeroControl _heroControl;

    [SerializeField]
    private float _powerProtected, _timeProtected, _cooldown;
    private float _width, _height, _cooldownConst, _timeProtectedConst, _fillAmountTime;
    private bool _isCast, _isReady;
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        _imageAbiliti.fillAmount = 0;
        _timeProtectedConst = _timeProtected;
        _timeProtected = 0;
        _cooldownConst = _cooldown;
        _cooldown = 0;
        _fillAmountTime = Time.fixedDeltaTime / _cooldownConst;
        _width = rectTransform.rect.width / 2;
        _height = rectTransform.rect.height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Mathf.Abs(transform.position.x - Input.mousePosition.x) <= _width
                && Mathf.Abs(transform.position.y - Input.mousePosition.y) <= _height)
            {
                if (_isReady)
                {
                    CastAbilite();
                    _cooldown = _cooldownConst;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (_isCast)
        {
            if (_timeProtected>0)
            {
                _timeProtected -= Time.fixedDeltaTime;
            }
            else
            {
                _isCast = false;
                EndCastAbiliti();
            }
        }

        if (_cooldown<=0&&!_isReady)
        {
            _isReady = true;
        }

        if (_cooldown>0)
        {
            _cooldown -= Time.fixedDeltaTime;
            _imageAbiliti.fillAmount -= _fillAmountTime;
        }
    }
    private void CastAbilite()
    {
        _timeProtected = _timeProtectedConst;
        _isCast = true;
        _isReady = false;
        _heroControl.BuffArmor += _powerProtected;
        _imageAbiliti.fillAmount = 1;
    }
    private void EndCastAbiliti()
    {
        _heroControl.BuffArmor -= _powerProtected;
    }
    public void initializationHero(HeroControl hero, Image img)
    {
        _heroControl = hero;
        _imageAbiliti = img;
    }

}
