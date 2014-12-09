using UnityEngine;

public enum SpiderState {
    Follow, DropDown, Idle, Attack, Idle2, WasHit, PullUp, Dead
}

public class Spider : Enemy {
    #region Variables
    public GameObject projectile;

    private float projectileSpeed = 3f;
    private Vector2 followDurationRange = new Vector2(2f, 5f);
    private Vector2 idleDurationRange = new Vector2(1f, 1.5f);

    private SpiderState spiderState;
    private Vector2 dir;
    private float timer;
    private float followDuration;
    private float idleDuration;

    private ParticleSystem deathParticle;
    private SpriteRenderer shadow;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        spiderState = SpiderState.Follow;

        followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
        deathParticle = transform.Find("DeathParticles").GetComponent<ParticleSystem>();
        shadow = transform.Find("SpriteContainer").Find("Shadow").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Methods
    protected override void CheckState() {
        //MainDebug.WriteLine("timer", timer.ToString());
        //MainDebug.WriteLine("follow", followDuration.ToString());
        MainDebug.WriteLine("state", spiderState.ToString());

        // convert base enemy state to spider state
        if (state == EnemyState.WasHit) {
            spiderState = SpiderState.WasHit;
            animator.SetInteger("state", (int)spiderState);
            state = EnemyState.Idle;
            hitParticles.Play();
        }
        else if (state == EnemyState.Dead && spiderState != SpiderState.Dead) {
            rigidbody2D.AddForce(pushbackForce * 0.7f, ForceMode2D.Impulse);
            spiderState = SpiderState.Dead;
        }
        // spider state machine
        else {
            switch (spiderState) {
                case SpiderState.Follow:
                    // change layer to avoid colliding with the player
                    gameObject.layer = LayerMask.NameToLayer("SpiderFollow");

                    // check if the spider should drop down
                    if (timer > followDuration) {
                        timer = 0;
                        spiderState = SpiderState.DropDown;
                        followDuration = Random.Range(followDurationRange.x, followDurationRange.y);
                    }
                    break;
                case SpiderState.DropDown:
                    spiderState = SpiderState.Idle;
                    break;
                case SpiderState.Idle:
                    // change the layer after a short delay
                    if (timer > 0.1f) {
                        gameObject.layer = LayerMask.NameToLayer("Enemy");
                    }

                    // check if the spider should attack
                    if (timer > idleDuration) {
                        timer = 0.2f;
                        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
                        spiderState = SpiderState.Attack;
                    }
                    break;
                case SpiderState.Attack:
                    Attack();
                    spiderState = SpiderState.Idle2;
                    break;
                case SpiderState.Idle2:
                    // check if the spider should pull up
                    if (timer > idleDuration) {
                        timer = 0.5f;
                        idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
                        spiderState = SpiderState.PullUp;
                    }
                    break;
                case SpiderState.WasHit:
                    timer = 0; // FIXME: allows the spider to pull up and move at the same time
                    idleDuration = Random.Range(idleDurationRange.x, idleDurationRange.y);
                    spiderState = SpiderState.PullUp;
                    break;
                case SpiderState.PullUp:
                    if (timer > 0.5f) {
                        spiderState = SpiderState.Follow;
                        timer = 0;

                        // choose a random direction
                        dir = Random.insideUnitSphere;
                        dir = dir.normalized;

                        // modify speed
                        dir.x *= movementSpeed.x;
                        dir.y *= movementSpeed.y;
                        dir *= Time.deltaTime;
                    }
                    break;
            }
        }

        animator.SetInteger("state", (int)spiderState);
        timer += Time.deltaTime;
    }

    protected override void Movement() {
        if (spiderState == SpiderState.Follow) {
            // apply force
            rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
            Debug.DrawRay(transform.position, dir, Color.red);      //DEBUG: direction
        }
    }

    protected override void Rotate() {
        // flip sprite
        int flip = PlayerController.Instance.transform.position.x >= transform.position.x ? 1 : -1;
        sprite.transform.parent.localScale = new Vector3(spriteScale.x * flip, spriteScale.y);
    }

    protected override void Attack() {
        GameObject go = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
        go.GetComponent<SpiderProjectile>().Launch((PlayerController.Instance.transform.position - transform.position).normalized * projectileSpeed);
    }

    protected override void Die() {
        base.Die();
        Invoke("Explode", 2f);
    }

    private void Explode() {
        deathParticle.Play();
        sprite.enabled = false;
        shadow.enabled = false;
    }
    #endregion
}
