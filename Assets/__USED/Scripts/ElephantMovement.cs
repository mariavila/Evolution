using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElephantMovement : MonoBehaviour {
    public float speed = 3f;
    private Vector3 movement;
    private Rigidbody elephantRigidBody;
    private Animator anim;
    private int floorMask;
    private int camRayLength;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        elephantRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float h = UnityEngine.Random.Range(0f, 1f);
        float v = UnityEngine.Random.Range(0f, 1f);
        Move(h, v);
        Animating(h, v);
	}

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        elephantRigidBody.MovePosition(transform.position + movement);
        elephantRigidBody.MoveRotation(Quaternion.LookRotation(movement));
    }

    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}
