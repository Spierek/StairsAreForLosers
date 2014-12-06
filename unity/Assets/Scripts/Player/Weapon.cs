using System;
using UnityEngine;

public class Weapon : MonoBehaviour {
    #region Variables
    protected float attackDelay = 0.3f;

    protected float attackTimer;
    [NonSerialized] public Vector3 scale;

    protected WeaponHitbox  hitbox;
    protected Animator      animator;
    #endregion

    #region Monobehaviour Methods
    protected virtual void Awake () {
        hitbox = transform.Find("Hitbox").GetComponent<WeaponHitbox>();
        scale = transform.localScale;
        animator = GetComponent<Animator>();
    }
    
    protected virtual void Update () {
        attackTimer += Time.deltaTime;
        if (attackTimer > attackDelay) {
            animator.SetBool("isAttacking", false);
        }
    }
    #endregion

    #region Methods
    public void Attack() {
        if (attackTimer > attackDelay) {
            hitbox.Attack(0.1f);
            attackTimer = 0;
            animator.SetBool("isAttacking", true);
        }
    }
    #endregion
}
