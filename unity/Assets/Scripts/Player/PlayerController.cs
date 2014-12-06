using UnityEngine;

public class PlayerController : MonoBehaviour {
    #region Variables
    private Vector2 speed = new Vector2(2.5f, 2f);

    private Vector3 mousePosition;
    private Vector3 spriteScale;

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
        // get input and apply movement to rigidbody
        Vector2 vel = new Vector2(Input.GetAxis("Horizontal") * speed.x, Input.GetAxis("Vertical") * speed.y);
        rigidbody2D.velocity = vel;
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
