using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBackground : MonoBehaviour {

    public static MusicBackground instance;
    private AudioSource audioSource;

    void Awake() {
        audioSource = GetComponent<AudioSource>();

        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else if(instance != this) {
            Destroy(gameObject);
        }
    }

    public void ChangeMusicVolume(float musicVolume) {
        audioSource.volume = musicVolume;
    }
    public float GetVolume() {
        return audioSource.volume;
    }
}
