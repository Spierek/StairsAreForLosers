using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    #region Variables
    protected Vector2           movementSpeed;

    protected Vector3           spriteScale;

    protected new Rigidbody2D   rigidbody2D;
    protected SpriteRenderer    sprite;

    #endregion

    #region Monobehaviour Methods
    protected virtual void Awake () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        spriteScale = sprite.transform.localScale;
    }
    
    protected virtual void Update () {
        Movement();
        Rotate();
    }
    #endregion

    #region Methods
    protected virtual void Movement() {
        
    }

    protected virtual void Rotate() {
        // flip sprite
        int dir = rigidbody2D.velocity.x >= 0 ? 1 : -1;
        sprite.transform.localScale = new Vector3(spriteScale.x * dir, spriteScale.y);
    }

    protected virtual void Attack() {
        
    }
    #endregion
}
