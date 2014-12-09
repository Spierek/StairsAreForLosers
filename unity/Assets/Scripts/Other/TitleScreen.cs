using DG.Tweening;
using UnityEngine;

public class TitleScreen : MonoBehaviour {
    #region Variables
    private float introDelay = 2f;
    private float outroDelay = 1.5f;
    private float timer;

    private bool isOutro;

    private SpriteRenderer sprite;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        sprite = GetComponent<SpriteRenderer>();
        sprite.material.color = Color.black;
        sprite.material.DOColor(Color.white, introDelay);
    }
    
    void Update () {
        if (!isOutro && timer > introDelay && (Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))) {
            isOutro = true;
            timer = 0;
            sprite.material.DOColor(Color.black, outroDelay);
        }

        if (isOutro && timer > outroDelay) {
            Application.LoadLevel(Application.loadedLevel + 1);
        }

        timer += Time.deltaTime;
    }
    #endregion
}
