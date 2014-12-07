using DG.Tweening;
using UnityEngine;

public enum EnemyState {
    Idle, Follow, Attack, WasHit, Dead
};

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Entity {
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
        CheckState();

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
    protected virtual void CheckState() {
        
    }

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
        rigidbody2D.AddForce(pushbackForce, ForceMode2D.Impulse);

        pushbackTimer = 0;
        state = EnemyState.WasHit;

        // set sprite color to red
        sprite.material.color = new Color(1f, 0.3f, 0.3f, 1f);
        sprite.material.DOColor(Color.white, 0.5f);
    }

    protected virtual void Pushback() {
        pushbackTimer += Time.deltaTime;

        if (pushbackTimer > pushbackDuration) {
            state = EnemyState.Follow;
        }
    }

    protected virtual void Die() {
        MainDebug.WriteLine("ded", 2f);
        Destroy(gameObject);
    }
    #endregion
}
