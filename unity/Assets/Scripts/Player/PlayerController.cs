using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Variables
    public Vector2  movementSpeed = new Vector2(30f, 21f);
    public float    dashMod = 3f;
    public float    dashDuration = 0.25f;
    public float    dashDelay = 0.4f;

    public Weapon   weapon;

    private Vector3 mousePosition;
    private Vector3 spriteScale;
    private Vector2 dashForce;
    private float   dashTimer;

    private Animator        animator;
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer  sprite;
    private Hitbox          hitbox;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteScale = sprite.transform.localScale;
        hitbox = transform.Find("Hitbox").GetComponent<Hitbox>();

        dashTimer = dashDelay;
    }
    
    void Update () {
        Movement();
        Rotation();
        Attack();
    }

    void OnDrawGizmos() {
        // draw mouse direction
        Gizmos.color = new Color(0,1,1,0.5f);
        Gizmos.DrawLine(transform.position, mousePosition);
    }
    #endregion

    #region Methods
    private void Movement() {
        Vector2 vel = new Vector2();

        // add dash force instead of WSAD input if dashing
        if (dashTimer < dashDuration) {
            vel = dashForce;
        }
        // WSAD input
        else {
            vel.x = Input.GetAxis("Horizontal") * movementSpeed.x;
            vel.y = Input.GetAxis("Vertical") * movementSpeed.y;
        }
        
        // calculate dash
        if (Input.GetButtonDown("Dash") && dashTimer > dashDelay) {
            Vector2 mouseDir = (transform.position - mousePosition).normalized;
            dashForce = -new Vector2(movementSpeed.x * mouseDir.x, movementSpeed.y * mouseDir.y) * dashMod;
            dashTimer = 0;
        }
        dashForce -= dashForce * Time.deltaTime / dashDuration;
        dashTimer += Time.deltaTime;

        // apply movement
        if (vel != Vector2.zero) {
            rigidbody2D.AddForce(vel, ForceMode2D.Impulse);
        }

        // DEBUG
        MainDebug.WriteLine("Dash Timer", dashTimer.ToString());
    }

    private void Rotation() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // flip sprite & weapon based on mouse position
        int dir = mousePosition.x >= transform.position.x ? 1 : -1;
        sprite.transform.localScale = new Vector3(spriteScale.x * dir, spriteScale.y);
        weapon.transform.localScale = new Vector3(weapon.scale.x * dir, weapon.scale.y);

        // rotate weapon
        weapon.transform.localRotation = Quaternion.Euler(0, 0, GetMouseRotation());
    }

    private void Attack() {
        if (Input.GetButtonDown("Attack")) {
            weapon.Attack();
        }
    }

    private float GetMouseRotation() {
        Vector2 mouseDir = mousePosition - transform.position;
        float rot = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;

        // clamp to (-90; 90) range to compensate for sprite flipping
        if (rot < -90 || rot > 90)
            rot += 180;
        return rot;
    }
    #endregion
}
