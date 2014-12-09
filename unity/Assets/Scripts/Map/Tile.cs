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

    protected override void OnStartFalling()
    {
        if (precedesor != null) precedesor.GetComponentInChildren<SpriteRenderer>().material.DOColor(new Color(0.5f, 0.5f, 0.5f), 0.75f);
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
                    succesor = Instantiate(Map.instance.tilePrefab, transform.position, Quaternion.identity) as GameObject;
                    succesor.transform.parent = this.transform.parent;
                    Tile succesorTile = succesor.GetComponent<Tile>();
                    succesorTile.precedesor = this.gameObject;
                    succesorTile.finalPosition = this.transform.position;
                    succesorTile.location = this.location;
                    succesor.GetComponentInChildren<SpriteRenderer>().color = Map.instance.GetIndexColor(Map.floors[0].colorMap[(int)location.x, (int)location.y]);
                    succesor.name = this.gameObject.name;

                    GameObject entityToSpawn;
                    switch(Map.floors[0].entitiesMap[(int)location.x, (int)location.y])
                    {
                        default: entityToSpawn = null; break;
                        case 1: entityToSpawn = Map.instance.columnPrefab; break;
                        case 2: entityToSpawn = Resources.Load("Prefabs/Enemies/Spooky Skeleton") as GameObject; break;
                        case 3: entityToSpawn = Resources.Load("Prefabs/Enemies/Bat") as GameObject; break;
                        case 4: entityToSpawn = Resources.Load("Prefabs/Enemies/Zombie") as GameObject; break;
                        case 5: entityToSpawn = Resources.Load("Prefabs/Enemies/Spider") as GameObject; break;
                    }
                    if (entityToSpawn != null)
                    {
                        GameObject newEntity = Instantiate(entityToSpawn, transform.position, Quaternion.identity) as GameObject;
                        if (newEntity.GetComponent<Enemy>() != null)
                        {
                            newEntity.GetComponent<Enemy>().finalPosition = this.transform.position;
                            newEntity.GetComponent<Enemy>().delayTween = true;
                            newEntity.name = entityToSpawn.name;
                            Map.instance.enemiesCount++;
                        }
                        else
                        {
                            newEntity.GetComponent<Column>().finalPosition = this.transform.position;
                            newEntity.GetComponent<Column>().delayTween = true;
                            newEntity.GetComponent<Column>().ID = Map.floors[0].columnsMap[(int)location.x, (int)location.y];
                            Map.instance.columnsCount++;
                            newEntity.name = entityToSpawn.name;
                            
                        }
                    }
                }
            }
	}
}
