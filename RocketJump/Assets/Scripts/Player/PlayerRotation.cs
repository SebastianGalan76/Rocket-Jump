using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour {
    [SerializeField] private BoxCollider2D cannonCollider;
    [SerializeField] private Transform cannonTransform;

    private Vector2 lookDirection;
    private float angle;

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    void Update() {
        if (!UIPause.isPaused) {
            lookDirection = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized;
            angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            cannonTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //Return true if the cannon is looking at the platform.
    public bool LookPlatform() {
        float distance = 0.2f;
        RaycastHit2D raycast = Physics2D.BoxCast(cannonCollider.bounds.center, cannonCollider.bounds.size, 0f, lookDirection, distance, player.platformLayerMask);
        if (raycast.collider != null) {
            return raycast.collider;
        }

        raycast = Physics2D.BoxCast(cannonCollider.bounds.center, cannonCollider.bounds.size, 0f, lookDirection, distance, player.slopeLayerMask);
        return raycast.collider != null;
    }

    public Vector2 LookDirection {
        get { return lookDirection; }
    }

    public float Angle {
        get { return angle; }
    }
}
