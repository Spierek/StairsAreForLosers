using UnityEngine;
using System.Collections;

using DG.Tweening;

public class Tile : Entity {

    public Vector2 location;
    public float lateParticleEffect;
    public float spawnTileTimer = 2f;
    public float deathTimer = 3f;
    public ParticleSystem particles;
    public GameObject succesor;
    public GameObject precedesor;
    public bool spawnColumn;
    private bool isDying;

    private bool deathParticlesPlayed;
    public void initDeath()
    {
        isDying = true;
        lateParticleEffect = Random.Range(0f, 1f);
        spawnTileTimer = lateParticleEffect + 1f;
    }

    protected override void OnFallen()
    {
        Destroy(precedesor);
    }

	// Use this for initialization
	protected override void Start () {
        base.Start();
        particles = GetComponentInChildren<ParticleSystem>();            
	}
	
	// Update is called once per frame
	void Update () {
        	if(isDying)
            {
                deathTimer -= Time.deltaTime;
                spawnTileTimer -= Time.deltaTime;
                if(lateParticleEffect>0)
                {
                    lateParticleEffect -= Time.deltaTime;
                }
                else if(!deathParticlesPlayed)
                {
                    particles.Play();
                    deathParticlesPlayed = true;
                }
                if(spawnTileTimer <= 0 && succesor == null)
                {
                    succesor = Instantiate(Map.instance.tilePrefab, particles.gameObject.transform.position, Quaternion.identity) as GameObject;
                    succesor.transform.parent = this.transform.parent;
                    succesor.GetComponent<Tile>().precedesor = this.gameObject;
                    succesor.GetComponent<Tile>().finalPosition = this.transform.position;
                    succesor.name = this.gameObject.name;
                }
            }
	}
}
