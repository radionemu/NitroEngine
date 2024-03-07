using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DriftEscape : MonoBehaviour
{
    public Image image;

    public Sprite DE_deactive;

    public Sprite DE_active;

    private void Start() {
        image.sprite = DE_deactive;
    }

    public void onAcitve(){
        image.sprite = DE_active;
    }

    public void onDeActive(){
        image.sprite = DE_deactive;
    }
}
