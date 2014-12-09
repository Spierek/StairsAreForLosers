using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {
    #region Variables
    public static PlayerController Instance;

    public Vector2  movementSpeed = new Vector2(30f, 21f);
    public float    dashMod = 3f;
    public float    dashDuration = 0.25f;
    public float    dashDelay = 0.4f;
    public float    playerPushback = 0.6f;
    [NonSerialized] public float    invincibilityDuration = 1f;

    public Weapon   weapon;

    private int     health;
    private int     maxHealth = 8;

    private Vector3 mousePosition;
    private Vector3 spriteScale;
    private Vector2 dashForce;
    private float   dashTimer;

    private Animator        animator;
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer  sprite;
    private Hitbox          hitbox;
    private ParticleSystem  dashParticles;

    private float restartInputDelay = 0.5f;
    private float restartInputTimer;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        Instance = this;

        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = transform.Find("SpriteContainer").Find("Sprite").GetComponent<SpriteRenderer>();
        spriteScale = sprite.transform.parent.localScale;
        hitbox = transform.Find("Hitbox").GetComponent<Hitbox>();
        dashParticles = transform.Find("DashParticles").GetComponent<ParticleSystem>();
        dashParticles.enableEmission = false;

        health = maxHealth;

        dashTimer = dashDelay;
    }
    
    void Update () {
        if (health > 0) {
            Movement();
            Rotation();
            Attack();

            // DEBUG: healthbar
            string hp = "";
            for (int i = 0; i < health; i++) {
                hp += "♥";
            }
            MainDebug.WriteLine("Health", hp);
        }
        else {
            // FIXME: this shouldn't be here D:
            MainDebug.WriteLine("YOU ARE DEAD");
            MainDebug.WriteLine("PRESS ENTER TO RESTART");

            if (restartInputTimer > restartInputDelay && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))) {
                Application.LoadLevel(Application.loadedLevel);
            }
            restartInputTimer += Time.deltaTime;
        }

        // DEBUG: killing the player
        if (Input.GetKeyDown(KeyCode.U)) {
            Die();
        }
    }

    void OnDrawGizmos() {
        // draw mouse direction
        Gizmos.color = new Color(0,1,1,0.5f);
        Gizmos.DrawLine(transform.position, mousePosition);
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Pickup")) {
            col.gameObject.GetComponent<HeartPickup>().Grab();

            if (health != maxHealth)
                health++;
        }
    }
    #endregion

    #region Methods
    private void Movement() {
        Vector2 vel = new Vector2();

        // add dash force instead of WSAD input if dashing
        if (dashTimer < dashDuration) {
            vel = dashForce;
            if (!dashParticles.enableEmission) {
                dashParticles.enableEmission = true;
            }
        }
        // WSAD input
        else {
            vel.x = Input.GetAxis("Horizontal") * movementSpeed.x;
            vel.y = Input.GetAxis("Vertical") * movementSpeed.y;
            animator.SetBool("isRunning", vel != Vector2.zero);

            dashParticles.enableEmission = false;
        }
        
        // calculate dash
        if (Input.GetButtonDown("Dash") && dashTimer > dashDelay) {
            Vector2 mouseDir = GetMouseDirection();
            dashForce = -new Vector2(movementSpeed.x * mouseDir.x, movementSpeed.y * mouseDir.y) * dashMod;
            dashTimer = 0;
        }
        dashForce -= dashForce * Time.deltaTime / dashDuration;
        dashTimer += Time.deltaTime;

        // apply movement
        if (vel != Vector2.zero) {
            rigidbody2D.AddForce(vel * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    private void Rotation() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // flip sprite & weapon based on mouse position
        int dir = mousePosition.x >= transform.position.x ? 1 : -1;
        sprite.transform.localScale = new Vector3(spriteScale.x * dir, spriteScale.y);
        weapon.transform.parent.localScale = new Vector3(weapon.scale.x * dir, weapon.scale.y);

        // rotate weapon
        weapon.transform.localRotation = Quaternion.Euler(0, 0, GetMouseRotation());
    }

    private void Attack() {
        if (Input.GetButtonDown("Attack")) {
            weapon.Attack();
        }
    }

    public void Hit(int damage, Vector3 position, float pushbackMod = 1f) {
        Vector2 dir = (transform.position - position).normalized;
        rigidbody2D.AddForce(new Vector2(movementSpeed.x * dir.x, movementSpeed.y * dir.y) * playerPushback * pushbackMod, ForceMode2D.Impulse);

        // flash color
        sprite.material.color = new Color(1f, 0.3f, 0.3f, 1f);
        sprite.material.DOColor(Color.white, invincibilityDuration - 0.1f);

        if (health > 0)
            health -= damage;
        if (health <= 0) {
            Die();
        }

        hitbox.DisableCollider();
    }

    private float GetMouseRotation() {
        Vector2 mouseDir = mousePosition - transform.position;
        float rot = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        // clamp to (-90; 90) range to compensate for sprite flipping
        if (rot < -90 || rot > 90) {
            rot += 180;
            rot *= -1;
        }
            
        return rot;
    }

    public Vector2 GetMouseDirection() {
        return (transform.position - mousePosition).normalized;
    }

    private void Die() {
        health = 0;
        animator.SetBool("isDead", true);

    }
    #endregion
}
