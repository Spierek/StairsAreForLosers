using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour {
    #region Variables
    private bool invincible;
    #endregion

    #region Monobehaviour Methods
    private void OnTriggerEnter2D(Collider2D col) {
        if (!invincible) {
            // TODO: add enemy damage
            PlayerController.Instance.Hit(1, col.transform.position);
            DisableCollider();
        }
        
        if (col.gameObject.layer == LayerMask.NameToLayer("Projectile")) {
            col.GetComponent<SpiderProjectile>().Die();
        }
    }
    #endregion

    #region Methods
    public void DisableCollider() {
        invincible = true;
        Invoke("EnableCollider", PlayerController.Instance.invincibilityDuration);   
    }

    private void EnableCollider() {
        invincible = false;
    }
    #endregion
}
