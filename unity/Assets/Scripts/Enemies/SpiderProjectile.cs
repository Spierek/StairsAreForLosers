using System.Linq.Expressions;
using UnityEngine;

public class SpiderProjectile : MonoBehaviour {
    #region Variables
    private float rotationSpeed = 180f;
    private SpriteRenderer      sprite;
    private ParticleSystem      slimeParticle;
    private ParticleSystem      deathParticle;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        slimeParticle = transform.Find("Slime").GetComponent<ParticleSystem>();
        deathParticle = transform.Find("Death").GetComponent<ParticleSystem>();
    }
    
    void Update () {
        sprite.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Column") ||
            col.gameObject.layer == LayerMask.NameToLayer("Obstacle") ||
            col.gameObject.layer == LayerMask.NameToLayer("Weapon")) {
            Die();
        }
        else if (col.gameObject.layer == LayerMask.NameToLayer("PlayerHitbox")) {
            PlayerController.Instance.Hit(1, transform.position, 0.1f);
            Die();
        }
    }
    #endregion

    #region Methods
    public void Launch(Vector2 force) {
        rigidbody2D.AddForce(force, ForceMode2D.Impulse);
    }

    private void Die() {
        sprite.enabled = false;
        collider2D.enabled = false;
        Destroy(gameObject, 0.5f);
        deathParticle.Play();
        slimeParticle.enableEmission = false;
    }
    #endregion
}
