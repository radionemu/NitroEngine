using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COM : MonoBehaviour
{
    public Rigidbody rbody;
    public Transform com;

    // Start is called before the first frame update
    void Start()
    {
        rbody.centerOfMass = com.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
