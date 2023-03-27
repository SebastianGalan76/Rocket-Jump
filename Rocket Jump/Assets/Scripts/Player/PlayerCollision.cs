using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Animator secretRoomAnim;
    [SerializeField] private UnityEngine.ParticleSystem[] particles;
    [SerializeField] private SoundSystem soundSystem;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private UIGameCompleted uiGameCompleted;
    [SerializeField] private UIInfoPanel infoPanel;

    private Player player;
    private CircleCollider2D collider;
    private new Rigidbody2D rigidbody;

    private float soundTime = 0;

    private Vector2 lastVelocity;
    private int collisionCount = 0;

    private void Awake()
    {
        player = GetComponent<Player>();
        collider = GetComponent<CircleCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        soundTime += Time.deltaTime;

        if (GameSystem.isPaused) return;

        lastVelocity = rigidbody.velocity;
    }

    void FixedUpdate()
    {
        CheckGround();

        void CheckGround()
        {
            List<ContactPoint2D> contactPoints = new List<ContactPoint2D>();
            rigidbody.GetContacts(contactPoints);

            int[] contact = new int[2];
            contact[0] = 0;
            contact[1] = 0;

            foreach (ContactPoint2D contactPoint in contactPoints)
            {
                LayerMask layer = contactPoint.collider.gameObject.layer;

                if (LayerMask.LayerToName(layer.value) == "Slope")
                {
                    contact[0] = 1;
                }
                if (LayerMask.LayerToName(layer.value) == "Platform")
                {
                    contact[1] = 1;
                }
            }

            if (contact[1] == 1)
            {
                player.IsOnSlope = false;
            }
            else if (contact[1] == 0 && contact[0] == 1)
            {
                player.IsOnSlope = true;
            }
        }
    }

    //Return true if the player is touching the certain ground
    public bool IsGrounded(string layerName = null)
    {
        RaycastHit2D raycast;

        if (layerName == "Platform")
        {
            raycast = Physics2D.Raycast(collider.bounds.center + new Vector3(0, collider.bounds.extents.y), Vector2.down, (2 * collider.bounds.extents.y) + 0.2f, player.platformLayerMask);

            return raycast.collider;
        }
        else if (layerName == "Slope")
        {
            raycast = Physics2D.Raycast(collider.bounds.center + new Vector3(0, collider.bounds.extents.y), Vector2.down, (2 * collider.bounds.extents.y) + 0.4f, player.slopeLayerMask);

            return raycast.collider;
        }
        else
        {
            raycast = Physics2D.Raycast(collider.bounds.center + new Vector3(0, collider.bounds.extents.y), Vector2.down, (2 * collider.bounds.extents.y) + 0.2f, player.platformLayerMask);
            if (raycast.collider == null)
            {
                raycast = Physics2D.Raycast(collider.bounds.center + new Vector3(0, collider.bounds.extents.y), Vector2.down, (2 * collider.bounds.extents.y) + 0.4f, player.slopeLayerMask);
            }

            return raycast.collider;
        }
    }

    //Return the name of the layer the player collides with.
    private string CheckGround(Collision2D collision)
    {
        LayerMask layer = collision.collider.gameObject.layer;
        return LayerMask.LayerToName(layer.value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CheckGround(collision) == "Platform")
        {
            if (player.IsOnSlope) { player.IsOnSlope = false; }

            if (IsGrounded())
            {
                if (soundTime >= 0.09f)
                {
                    soundSystem.PlaySound(4, rigidbody.velocity);
                    soundTime = 0;
                }
                particle.ShowParticle(2, collision.contacts[0].point);
            }
            else
            {
                particle.ShowParticle(1, collision.contacts[0].point);
                soundSystem.PlaySound(4, rigidbody.velocity);
            }

            BouncePlayer();

            void BouncePlayer()
            {
                if (lastVelocity.y > 0 && Mathf.Abs(collision.contacts[0].normal.x) > Mathf.Abs(collision.contacts[0].normal.y))
                {
                    float speed = lastVelocity.magnitude;
                    Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                    float x = direction.x * 0.75f;
                    float y = direction.y;

                    direction = new Vector3(x, y);

                    rigidbody.velocity = direction.normalized * speed;
                }
                if (Mathf.Abs(collision.contacts[0].normal.x) < Mathf.Abs(collision.contacts[0].normal.y) && collision.contacts[0].normal.y < 0)
                {
                    float speed = lastVelocity.magnitude * 0.7f;
                    Vector3 direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                    float x = direction.x * 4;
                    float y = direction.y + (direction.y / 2);

                    direction = new Vector3(x, y);

                    rigidbody.velocity = direction.normalized * speed * 1.2f;
                }
            }
        }
        else if (CheckGround(collision) == "Slope")
        {
            if (IsGrounded("Slope"))
            {
                player.IsOnSlope = true;
            }

            particle.ShowParticle(2, collision.contacts[0].point);

            if (soundTime >= 0.09f)
            {
                soundSystem.PlaySound(4, rigidbody.velocity);
                soundTime = 0;
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !player.IsDead)
        {
            player.IsDead = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slope") && !IsGrounded())
        {
            float speed = lastVelocity.magnitude;
            rigidbody.velocity = lastVelocity.normalized * speed * 1.8f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slope") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            collisionCount++;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && !player.IsDead)
        {
            player.IsDead = true;
        }

        if (collision.gameObject.tag == "Slime" && !player.IsDead)
        {
            rigidbody.velocity = Vector2.zero;
            rigidbody.gravityScale = 0.05f;
        }
        else if (collision.gameObject.tag == "SecretRoom")
        {
            player.IsInSecretRoom = true;

            if (!player.IsDead)
            {
                secretRoomAnim.SetBool("showSecretRoom", true);
            }
        }
        else if (collision.gameObject.tag == "GoldenMedal" && !player.IsDead)
        {
            FileManager.SaveData("currentGame/wiseMovement" + collision.gameObject.name, 0);
            FileManager.SaveData("stats/wiseMovement" + collision.gameObject.name, 1);
            Destroy(collision.gameObject, 1);

            collision.gameObject.GetComponent<Animator>().Play("HideGoldenCoin");
            soundSystem.PlaySound(1);
            particle.ShowParticle(3, collision.transform.position);

            SteamAchievements.CheckWiseMovement();

            infoPanel.ShowPInfo(3);
        }
        else if (collision.gameObject.tag == "Magnifier" && !player.IsDead)
        {
            FileManager.SaveData("currentGame/secretRoom" + collision.gameObject.name, 0);
            FileManager.SaveData("stats/secretRoom" + collision.gameObject.name, 1);
            Destroy(collision.gameObject, 1);

            collision.gameObject.GetComponent<Animator>().Play("HideMagnifier");
            soundSystem.PlaySound(1);
            particle.ShowParticle(4, collision.transform.position);

            SteamAchievements.CheckSecretRoom();

            infoPanel.ShowPInfo(2);
        }
        else if (collision.gameObject.name == "Crown" && !player.IsDead)
        {
            //Finish Game
            particles[0].Play();
            particles[1].Play();

            FileManager.SaveData("currentGame/levelIsCompleted", 1);

            Time.timeScale = 0.1f;

            soundSystem.PlaySound(2);

            Destroy(collision.gameObject);

            //(1800) Complete the game within 30 minutes.
            if (TimeSystem.TimeValue < 1800)
            {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_8");
            }
            //(3600) Complete the game within 60 minutes.
            if (TimeSystem.TimeValue < 3600)
            {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_7");
            }
            SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_6");

            if (SteamAchievements.DeathAmountCurrentGame == 0)
            {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_13");
            }
            if (SteamAchievements.FallAmountCurrentGame == 0)
            {
                SteamAchievements.UnlockAchievement("NEW_ACHIEVEMENT_1_12");
            }

            StartCoroutine(wait());
            IEnumerator wait()
            {
                yield return new WaitForSeconds(0.4f);
                Time.timeScale = 1f;
                uiGameCompleted.ShowGameCompleted();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Slope") || collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            collisionCount--;
        }

        if (!IsGrounded() && collisionCount <= 0 && player.IsDead && (collision.gameObject.layer == LayerMask.NameToLayer("Slope") || collision.gameObject.layer == LayerMask.NameToLayer("Platform")) && !player.IsInSecretRoom)
        {
            player.IsDead = false;
        }

        if (collision.gameObject.tag == "Slime")
        {
            rigidbody.gravityScale = 3f;
        }
        else if (collision.gameObject.tag == "SecretRoom")
        {
            player.IsInSecretRoom = false;
            secretRoomAnim.SetBool("showSecretRoom", false);
        }
    }
}
