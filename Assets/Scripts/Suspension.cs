using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Suspension : MonoBehaviour 
{
	public enum WheelType{
		FrontLeft,
		FrontRight,
		RearLeft,
		RearRight,
	}

	private Rigidbody rbody;

	[Header("Suspension")]
	public float restlength;
	public float springTravel;
	public float springStiffness;
	public float damperStiffness;

	private float maxLength;
	private float minLength;
	private float lastFrameLength;
	public float springLength;	
	private float springVelocity;
	[SerializeField]
	private float springForce;
	private float damperForce;

	private float Fx;
	public static float Fy;

	RaycastHit hit;

	[Header("Wheel")]
	public WheelType wheelType;
	public Vector3 wheelRadius;
	public Vector3 wheelTransform;

	private Vector3 SuspensionForce;
	private Vector3 wheelVelocityLS;

	[Header("Skid")]
	public GameObject skid;
	public GameObject nkcobj;
	public New_Kart_Controller nkc;
	public Skidmarks skidManager;
	private int skidindex = -1;
	private float intensity = 0.0f;
	public float skidadder = 5.0f;

	private void Start() {
		rbody = transform.root.GetComponent<Rigidbody>();
		minLength = restlength - springTravel;
		maxLength = restlength + springTravel;	
		nkc = nkcobj.GetComponent<New_Kart_Controller>();
	}

	private void Update() {
		//서스펜션 포스
		Debug.DrawRay(transform.position, -transform.up * springLength, Color.green);
		//휠의 속력
		//Debug.DrawRay(hit.point, wheelVelocityLS.z * transform.forward , Color.yellow);
		//구심력
		Debug.DrawRay(hit.point, wheelVelocityLS.x * -transform.right, Color.red);
		//합력
		//Debug.DrawRay(hit.point, SuspensionForce + Fy * -transform.right, Color.blue);
	}

	private void FixedUpdate()
	{
		bool isgrounded = Physics.Raycast(transform.position, -transform.up, out hit, restlength);
		if(isgrounded){
			lastFrameLength = springLength;
			springLength = hit.distance;
			springVelocity = (lastFrameLength - springLength)/Time.fixedDeltaTime;
			springForce = springStiffness * (restlength - springLength);
			damperForce = damperStiffness * springVelocity;
			SuspensionForce = (springForce+damperForce) * transform.up;

			wheelVelocityLS = transform.InverseTransformDirection(rbody.GetPointVelocity(hit.point)); 
			Fx = wheelVelocityLS.z;


			//구심력
			//이거 지우면 끝나긴 하는데 마찰력이 구현이 안됨
			Fy = wheelVelocityLS.x * springForce / 3;
			if(nkc.isdrift){
				Fy /= 8;
			}


			rbody.AddForceAtPosition(Fy * -transform.right, hit.point);
			rbody.AddForceAtPosition(SuspensionForce, hit.point);

			if(skidManager != null && nkc.isdrift){
				intensity = Math.Clamp(intensity+Time.deltaTime*nkc.speedfloat/skidadder, 0.0f, 1.0f);
				skidindex = skidManager.AddSkidMark(hit.point, hit.normal, intensity, skidindex);
			}else{
				if(intensity > 0){
					intensity = Math.Clamp(intensity-Time.deltaTime*2.0f, 0.0f, 1.0f);
					skidindex = skidManager.AddSkidMark(hit.point, hit.normal, intensity, skidindex);
				}else{
					skidindex = -1;
				}
			}	
						//wheel position
			if((int)wheelType%2 == 0){
				//left
				wheelTransform = new Vector3(hit.point.x+wheelRadius.x, hit.point.y+wheelRadius.y, hit.point.z+wheelRadius.z);
			}else{
				wheelTransform = new Vector3(hit.point.x+wheelRadius.x, hit.point.y+wheelRadius.y, hit.point.z-wheelRadius.z);
			}
		
		}
	}

	public bool isGrounded(){
		return Physics.Raycast(transform.position, -transform.up, out hit, restlength);
	}
}
