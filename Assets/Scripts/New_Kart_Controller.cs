using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class New_Kart_Controller : MonoBehaviour
{
    [Header("Kart Stat")]
    public float maxSpeed;
    public float turnSpeed;

    public int driftEscapeConstants;

    [Header("Kart position")]
    public bool isdrift = false;
    public Transform steeringTransform;
    public Transform driftTransform;
    private Rigidbody rBody;
    public Vector3 wheelVelocityLS;
    public float ratio;
    public int speedmetre;
    public float speedfloat;

    //UI
    public UI_DriftEscape uI_DriftEscape;
    public UI_SPD uI_SPD;
    public Text text;

    //booster
    public circle_gauge_contorller circle_Gauge_Contorller;
    public float booster_time = 2f;
    public bool booster_on = false;
    float drift_start_time = 0f;
    bool drift_start_time_cor = false;
    //ool new_cut_cor = false;

    //car rotation
    Vector3 addition;

    //isgrounded
    public Suspension[] suspensiontr;

    // Start is called before the first frame update
    void Start()
    {
        rBody = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //드리프트 -> 반대키 입력시 반대키 토크?

        if((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && speedmetre >= 30 && (Input.GetAxisRaw("Horizontal")!=0)){
            rBody.angularDrag = 0;
            if(Input.GetAxisRaw("Horizontal") == 0)rBody.angularDrag = 5;
            isdrift = true;
        }else if(ratio >= 0.4f && speedmetre >= 30){
            rBody.angularDrag = 5;
            isdrift = true;
        }else{
            rBody.angularDrag = 0;
            isdrift = false;
        }

        //KartRay
        Debug.DrawRay(transform.position, wheelVelocityLS.z * transform.forward, Color.cyan);
        Debug.DrawRay(transform.position, wheelVelocityLS.x * transform.right, Color.blue);
        Ratio();

        //spdcheck
        speedfloat = rBody.velocity.magnitude;
        speedmetre = Mathf.RoundToInt(rBody.velocity.magnitude * 6.8f);
        spd(speedmetre);

        if (booster_on == false && Input.GetKeyDown(KeyCode.LeftControl) && circle_Gauge_Contorller.boosterManager.getBoosterQueueFront() != 0)
        {
            StartCoroutine(nameof(boosteron));
        }

    }

    private void FixedUpdate() {

        //wheelVelocityLS
        wheelVelocityLS = transform.InverseTransformDirection(rBody.velocity); 

        //old accel
        // if(!isdrift)
        // rBody.AddForce(transform.forward*Input.GetAxisRaw("Vertical")*2500);

        //new accel
        //rBody.AddRelativeForce(Vector3.forward * Input.GetAxisRaw("Vertical") * 1000);

        //booster
        if(booster_on == true)
        {
            rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 3500*1.3f);
            //circle_Gauge_Contorller.booster -= 1;
        }
        else if(speedmetre<=200 && !booster_on)
        {
            rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 3500);
        }

        //old Steer
        if(!isdrift)
             rBody.AddForceAtPosition(transform.right*Input.GetAxisRaw("Horizontal"), steeringTransform.position, ForceMode.Acceleration);
        // if(isdrift)
        //     rBody.AddForceAtPosition(transform.right*Input.GetAxisRaw("Horizontal")*10 * -1, driftTransform.position, ForceMode.Acceleration);


        if(isdrift){
            if (!drift_start_time_cor)
            {
                StartCoroutine(nameof(booster_timer));
            }            
            if(wheelVelocityLS.z < 0){
                wheelVelocityLS.z = Mathf.Max(0,wheelVelocityLS.z);
                wheelVelocityLS.x = Mathf.Lerp(wheelVelocityLS.x, 0,  Time.fixedDeltaTime * 0.001f);
                rBody.velocity = transform.TransformDirection(wheelVelocityLS);
                rBody.AddTorque(new Vector3(0, (wheelVelocityLS.x > 0 ? (Input.GetAxisRaw("Horizontal") > 0 ? Input.GetAxisRaw("Horizontal") : 0) : (Input.GetAxisRaw("Horizontal") < 0 ? Input.GetAxisRaw("Horizontal") : 0))*280,0));
            }else{
                rBody.AddTorque(new Vector3(0,Input.GetAxisRaw("Horizontal")*280,0));
            }
            if(ratio >= 0.3f && ratio <= 0.5f){
                uI_DriftEscape.onAcitve();
                rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 500);
            }else{
                uI_DriftEscape.onDeActive();
            }

            //차체 흔들리는 모션
            rBody.AddTorque(transform.forward*500*Input.GetAxis("Horizontal"), ForceMode.Force);
        }else{
            uI_DriftEscape.onDeActive();
        }

    }

    private void OnDrawGizmos() {
        if(!isdrift)
            Gizmos.DrawSphere(steeringTransform.position, 0.1f);
        if(isdrift)
            Gizmos.DrawSphere(driftTransform.position, 0.1f);
    }

    public void Ratio(){
        float x = Mathf.Abs(wheelVelocityLS.x);
        float z = Mathf.Abs(wheelVelocityLS.z);
        
        ratio = (x) / (x + z);
        if(speedmetre <= 30)ratio = 0;

        text.text = Mathf.RoundToInt(ratio*100).ToString("000");
    }

    public void spd(int val){
        uI_SPD.updText(val);
    }

    public bool Ground(){
        foreach(Suspension sus in suspensiontr){
            if(!sus.isGrounded()) return false;
        }
        return true;
    }

    private IEnumerator boosteron()
    {
        // Debug.Log("on");
        booster_on = true;
        circle_Gauge_Contorller.boosterManager.DeQueue();
        rBody.AddForce(transform.forward * Input.GetAxisRaw("Vertical") * 2500 * 1.3f);
        yield return new WaitForSeconds(booster_time);
        
        StopCoroutine("boosteron");
        booster_on = false;
        
    }

    private IEnumerator booster_timer()
    {
        drift_start_time_cor = true;
        yield return new WaitForSeconds(0.01f);
        drift_start_time += 0.01f;
        StopCoroutine("booster_timer");
        drift_start_time_cor = false;
    }
}
