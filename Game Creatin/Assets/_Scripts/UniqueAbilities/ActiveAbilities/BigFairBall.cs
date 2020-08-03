using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFairBall : MonoBehaviour
{
    private float _width, _height;
    void Start()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        _width = rectTransform.rect.width / 2;
        _height = rectTransform.rect.height / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(Mathf.Abs(transform.position.x - Input.mousePosition.x)<= _width
                &&Mathf.Abs(transform.position.y - Input.mousePosition.y)<= _height)
            {
                //Debug.Log(0);
            }
            else
            {
                //Debug.Log(1);

            }
        }
    }
}
