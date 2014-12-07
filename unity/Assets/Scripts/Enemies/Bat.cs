using UnityEngine;

public class Bat : Enemy {
    #region Variables
    private Vector2 dirDelayRange = new Vector2(1f, 2f);
    private Vector2 dirDurationRange = new Vector2(1f, 2f);
    private Vector2 dirLerpDurationRange = new Vector2(0.3f, 0.5f);

    private Vector2 dir;
    private Vector2 prevDir;
    private float dirTimer;
    private float dirDelay;
    private float dirDuration;
    private float dirLerpDuration;

    //private Vector2 playerDir;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        movementSpeed = new Vector2(800f, 600f);
        health = 1.2f;
        pushbackMod = 0.15f;
        pushbackDuration = 0.2f;
        state = EnemyState.Idle;

        dirTimer = dirDelayRange.y;
    }
    #endregion

    #region Methods
    protected override void CheckState() {
        //if ()
    }

    protected override void Movement() {
        // TODO: check if player is close, attack him if yes
        // choose a random direction
        // TODO: check if enemy doesn't try to run straight into a wall :V
        if (dirTimer > dirDelay) {
            prevDir = dir;
            dir = Random.insideUnitSphere;

            // modify speed
            dir.x *= movementSpeed.x;
            dir.y *= movementSpeed.y;
            dir *= Time.deltaTime;

            // randomize duration
            dirTimer = 0;
            dirDelay = Random.Range(dirDelayRange.x, dirDelayRange.y);
            dirDuration = Random.Range(dirDurationRange.x, dirDurationRange.y);
            dirLerpDuration = Random.Range(dirLerpDurationRange.x, dirLerpDurationRange.y);
        }
        
        // apply force while lerping between old and new directions, with a small delay between direction changes
        if (dirTimer < dirDuration) {
            rigidbody2D.AddForce(MainDebug.LerpVector2(prevDir, dir, dirTimer / dirLerpDuration), ForceMode2D.Impulse);
        }
        // TODO: add some dicking around when standing still

        dirTimer += Time.deltaTime;

        Debug.DrawRay(transform.position, MainDebug.LerpVector2(prevDir, dir, dirTimer / dirLerpDuration), Color.red);      //DEBUG: direction
    }
    #endregion
}
