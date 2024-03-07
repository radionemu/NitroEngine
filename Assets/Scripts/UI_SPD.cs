using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SPD : MonoBehaviour
{
    public Text text;

    // Start is called before the first frame update
    public void Start()
    {
        text.text = "000";
    }

    public void updText(int val){
        text.text = val.ToString("000");
    }
}
