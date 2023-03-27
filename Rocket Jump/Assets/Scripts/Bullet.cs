using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private new CircleCollider2D collider;
    private new Rigidbody2D rigidbody;

    private Rigidbody2D playerRigidbody;

    private Vector2 firstDirection, lastVelocity;
    private const float SPEED = 15f;

    private ParticleSystem particle;
    private SoundSystem sound;

    public void Initialize(Vector2 firstDirection, Rigidbody2D playerRigidbody, ParticleSystem particle, SoundSystem soundSystem) {
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        this.sound = soundSystem;

        this.firstDirection = firstDirection;
        this.playerRigidbody = playerRigidbody;
        this.particle = particle;

        rigidbody.velocity = firstDirection.normalized * SPEED;

        Destroy(gameObject, 2f);
    }

    private void FixedUpdate() {
        lastVelocity = rigidbody.velocity;
        if(lastVelocity.magnitude <= SPEED - 5) {
            Vector2 changeDirection = Vector2.Perpendicular(firstDirection);
            rigidbody.velocity = -changeDirection.normalized * SPEED;
        }
        if(lastVelocity.magnitude <= 5) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        particle.ShowParticle(1, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Platform") || collision.gameObject.layer == LayerMask.NameToLayer("Slope") || collision.gameObject.layer == LayerMask.NameToLayer("Bullet")) {
            Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rigidbody.velocity = direction * SPEED;

            particle.ShowParticle(1, transform.position);
        } else if(collision.gameObject.layer == LayerMask.NameToLayer("Player")) {
            Vector3 direction = -lastVelocity.normalized;

            rigidbody.velocity = direction * SPEED;

            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(-direction * 800f);

            particle.ShowParticle(1, transform.position);
            sound.PlaySound(0);
        } else if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.tag == "Slime") {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.gameObject.tag == "Player") {
            collider.isTrigger = false;
        }
    }
}
