using DG.Tweening;
using UnityEngine;

public class Column : Entity {
    public int HP;
    public int ID;

    public AudioSource shake;
    public AudioSource hit;
    
    private bool destroyed;

    private SpriteRenderer sprite;
    private Animator animator;
    private ParticleSystem dustParticles;

    void Awake () {
        animator = GetComponent<Animator>();
        animator.SetFloat("HP", HP);
        dustParticles = transform.Find("DustParticles").GetComponent<ParticleSystem>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
    
    void Update () {
        animator.SetFloat("HP", HP);
        
        if (HP <= 0 && !destroyed) {
            destroyed = true;
            Destroy(gameObject, 2f);
            Map.instance.DemolishChunk(ID);
            Map.instance.columnsCount--;
            transform.DOShakePosition(10f, 0.1f);
            sprite.material.DOFade(0f, 1.8f);
            shake.Play();
        }
    }

    // TODO: add custom damage
    public void Damage() {
        if (HP > 0) {
            HP--;
            hit.pitch = UnityEngine.Random.Range(0.80f, 1.10f);
            hit.Play();
            dustParticles.Play();         
        }
    }
}
