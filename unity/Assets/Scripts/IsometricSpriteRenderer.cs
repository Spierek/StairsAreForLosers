﻿using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricSpriteRenderer : MonoBehaviour {
    #region Variables
    public int extraOffset;
    #endregion

    #region Monobehaviour Methods
    void Update () {
        renderer.sortingOrder = 10000 - (int)(transform.position.y * 10) + extraOffset;
    }
    #endregion
}
