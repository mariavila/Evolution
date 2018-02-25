using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour {

    private Rigidbody foodRigidBody;
    // Use this for initialization
    void Awake ()
    {
        foodRigidBody = GetComponent<Rigidbody>();
    }
	/**
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bunny"))
        {
            float h = UnityEngine.Random.Range(-20f, 20f);
            float v = UnityEngine.Random.Range(-20f, 20f);
            foodRigidBody.MovePosition(new Vector3(h, 0f, v));
        }
    }
    */
}
