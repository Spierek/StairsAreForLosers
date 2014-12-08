using UnityEngine;
using System.Collections;

public class TileHitbox : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("EnemyHitbox"))
        {
            col.gameObject.GetComponent<Enemy>().Hit(2, Vector3.zero);
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
