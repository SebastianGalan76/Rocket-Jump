using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour {
    public GameObject[] soundObjects;

    public float soundVolume;

    public void PlaySound(int soundID) {
        GameObject soundObj = Instantiate(soundObjects[soundID]);

        float soundVolume;

        if(soundID == 3) {
            soundVolume = this.soundVolume / 2;
        } else {
            soundVolume = this.soundVolume;

        }

        soundObj.GetComponent<AudioSource>().volume = soundVolume;

        Destroy(soundObj, 3f);
    }
    public void PlaySound(int soundID, Vector2 velocity) {
        if(!(Mathf.Abs(velocity.y) >= 0.45f)) {
            return;
        }
        GameObject soundObj = Instantiate(soundObjects[soundID]);

        float magnitude = velocity.magnitude;

        float multiplier = Mathf.Sqrt(magnitude / 2);
        if(multiplier > 1) {
            multiplier = 1;
        }

        float soundVolume = (this.soundVolume / 2) * multiplier;

        soundObj.GetComponent<AudioSource>().volume = soundVolume;

        Destroy(soundObj, 3f);
    }
}
