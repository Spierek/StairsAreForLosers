using UnityEngine;
using System.Collections;

using DG.Tweening;

public class Tile : MonoBehaviour {

    public Vector2 location;
    public float lateParticleEffect;
    public float spawnTileTimer = 2f;
    public float deathTimer = 3f;
    public ParticleSystem particles;
    public GameObject succesor;
    public GameObject precedesor;
    public bool doNotTween;
    public Vector3 finalPosition;
    public bool spawnColumn;
    private bool isDying;

    private bool deathParticlesPlayed;
    public void initDeath()
    {
        isDying = true;
        lateParticleEffect = Random.Range(0f, 1f);
        spawnTileTimer = lateParticleEffect + 1f;
    }

    public void TileFallen()
    {
        Destroy(precedesor);
    }

	// Use this for initialization
	void Start () {
        particles = GetComponentInChildren<ParticleSystem>();
        if (!doNotTween)
        {
            transform.DOMove(finalPosition, 0.5f).OnComplete(TileFallen).SetEase(Ease.InQuint);
            GetComponent<SpriteRenderer>().material.DOColor(new Color(1f,1f,1f,1f), 0.5f);
        }
            
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
