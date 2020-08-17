using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    public GameObject butterfly, cocoon, crackedCocoon, buttons;
    public float shakeTimer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer != 0){
            shakeTimer -= Time.deltaTime;
        }
        if(shakeTimer < 0){
            shakeTimer = 0;
            cocoon.SetActive(false);
            crackedCocoon.SetActive(true);
            butterfly.SetActive(true);
            Invoke("PlayWooHoo",3.5f);
            Invoke("PlayYay",5f);
        }
    }

    void PlayWooHoo(){
        AudioManager.audioManager.Play("WooHoo");
    }
    void PlayYay(){
        AudioManager.audioManager.Play("Yay");
        buttons.SetActive(true);
    }
    public void Quit(){
        Application.Quit();
    }
}
