using System;
using UnityEngine;

public class WeaponHitbox : MonoBehaviour {
    #region Variables
    private new Collider2D collider2D;
    [NonSerialized] public float damage;
    #endregion

    #region Monobehaviour Methods
    private void Awake() {
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D coll) {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Column")) {
            coll.transform.parent.gameObject.GetComponent<Column>().Damage();
            MainDebug.WriteLine("Column hit!", 2f);
        }
    }
    #endregion

    #region Methods
    public void Attack(float activeTime) {
        collider2D.enabled = true;
        Invoke("StopAttack", activeTime);
    }

    public void StopAttack() {
        collider2D.enabled = false;
    }
    #endregion
}
