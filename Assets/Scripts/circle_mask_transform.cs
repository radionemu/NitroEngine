using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circle_mask_transform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform circle_gauge_transform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.transform.position = circle_gauge_transform.position;
    }
}
