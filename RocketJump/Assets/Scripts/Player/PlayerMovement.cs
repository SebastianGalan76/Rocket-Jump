using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private Vector2 startPosition, savePosition;
    private bool freeMovement;

    private new Rigidbody2D rigidbody;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start() {
        startPosition = transform.position;
    }

    private void FixedUpdate() {
        //If the player somehow ended up under the map.
        if (transform.position.y < 0) {
            rigidbody.velocity = Vector2.zero;
            transform.position = startPosition;
        }
    }

    void Update() {
        if (!UIPause.isPaused) {
            if (TestMode.testMode) {
                if (Input.GetKeyDown(KeyCode.Z)) {
                    freeMovement = !freeMovement;

                    if (freeMovement) {
                        rigidbody.gravityScale = 0.001f;
                    } else {
                        rigidbody.gravityScale = 3f;
                    }
                }

                if (Input.GetKey(KeyCode.UpArrow)) {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.up, 0.4f);
                }
                if (Input.GetKey(KeyCode.DownArrow)) {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.down, 0.4f);
                }
                if (Input.GetKey(KeyCode.LeftArrow)) {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.left, 0.4f);
                }
                if (Input.GetKey(KeyCode.RightArrow)) {
                    transform.position = Vector3.Lerp(transform.position, transform.position + Vector3.right, 0.4f);
                }

                if (Input.GetKeyDown(KeyCode.N)) {
                    savePosition = transform.position;
                }
                if (Input.GetKeyDown(KeyCode.M)) {
                    transform.position = savePosition;
                    rigidbody.velocity = Vector2.zero;
                }
            }
        }
    }
}
