using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BunnyHealth : MonoBehaviour
{
    public float health = 100f;
    public float sinkSpeed = 2.5f;

    Rigidbody bunnyRigidBody;
    Animator anim;
    BunnyMovement bunnyMovement;
    bool isDead;
    bool damaged;

    void Awake()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        bunnyMovement = GetComponent<BunnyMovement>();
        bunnyRigidBody = GetComponent<Rigidbody>();
    }


    /**void Update()
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

        // Turn off the movement and shooting scripts.
        Destroy(gameObject, 2f);
        bunnyMovement.enabled = false;
    }
    */
    private void Timer()
    {
        anim.SetBool("IsAlive", true);
        bunnyMovement.enabled = true;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Food"))
        {
            other.gameObject.transform.position = new Vector3(UnityEngine.Random.Range(-40f, 40f), 0.5f, UnityEngine.Random.Range(-40f, 40f));
            bunnyMovement.net.AddFitness(100f);
        }
        else if (other.gameObject.CompareTag("Elephant"))
        {
            bunnyMovement.net.AddFitness(-150f);
            anim.SetBool("IsAlive", false);
            bunnyMovement.enabled = false;
            anim.SetTrigger("Die");
            Invoke("Timer", 1.0f);
        }
    }
}

