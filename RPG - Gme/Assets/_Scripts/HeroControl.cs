using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeroControl : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    private PointControl _pointMain;
    void Start()
    {
    }

    void FixedUpdate()
    {
        if (StaticLevelManager.IsStartLevel)
        {
            _pointMain = MapControl.GetPositionOntheMap(Mathf.RoundToInt(transform.position.z), Mathf.RoundToInt(transform.position.x));
            transform.rotation = _agent.transform.rotation;
            transform.position =Vector3.MoveTowards(transform.position, _agent.steeringTarget,0.1f);
        }
        if (Input.GetKeyDown(KeyCode.Space))
            StaticLevelManager.IsStartLevel = true;
        //MapControl.MapPoint[Mathf.RoundToInt(transform.position.z),Mathf.RoundToInt(transform.position.x) ].Flag();
    }
}
