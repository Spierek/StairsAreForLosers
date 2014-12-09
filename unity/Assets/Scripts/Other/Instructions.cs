using DG.Tweening;
using UnityEngine;

public class Instructions : MonoBehaviour {
    #region Variables
    public Sprite A;
    public Sprite B;

    private float introDelay = 2f;
    private float outroDelay = 1.5f;
    private float timer;
    private float spriteChangeDelay = 0.5f;
    private float spriteChangeTimer;

    private bool isOutro;
    private bool isSpriteA;

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

        if (spriteChangeTimer > spriteChangeDelay) {
            sprite.sprite = isSpriteA ? B : A;
            isSpriteA = !isSpriteA;
            spriteChangeTimer = 0;
        }

        timer += Time.deltaTime;
        spriteChangeTimer += Time.deltaTime;
    }
    #endregion
}
