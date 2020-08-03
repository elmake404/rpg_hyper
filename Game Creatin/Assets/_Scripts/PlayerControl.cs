using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Camera _camera;
    private HeroControl _heroControl;
    private Vector2 _startPos, _currPos;
    private void Awake()
    {
        //StaticLevelManager.IsGameFlove = true;
    }
    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (!StaticLevelManager.IsGameFlove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
                Collider2D Collider = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100)));
                if (Collider != null)
                {
                    if (Collider.gameObject.tag == "Hero")
                    {
                        _heroControl = Collider.gameObject.GetComponentInParent<HeroControl>();
                        _heroControl.MoveTheHero();
                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (_startPos == Vector2.zero)
                {
                    _startPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
                }

                _currPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));

                if (_heroControl != null)
                {
                    _heroControl.transform.position = _currPos;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_heroControl != null)
                {
                    _heroControl.InstallHero();
                    _heroControl = null;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _startPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));
                Collider2D Collider = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100)));
                if (Collider != null)
                {
                    if (Collider.gameObject.tag == "Hero")
                    {
                        _heroControl = Collider.gameObject.GetComponentInParent<HeroControl>();
                    }
                    else
                    {

                    }
                }
            }
            else if (Input.GetMouseButton(0))
            {
                _currPos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2));

            }
            else if (Input.GetMouseButtonUp(0))
            {
                Collider2D Collider = Physics2D.OverlapPoint(_camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -100)));
                if (_heroControl != null && Collider != null && Collider.tag == "Enemy")
                {
                    _heroControl.EnemyTarget = Collider.GetComponent<EnemyControl>();
                }
                else
                {
                    _heroControl = null;
                }
            }

        }
    }

}
