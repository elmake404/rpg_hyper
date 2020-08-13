using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSmooth : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private float _speed;
    void FixedUpdate()
    {
        transform.Translate(direction*_speed);
    }
}
