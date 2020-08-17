using System.Collections;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Music");
        if(objects.Length > 1){
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Mute(){
        AudioSource thisClip = gameObject.GetComponent<AudioSource>();
        thisClip.mute = !thisClip.mute;
    }
}