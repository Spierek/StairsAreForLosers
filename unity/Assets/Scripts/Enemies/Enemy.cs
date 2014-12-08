using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyState {
    Idle, Follow, Attack, WasHit, Dead
};

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : Entity {
    #region Variables
    public Vector2              movementSpeed;
    public float                health;
    public float                pushbackMod;
    public float                pushbackDuration;

    protected EnemyState        state;
    protected Vector3           spriteScale;
    protected Vector3           pushbackForce;
    protected float             pushbackTimer;

    protected Animator          animator;
    protected new Rigidbody2D   rigidbody2D;
    protected SpriteRenderer    sprite;
    protected ParticleSystem    hitParticles;
    #endregion

    #region Monobehaviour Methods
    protected virtual void Awake () {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        sprite = transform.Find("SpriteContainer").Find("Sprite").GetComponent<SpriteRenderer>();
        spriteScale = sprite.transform.parent.localScale;
        hitParticles = transform.Find("HitParticles").GetComponent<ParticleSystem>();
    }
    
    protected virtual void Update () {
        CheckState();

        if (state != EnemyState.Dead) {
            if (state == EnemyState.WasHit) {
                Pushback();
                if (hitParticles.isStopped)
                    hitParticles.Play();
            }
            else {
                Movement();
                Rotate(); 
            }
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
        sprite.transform.parent.localScale = new Vector3(spriteScale.x * dir, spriteScale.y);
    }

    protected virtual void Attack() {
        
    }

    public virtual void Hit(float damage, Vector2 direction) {
        // calculate force
        pushbackForce = -new Vector2(movementSpeed.x * direction.x, movementSpeed.y * direction.y);
        rigidbody2D.AddForce(pushbackForce * pushbackMod, ForceMode2D.Impulse);

        pushbackTimer = 0;
        state = EnemyState.WasHit;

        // set sprite color to red
        sprite.material.color = new Color(1f, 0.3f, 0.3f, 1f);
        sprite.material.DOColor(Color.white, 0.5f);

        // apply damage
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    protected virtual void Pushback() {
        pushbackTimer += Time.deltaTime;

        if (pushbackTimer > pushbackDuration) {
            state = EnemyState.Follow;
        }
    }

    protected virtual void Die() {
        MainDebug.WriteLine("ded", 2f);
        state = EnemyState.Dead;
        animator.SetBool("isDead", true);
        sprite.material.DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), 0.5f);
        if (hitParticles.isStopped)
            hitParticles.Play();

        Destroy(gameObject, 3f);
    }
    #endregion
}
