using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerRotation : MonoBehaviour
{
    private Player player;

    private Vector2 lookDirection;
    float angle;

    private Transform cannonTransform;
    private BoxCollider2D cannonCollider;

    private void Start()
    {
        player = GetComponent<Player>();

        cannonTransform = gameObject.transform.Find("Cannon").GetComponent<Transform>();
        cannonCollider = gameObject.transform.Find("Cannon").GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (GameSystem.isPaused) return;

        RotatePlayer();

        void RotatePlayer()
        {
            lookDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            cannonTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //Return true if the cannon is looking at the platform.
    public bool LookPlatform()
    {
        float distance = 0.2f;
        RaycastHit2D raycast = Physics2D.BoxCast(cannonCollider.bounds.center, cannonCollider.bounds.size, 0f, lookDirection, distance, player.platformLayerMask);
        if (raycast.collider != null)
        {
            return raycast.collider;
        }

        raycast = Physics2D.BoxCast(cannonCollider.bounds.center, cannonCollider.bounds.size, 0f, lookDirection, distance, player.slopeLayerMask);
        return raycast.collider != null;
    }

    public Vector2 LookDirection
    {
        get { return lookDirection; }
    }

    public float Angle
    {
        get { return angle; }
    }
}
