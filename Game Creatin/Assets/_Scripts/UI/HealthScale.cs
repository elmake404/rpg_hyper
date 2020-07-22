using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScale : MonoBehaviour
{
    private IControl _hero;
    [SerializeField]
    private Image _healthBar;
    void Start()
    {
        _hero = transform.parent.GetComponent<IControl>();
        ControlHealth();
    }
    void FixedUpdate()
    {
        ControlHealth();
    }
    private void ControlHealth()
    {
        _healthBar.fillAmount = _hero.GetHealthProcent() / 100f;
    }
}
