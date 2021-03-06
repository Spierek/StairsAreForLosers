﻿using DG.Tweening;
using UnityEngine;

public class Zombie : Enemy {
    #region Variables
    private float attackRange = 0.5f;
    private float attackDelay = 1f;
    private float attackTimer;

    private ZombieExplosion explosion;
    private ParticleSystem explosionParticle;
    private SpriteRenderer shadow;
    #endregion

    #region Monobehaviour Methods
    protected override void Awake() {
        base.Awake();
        state = EnemyState.Follow;
        attackTimer = attackDelay;

        explosion = transform.Find("Explosion").GetComponent<ZombieExplosion>();
        explosionParticle = explosion.transform.Find("Particles").GetComponent<ParticleSystem>();
        shadow = transform.Find("SpriteContainer").Find("Shadow").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Methods
    protected override void CheckState() {
        base.CheckState();
    }

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
        MainDebug.WriteLine("ded", 2f);
        state = EnemyState.Dead;
        animator.SetBool("isDead", true);
        sprite.material.DOColor(new Color(0.5f, 0.5f, 0.5f, 1f), 0.5f);
        if (hitParticles.isStopped)
            hitParticles.Play();

        Invoke("Explode", 3f);
        DropPickup();
        Destroy(gameObject, 6f);
    }

    private void Explode() {
        // particles
        explosionParticle.Play();
        explosion.EnableCollider();
        sprite.enabled = false;
        shadow.enabled = false;
    }
    #endregion
}
