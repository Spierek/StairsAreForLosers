using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour {
    #region Variables
    private new Collider2D collider2D;
    #endregion

    #region Monobehaviour Methods
    private void Awake() {
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // TODO: add enemy damage
        PlayerController.Instance.Hit(1, col.transform.position);
        collider2D.enabled = false;
        Invoke("EnableCollider", PlayerController.Instance.invincibilityDuration);

        
        if (col.gameObject.layer == LayerMask.NameToLayer("Projectile")) {
            col.GetComponent<SpiderProjectile>().Die();
        }
    }
    #endregion

    #region Methods
    private void EnableCollider() {
        collider2D.enabled = true;
    }
    #endregion
}
