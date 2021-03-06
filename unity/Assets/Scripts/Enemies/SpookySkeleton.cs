﻿using UnityEngine;
using UnityEngine.UI;

public class SpookySkeleton : Enemy {
    #region Variables
    private float attackRange = 0.3f;
    private float attackDelay = 1f;
    private float attackTimer;

    private ParticleSystem deathParticle;
    private SpriteRenderer shadow;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        state = EnemyState.Follow;
        attackTimer = attackDelay;
        deathParticle = transform.Find("DeathParticles").GetComponent<ParticleSystem>();
        shadow = spriteContainer.Find("Shadow").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Methods
    protected override void CheckState() {
        base.CheckState();
    }

    protected override void Movement() {
        // HACK: keep sprite container where it should be
        spriteContainer.localPosition = Vector2.zero;
        
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

        // attack when close to the player
        if (playerDist.magnitude <= attackRange) {
            if (attackTimer > attackDelay) {
                attackTimer = 0;
                Attack();
            }
        }
        attackTimer += Time.deltaTime;
    }

    protected override void Attack() {
        PlayerController.Instance.Hit(1, transform.position, 0.7f);
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
