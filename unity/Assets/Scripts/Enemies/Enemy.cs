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
    public Vector2              movementSpeed;
    public float                health;
    public float                pushbackMod;
    public float                pushbackDuration;
    public AudioSource          hit;

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

        hit.pitch = UnityEngine.Random.Range(0.80f, 1.10f);
        hit.Play();

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
        state = EnemyState.Dead;
        animator.SetBool("isDead", true);
        sprite.material.DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), 0.5f);
        if (hitParticles.isStopped)
            hitParticles.Play();

        DropPickup();
        Destroy(gameObject, 3f);
    }

    protected void DropPickup() {
        if (Random.Range(0f, 1f) < 0.2f) {
            GameObject go = Resources.Load("Prefabs/HeartPickup") as GameObject;
            go = Instantiate(go, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<HeartPickup>().Spawn();  
        }
    }
    #endregion
}
