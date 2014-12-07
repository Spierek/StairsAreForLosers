using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Hitbox : MonoBehaviour {
    #region Variables
    private PlayerController player;
    private new Collider2D collider2D;
    #endregion

    #region Monobehaviour Methods
    private void Awake() {
        player = transform.parent.GetComponent<PlayerController>();
        collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // TODO: add enemy damage
        player.Hit(1, col.transform.position);
    }
    #endregion

    #region Methods
    #endregion
}
