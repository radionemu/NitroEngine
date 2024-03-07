using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circle_gauge_contorller : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform circle_gauge_transform;

    public New_Kart_Controller new_Kart_Controller;

    public Image booster_gauge_image;

    public Text booster_text;

    float booster_gauge = 0f;
    private bool booster_slider_plus_cor = false;

    [SerializeField]
    private float surspd;
    private float surratio;

    public int booster = 0;

    public BoosterManager boosterManager;

    void Start()
    {
        gameObject.transform.position = circle_gauge_transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        booster_gauge_image.fillAmount = 0.1f + booster_gauge;

        if(booster_gauge_image.fillAmount >= 0.9f && new_Kart_Controller.isdrift == false)
        {
            booster_gauge = 0;
            booster_gauge_image.fillAmount = 0.1f;
            if (booster < 2)
            {
                boosterManager.AddQueue();
            }
            
        }
    }

    private void FixedUpdate()
    {
        surspd = new_Kart_Controller.speedfloat;
        surratio = new_Kart_Controller.ratio;

        if (new_Kart_Controller.isdrift)
        {
            booster_slider();
        }
    }

    private IEnumerator booster_slider_plus()
    {
        Debug.Log("on");
        booster_slider_plus_cor = true;
        yield return new WaitForSeconds(0.01f);
        //booster_gauge += Time.deltaTime;
        if (booster_gauge_image.fillAmount <= 0.9f)
        {
            booster_gauge += 3 * Time.deltaTime;
        }
        StopCoroutine("booster_slider_plus");
        booster_slider_plus_cor = false;
    }

        private void booster_slider()
    {
        if (booster_gauge_image.fillAmount <= 0.9f)
        {
            booster_gauge +=  (surspd*0.5f) * surratio*0.5f * Time.deltaTime/4;
        }
        booster_gauge_image.fillAmount = Mathf.Max(booster_gauge_image.fillAmount, 0.9f);
    }


}
