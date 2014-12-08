﻿using UnityEngine;

public class Column : Entity {
    public int HP;
    public int ID;

    private bool destroyed;

    private Animator animator;
    private ParticleSystem dustParticles;

    void Awake () {
        animator = GetComponent<Animator>();
        animator.SetFloat("HP", HP);
        dustParticles = transform.Find("DustParticles").GetComponent<ParticleSystem>();
    }
    
    void Update () {
        animator.SetFloat("HP", HP);
        
        if (HP <= 0 && !destroyed) {
            destroyed = true;
            Destroy(gameObject, 2f);
            Map.instance.DemolishChunk(ID);
        }
    }

    // TODO: add custom damage
    public void Damage() {
        HP--;
        dustParticles.Play();
    }
}
