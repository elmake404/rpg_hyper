using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDestroy : MonoBehaviour
{
    [SerializeField]
    private float _timeLife;
    private void Awake()
    {
        Destroy(gameObject,_timeLife);
    }
}
