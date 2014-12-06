using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricSpriteRenderer : MonoBehaviour {
	#region Variables
	#endregion

	#region Monobehaviour Methods
	void Update () {
	    renderer.sortingOrder = 10000 - (int)(transform.position.y * 10);
	}
	#endregion
}
