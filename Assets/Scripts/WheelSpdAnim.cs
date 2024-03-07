using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WheelSpdAnim : MonoBehaviour
{
    public New_Kart_Controller nKartcon;
    public Transform wheelTransform;
    public Suspension sus;
    public float wheelYOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //request wheel speed from New Kart Controller
        wheelTransform.Rotate(new Vector3(0,0,nKartcon.wheelVelocityLS.z) * 200.0f * Time.deltaTime);
        //request wheel position
        wheelTransform.localPosition = new Vector3(wheelTransform.localPosition.x,-sus.springLength+wheelYOffset,wheelTransform.localPosition.z);
        // wheelTransform.position = sus.wheelTransform;
    }
}
