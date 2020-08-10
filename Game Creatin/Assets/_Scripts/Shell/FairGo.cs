using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairGo : MonoBehaviour
{
    [SerializeField]
    public Vector2 Target;
    void FixedUpdate()
    {
        if (((Vector2)transform.position - Target).magnitude >= 0.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target, 0.5f);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
