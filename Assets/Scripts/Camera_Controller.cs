using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public float rotsmoothconstants;
    public float smoothconstants;
    public float rotationzconstants;

    public Transform player;

    public Camera camera;
    public New_Kart_Controller new_Kart_Controller;
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, smoothconstants * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, rotsmoothconstants * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));

        if(new_Kart_Controller.booster_on){
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,80, speed * Time.fixedDeltaTime);
        }else{
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView,60, speed * Time.fixedDeltaTime);
        }
    }
}
