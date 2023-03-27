using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystem : MonoBehaviour {
    public GameObject[] particles;

    public void ShowParticle(int particleID, Vector3 position, float destroyDelay = 2f) {
        GameObject particle = Instantiate(particles[particleID]);
        particle.transform.position = position;

        Destroy(particle, destroyDelay);
    }
}
