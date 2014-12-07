using UnityEngine;

public class Bat : Enemy {
    #region Variables
    private Vector2 dirDelayRange = new Vector2(1f, 2f);
    private Vector2 dirDurationRange = new Vector2(1f, 2f);

    private Vector2 dir;
    private Vector2 prevDir;
    private float dirTimer;
    private float dirDelay;
    private float dirDuration;

    //private Vector2 playerDir;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
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
            dir = dir.normalized;

            // modify speed
            dir.x *= movementSpeed.x;
            dir.y *= movementSpeed.y;
            dir *= Time.deltaTime;

            // randomize duration
            dirTimer = 0;
            dirDelay = Random.Range(dirDelayRange.x, dirDelayRange.y);
            dirDuration = Random.Range(dirDurationRange.x, dirDurationRange.y);
        }
        
        // apply force while lerping between old and new directions, with a small delay between direction changes
        if (dirTimer < dirDuration) {
            rigidbody2D.AddForce(MainDebug.LerpVector2(prevDir, dir, dirTimer / dirDuration), ForceMode2D.Impulse);
        }
        else {
            dir = MainDebug.LerpVector2(dir, Vector2.zero, (dirTimer - dirDuration) / (dirDelay - dirDuration));
            rigidbody2D.AddForce(MainDebug.LerpVector2(prevDir, dir, dirTimer), ForceMode2D.Impulse);
        }

        dirTimer += Time.deltaTime;

        Debug.DrawRay(transform.position, MainDebug.LerpVector2(prevDir, dir, dirTimer), Color.red);      //DEBUG: direction
    }
    #endregion
}
