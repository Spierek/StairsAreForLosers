using System;
using UnityEngine;

public class Weapon : MonoBehaviour {
    #region Variables
    private float attackDelay = 0.3f;

    private float attackTimer;
    [NonSerialized] public Vector3 scale;

    private WeaponHitbox hitbox;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        hitbox = transform.Find("Hitbox").GetComponent<WeaponHitbox>();
        scale = transform.localScale;
    }
    
    void Update () {
        attackTimer += Time.deltaTime;
    }
    #endregion

    #region Methods
    public void Attack() {
        if (attackTimer > attackDelay) {
            hitbox.Attack(0.1f);
        }
    }
    #endregion
}
