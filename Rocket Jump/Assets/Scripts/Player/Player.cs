using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("LayerMasks")]
    public LayerMask platformLayerMask, slopeLayerMask;

    private bool _playerIsOnSlope;
    private bool _playerIsDead;
    private bool _playerIsInSecretRoom;

    private Transform cannonTransform;
    private SpriteRenderer playerSprite;

    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;

    private void Start()
    {
        cannonTransform = gameObject.transform.Find("Cannon").transform;
        playerSprite = gameObject.transform.Find("PlayerSkin").GetComponent<SpriteRenderer>();

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CircleCollider2D>();

    }



    private void ChangeCannonVisibility(bool value)
    {
        cannonTransform.gameObject.SetActive(value);
    }

    public bool IsOnSlope
    {
        set
        {
            if (!value)
            {
                _playerIsOnSlope = false;
                rigidbody.angularDrag = 100f;
                ChangeCannonVisibility(true);
            }
            else
            {
                _playerIsOnSlope = true;
                rigidbody.angularDrag = 0.3f;
                ChangeCannonVisibility(false);
            }
        }
        get { return _playerIsOnSlope; }
    }
    public bool IsDead
    {
        set
        {
            _playerIsDead = value;
            ChangeCannonVisibility(!value);

            collider.isTrigger = value;

            Color color = playerSprite.color;

            if (value)
            {
                rigidbody.velocity = Vector2.zero;
                color.a = 0.25f;
            }
            else
            {
                color.a = 1f;
            }

            playerSprite.color = color;

            //Achievements
            if (value)
            {
                SteamAchievements.DeathAmount += 1;
                SteamAchievements.DeathAmountCurrentGame += 1;
            }
        }
        get { return _playerIsDead; }
    }
    public bool IsInSecretRoom
    {
        get { return _playerIsInSecretRoom;}
        set { _playerIsInSecretRoom = value; }
    }

    public Vector2 Velocity
    {
        get { return rigidbody.velocity; }
        set { rigidbody.velocity = value; }
    }
}
