using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Variables
    private Vector2 movementSpeed = new Vector2(30f, 21f);
    private float   dashMod = 3f;
    private float   dashDuration = 0.3f;
    private float   dashDelay = 0.5f;

    private Vector3 mousePosition;
    private Vector3 spriteScale;
    private Vector2 dashForce;
    private float   dashTimer;

    private Animator        animator;
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer  sprite;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteScale = sprite.transform.localScale;

        dashTimer = dashDelay;
    }
    
    void Update () {
        Movement();
        Rotation();
    }

    void OnDrawGizmos() {
        // draw mouse direction
        Gizmos.color = Color.cyan;
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

        // rotate sprite based on mouse position
        int dir = mousePosition.x >= transform.position.x ? 1 : -1;
        sprite.transform.localScale = new Vector3(spriteScale.x * dir, spriteScale.y);
    }
    #endregion
}
