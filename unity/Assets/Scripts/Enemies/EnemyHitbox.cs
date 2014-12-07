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

    private void OnTriggerEnter2D() {
        MainDebug.WriteLine("WAS HIT!", 2f);
        enemy.Pushback();
    }
    #endregion

    #region Methods
    #endregion
}
