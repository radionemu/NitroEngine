using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kart_Controller : MonoBehaviour
{
    public Rigidbody rbody;
    public Transform steeringtransform;

    private float max_Speed = 12.0f;
    private float acceleration = 0.5f;

    private float rotation;
    private float Steering = 80f;

    private float current_Velocity = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(current_Velocity >= 0){
            if(Input.GetAxisRaw("Vertical") > 0){
                current_Velocity = Mathf.Lerp(current_Velocity, max_Speed, Time.deltaTime * acceleration);
            }else if(Input.GetAxisRaw("Vertical") < 0){
                current_Velocity -= 8*acceleration*Time.deltaTime;
            }else{
                current_Velocity = Mathf.Lerp(current_Velocity, 0, Time.deltaTime * acceleration);
            }
        }

        int direction = 0;
        if(Input.GetAxis("Horizontal") != 0){
            direction = Input.GetAxis("Horizontal") > 0 ? 1 : -1;
            float amount = Mathf.Abs(Input.GetAxis("Horizontal")) / Mathf.Max(3.0f, current_Velocity);
            Steer(direction, amount);
        }
    }

    void FixedUpdate(){
        //전진 후진(중력 상관 없이)
        Vector3 v3 = transform.forward * current_Velocity;
        v3.y = rbody.velocity.y;
        rbody.velocity = v3;

        //회전
        // transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(transform.eulerAngles.x ,transform.eulerAngles.y + rotation, transform.eulerAngles.z), Time.deltaTime * 5f);
        rbody.AddForceAtPosition(new Vector3(Input.GetAxisRaw("Horizontal")*3, 0, 0), steeringtransform.position, ForceMode.Acceleration);
        //드리프트

        //중력

    }

    void Steer(int dir, float amount){
        rotation = Steering * dir * amount;
    }
}
