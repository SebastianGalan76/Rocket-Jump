using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private Transform cannonTransform;
    [SerializeField] private SpriteRenderer playerSprite;

    private bool playerIsDead;
    private bool playerOnSlope;
    private bool playerInSecretRoom;

    private new Rigidbody2D rigidbody;
    [HideInInspector] public new CircleCollider2D collider;

    public LayerMask platformLayerMask, slopeLayerMask;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();
    }

    public void Dead(bool value) {
        PlayerIsDead = value;

        if (value) {
            SteamAchievements.DeathAmount += 1;
            SteamAchievements.DeathAmountCurrentGame += 1;
        }
    }
    public bool PlayerIsDead {
        set {
            playerIsDead = value;
            ChangeCannonVisibility(!value);

            collider.isTrigger = value;

            Color color = playerSprite.color;

            if (value) {
                rigidbody.velocity = Vector2.zero;
                color.a = 0.25f;
            } else {
                color.a = 1f;
            }

            playerSprite.color = color;
        }
        get { return playerIsDead; }
    }
    public bool PlayerOnSlope {
        set {
            if (!value) {
                playerOnSlope = false;
                rigidbody.angularDrag = 100f;
                ChangeCannonVisibility(true);
            } else {
                playerOnSlope = true;
                rigidbody.angularDrag = 0.3f;
                ChangeCannonVisibility(false);
            }
        }
        get { return playerOnSlope; }
    }
    public bool PlayerInSecretRoom {
        set { playerInSecretRoom = value; }
        get { return playerInSecretRoom; }
    }
    public Vector2 Velocity {
        get { return rigidbody.velocity; }
        set { rigidbody.velocity = value; }
    }


    private void ChangeCannonVisibility(bool value) {
        cannonTransform.gameObject.SetActive(value);
    }
}
