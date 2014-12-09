using UnityEngine;
using System.Collections;

public class TileHitbox : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            Enemy enemy = col.transform.parent.gameObject.GetComponent<Enemy>();
            if(enemy!=null) if(!enemy.isFalling) enemy.Hit(2, Vector3.zero);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
