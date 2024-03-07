using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    bool accessdenyleft = false;
    bool accessdenyright = false;

    public static int Axis = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {  
        if(!accessdenyleft){
            if(Input.GetKey(KeyCode.RightArrow)){
                if(Input.GetKey(KeyCode.LeftArrow))Axis = -1;
                else Axis = 1;
                accessdenyright = true;
            }else accessdenyright = false;
        }else Axis = 0;
        if(!accessdenyright){
            if(Input.GetKey(KeyCode.LeftArrow)){
                if(Input.GetKey(KeyCode.RightArrow))Axis = 1;
                else Axis = -1;
               accessdenyleft = true;
            }else accessdenyleft = false;
        }else Axis = 0;

        Debug.Log(Axis);
        
    }
}
