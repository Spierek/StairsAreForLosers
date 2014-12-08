using System.Linq.Expressions;
using UnityEngine;

public class SpiderProjectile : MonoBehaviour {
    #region Variables
    private float rotationSpeed = 180f;
    private SpriteRenderer      sprite;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
    
    void Update () {
        sprite.transform.Rotate(0, 0, Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("Column") ||
            col.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
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
        Destroy(gameObject);
        // TODO: add some particles on projectile death
    }
    #endregion
}
