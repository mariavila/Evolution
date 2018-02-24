using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunnyMovement : MonoBehaviour {
    public float speed = 6f;
    private Vector3 movement;
    private Rigidbody bunnyRigidBody;
    private Animator anim;
    private int floorMask;
    private int camRayLength;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        bunnyRigidBody = GetComponent<Rigidbody>();
    }

    //quan movem algo amb un rigidbody es crida
    private void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Move(h, v);
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        bunnyRigidBody.MovePosition(transform.position + movement);
        bunnyRigidBody.MoveRotation (Quaternion.LookRotation(movement));
    }


    void Animating(float h, float v)
    {
        bool walking = h != 0f || v!= 0f;
        anim.SetBool("IsWalking", walking);
    }
}
