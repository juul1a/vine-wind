using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audioManager;
    public Sound[] sounds;
    // Start is called before the first frame update
    void Awake()
    {
        audioManager = this;
        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = false;
            s.source.loop = s.loop;
        }
    }

    void Start(){
        //Play("MainTheme");
    }

    public void Play(string soundName){
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        if(s==null){
            Debug.LogWarning("Couldn't find sound with name "+soundName);
            return;
        }
        s.source.Play();
        // Debug.Log("Playing "+s.name);
    }
}
