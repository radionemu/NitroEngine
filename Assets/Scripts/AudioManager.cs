using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public New_Kart_Controller new_Kart_Controller;

    public AudioSource engineSound;

    public AudioSource driftSoudnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        engineSound.pitch = new_Kart_Controller.speedmetre*0.02f+0.5f;

        if(new_Kart_Controller.isdrift && !driftSoudnd.isPlaying)driftSoudnd.Play();
        else if(!new_Kart_Controller.isdrift) driftSoudnd.Stop();
    }
}
