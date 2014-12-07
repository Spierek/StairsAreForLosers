using UnityEngine;

public class SpookySkeleton : Enemy {
    #region Variables
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        movementSpeed = new Vector2(500f, 300f);
        pushbackMod = 0.1f;
        pushbackDuration = 0.2f;
        state = EnemyState.Follow;
    }
    #endregion

    #region Methods
    protected override void Movement() {
        Vector2 dir = (PlayerController.Instance.transform.position - transform.position).normalized * Time.deltaTime;
        dir.x *= movementSpeed.x;
        dir.y *= movementSpeed.y;
        rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
    }
    #endregion
}
