using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyHitbox : MonoBehaviour {
    #region Variables
    private Enemy enemy;
    private new Collider2D collider2D;
    #endregion

    #region Monobehaviour Methods
    private void Awake() {
        enemy = transform.parent.GetComponent<Enemy>();
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // TODO: possibly redundant check since EnemyHitbox can only collide with weapons
        if (col.gameObject.layer == LayerMask.NameToLayer("Weapon")) {
            enemy.Hit(col.GetComponent<WeaponHitbox>().damage);
        }
    }
    #endregion

    #region Methods
    #endregion
}
