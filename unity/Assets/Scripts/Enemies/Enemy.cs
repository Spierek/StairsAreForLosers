using UnityEngine;

public enum EnemyState {
    Idle, Follow, Attack, WasHit, Dead
};

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour {
    #region Variables
    protected Vector2           movementSpeed;
    protected float             health;
    protected float             pushbackMod;
    protected float             pushbackDuration;

    protected EnemyState        state;
    protected Vector3           spriteScale;
    protected Vector3           pushbackForce;
    protected float             pushbackTimer;

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
        if (state == EnemyState.WasHit) {
            Pushback();
        }
        else {
            Movement();
            Rotate(); 
        }
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

    public virtual void Hit(float damage) {
        // apply damage
        health -= damage;
        if (health < 0) {
            Die();
            return;
        }

        // calculate force
        Vector2 mouseDir = PlayerController.Instance.GetMouseDirection();
        pushbackForce = -new Vector2(movementSpeed.x * mouseDir.x, movementSpeed.y * mouseDir.y) * pushbackMod;

        pushbackTimer = 0;
        state = EnemyState.WasHit;

        // set sprite color to red
        sprite.color = new Color(1f, 0.3f, 0.3f, 1f);
    }

    protected virtual void Pushback() {
        // apply force
        rigidbody2D.AddForce(pushbackForce, ForceMode2D.Impulse);
        pushbackForce -= pushbackForce * Time.deltaTime / pushbackDuration;
        pushbackTimer += Time.deltaTime;

        if (pushbackTimer > pushbackDuration) {
            state = EnemyState.Follow;
        }

        // slowly reset sprite color
        float fade = Time.deltaTime * 3f;
        sprite.color = new Color(1f, sprite.color.g + fade, sprite.color.b + fade, 1f);
    }

    protected virtual void Die() {
        MainDebug.WriteLine("ded", 2f);
        Destroy(gameObject);
    }
    #endregion
}
