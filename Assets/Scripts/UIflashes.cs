using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIflashes : MonoBehaviour
{
    public GameObject gotHitScreen;
    public float StartOpacity = 0.7f;
    public float fadeAwaySpeed = 0.01f;

    // Update is called once per frame
    void Update()
    {
        if(gotHitScreen != null){
            if(gotHitScreen.GetComponent<Image>().color.a > 0){
                var color = gotHitScreen.GetComponent<Image>().color;
                color.a -= fadeAwaySpeed;
                gotHitScreen.GetComponent<Image>().color=color;
            }
        }
    }

    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer==11){
                var color = gotHitScreen.GetComponent<Image>().color;
                color.a = StartOpacity;
                gotHitScreen.GetComponent<Image>().color=color;
        }
    }
}
