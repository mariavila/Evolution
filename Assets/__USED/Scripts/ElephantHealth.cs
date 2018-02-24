using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantHealth : MonoBehaviour {
    public float health = 10f;
    public float sinkSpeed = 2.5f;

    Rigidbody elephantRigidBody;
    Animator anim;
    ElephantMovement elephantMovement;
    bool isDead;
    bool damaged;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        elephantMovement = GetComponent<ElephantMovement>();
        elephantRigidBody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (damaged)
        {
            // ... move the enemy down by the sinkSpeed per second.
            transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
        }
        damaged = false;
        TakeDamage(Time.deltaTime, false);
    }


    public void TakeDamage(float amount, bool hit)
    {
        // Set the damaged flag so the screen will flash.
        if (hit)
            damaged = true;

        // Reduce the current health by the damage amount.
        health -= amount;

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (health <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }


    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");
        elephantMovement.enabled = false;

        // Turn off the movement and shooting scripts.
        Destroy(gameObject, 2f);
    }
}
