using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private SoundSystem soundSystem;
    [SerializeField] private GameObject bulletPrefab;

    private Player player;
    private PlayerRotation rotation;
    private PlayerCollision collision;
    private PlayerSkinSystem skinSystem;
    private new Rigidbody2D rigidbody;

    private float currentPower, jumpPower;

    private GameObject currentBullet;

    private void Awake()
    {
        player = GetComponent<Player>();
        rotation = GetComponent<PlayerRotation>();
        collision = GetComponent<PlayerCollision>();
        skinSystem = GetComponent<PlayerSkinSystem>();

        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (GameSystem.isPaused) { return; }

        if (Input.GetMouseButtonDown(0))
        {
            currentPower = 1;
        }
        //Load jump power according to the length of the mouse button press.
        if (Input.GetMouseButton(0))
        {
            currentPower += 0.65f * Time.deltaTime;
            ChangeCannonSize();
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!player.IsDead && !player.IsOnSlope)
            {
                if (rotation.LookPlatform())
                {
                    Jump();
                }
                else
                {
                    ShootBullet();
                }
                currentPower = 1;
                ChangeCannonSize();
                soundSystem.PlaySound(0);
            }
        }

        void Jump()
        {
            JumpPower = currentPower;

            rigidbody.velocity = Vector2.zero;
            rigidbody.AddForce(-rotation.LookDirection * 700f * JumpPower);

            SteamAchievements.JumpAmount += 1;
            SteamAchievements.JumpAmountCurrentGame += 1;

            particle.ShowParticle(0, transform.position + new Vector3(rotation.LookDirection.x / 4, rotation.LookDirection.y / 4, transform.position.z));
        }
        void ShootBullet()
        {
            //Destroy current bullet if exist
            if (currentBullet != null)
            {
                Destroy(currentBullet);
            }

            //Create new bullet
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Bullet>().Initialize(rotation.LookDirection, rigidbody, particle, soundSystem);
            currentBullet = bullet;

            //Move player (left/right) after shooting
            if ((rotation.Angle < 50 || rotation.Angle > 130) && collision.IsGrounded("Platform") && !rotation.LookPlatform())
            {
                float x = rotation.LookDirection.x / Mathf.Abs(rotation.LookDirection.x);
                rigidbody.AddForce(Vector2.left * 120 * x);
            }

            particle.ShowParticle(0, transform.position + new Vector3(rotation.LookDirection.x / 1.5f, rotation.LookDirection.y / 1.5f, transform.position.z));
        }
        void ChangeCannonSize()
        {
            int stageNr;

            if (currentPower == 1f)
            {
                stageNr = 0;
            }
            else if (currentPower < 1.2f)
            {
                stageNr = 1;
            }
            else if (currentPower < 1.4f)
            {
                stageNr = 2;
            }
            else
            {
                stageNr = 3;
            }

            skinSystem.ChangeCannonVisibility(stageNr);
        }
    }

    private float JumpPower
    {
        set
        {
            int power = (int)((value - 1) / 0.05f);
            jumpPower = 1 + (power * 0.05f);
            if (jumpPower > 1.4f)
            {
                jumpPower = 1.4f;
            }
        }
        get
        {
            return jumpPower;
        }
    }
}
