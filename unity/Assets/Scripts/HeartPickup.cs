using UnityEngine;

public class HeartPickup : MonoBehaviour {
    #region Variables
    public AudioSource grab;
    private ParticleSystem particles;
    private SpriteRenderer sprite;
    #endregion

    #region Monobehaviour Methods
    void Awake () {
        particles = transform.Find("Particles").GetComponent<ParticleSystem>();
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Methods
    public void Grab() {
        particles.Play();
        grab.Play();
        sprite.enabled = false;
        collider2D.enabled = false;
        Destroy(gameObject, 2f);
    }

    public void Spawn() {
        Vector2 force = Random.insideUnitCircle;
        rigidbody2D.AddForce(force.normalized * 3, ForceMode2D.Impulse);
    }
    #endregion
}
