using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NavMeshControl : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private Transform _objСharacters;

    void Start()
    {
        _agent.updatePosition = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _agent.SetDestination(MapControl.MapPoint[9, 0].transform.position);
        //_agent.nextPosition = _objСharacters.position;
    }
}
