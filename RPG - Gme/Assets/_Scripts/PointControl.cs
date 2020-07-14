using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointControl : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjFlag;
    public void Flag()
    {
        GameObject f = Instantiate(ObjFlag, transform);
        f.transform.position = transform.position;
    }
}
