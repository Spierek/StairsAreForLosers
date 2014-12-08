using UnityEngine;

public class ZombieExplosion : MonoBehaviour {
    #region Variables
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        collider2D.enabled = false;
    }
    
    void OnTriggerEnter2D (Collider2D col) {
        if (col.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox")) {
            col.transform.parent.GetComponent<Enemy>().Hit(1, (transform.position - col.transform.position).normalized);
        }
        if (col.gameObject.layer == LayerMask.NameToLayer("Column")) {
            col.transform.parent.GetComponent<Column>().Damage();
        }
    }
    #endregion

    #region Methods
    public void EnableCollider() {
        collider2D.enabled = true;
        Invoke("DisableCollider", 1f);
    }

    public void DisableCollider() {
        collider2D.enabled = false;
    }
    #endregion
}
