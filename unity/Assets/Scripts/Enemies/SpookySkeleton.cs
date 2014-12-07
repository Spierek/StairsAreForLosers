using UnityEngine;

public class SpookySkeleton : Enemy {
    #region Variables
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        movementSpeed = new Vector2(500f, 300f);
        health = 1.2f;
        pushbackMod = 0.1f;
        pushbackDuration = 0.2f;
        state = EnemyState.Follow;
    }
    #endregion

    #region Methods
    protected override void Movement() {
        // get direction to player
        Vector2 playerDist = PlayerController.Instance.transform.position - transform.position;
        Vector2 dir = playerDist.normalized;

        // raycast to check for obstacles
        // TODO: well, this didn't work too well
        //Vector2 colliderPosition = collider2D.bounds.center;
        //RaycastHit2D hit = Physics2D.Raycast(colliderPosition, dir, 1, 1 << LayerMask.NameToLayer("EnemyObstacle"));
        //if (hit.collider != null) {
        //    // raycast both sides by 45deg and see if any of them is free
        //    Vector2 dirLeft = Quaternion.Euler(0, 0, 75) * dir;
        //    Vector2 dirRight = Quaternion.Euler(0, 0, -75) * dir;

        //    Debug.DrawRay(colliderPosition, dirLeft, Color.green);
        //    Debug.DrawRay(colliderPosition, dirRight, Color.green);

        //    RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, dir, 1, 1 << LayerMask.NameToLayer("EnemyObstacle"));
        //    RaycastHit2D hitRight = Physics2D.Raycast(transform.position, dir, 1, 1 << LayerMask.NameToLayer("EnemyObstacle"));

        //    // if both cases are empty, find the one closer to the player
        //    if ((hitLeft.collider != null && hitRight.collider != null) || (hitLeft.collider == null && hitRight.collider == null)) {
        //        dir = (playerDist + dirLeft).magnitude < (playerDist + dirRight).magnitude ? dirLeft : dirRight;
        //        MainDebug.WriteLine("dir closer", dir.ToString());
        //    }
        //    else if (hitLeft.collider != null) {
        //        dir = dirLeft;
        //        MainDebug.WriteLine("dir left", dir.ToString());
        //    }
        //    else if (hitRight.collider != null) {
        //        dir = dirRight;
        //        MainDebug.WriteLine("dir right", dir.ToString());
        //    }
        //}

        //// DEBUG: direction
        //Debug.DrawRay(colliderPosition, dir, Color.red);

        // modify speed
        dir.x *= movementSpeed.x;
        dir.y *= movementSpeed.y;
        dir *= Time.deltaTime;
        rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
    }
    #endregion
}
