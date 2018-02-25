using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elephant : MonoBehaviour {

    public float speed = 6f;
    private Vector3 movement;
    private Rigidbody elephantRigidBody;
    private Animator anim;
    private int floorMask;
    private int camRayLength;

    private NeuralNetwork net;
    private Transform[] food;
    float prevout1 = 0f;
    float prevout2 = 0f;
    private bool canmove = true;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        elephantRigidBody = GetComponent<Rigidbody>();
    }

    //quan movem algo amb un rigidbody es crida
    private void FixedUpdate()
    {
        if (canmove)
        {
            float[] inputs = new float[6]; //foodx,foodz,mex,mez
            float mindist = 1000f;
            float mindist2 = 1000f;
            int minpos = 0;
            int minpos2 = 1;
            for (int i = 0; i < food.Length; i++)
            {
                float aux = Vector2.Distance(transform.position, food[i].position);
                if (aux < mindist)
                {
                    mindist2 = mindist;
                    minpos2 = minpos;
                    mindist = aux;
                    minpos = i;
                }
                else if (aux < mindist2)
                {
                    mindist2 = aux;
                    minpos2 = i;
                }
            }

            inputs[0] = (food[minpos].position[0] - transform.position[0]) / 60f;
            inputs[1] = (food[minpos].position[2] - transform.position[2]) / 60f;
            inputs[2] = (food[minpos2].position[0] - transform.position[0]) / 60f;
            inputs[3] = (food[minpos2].position[2] - transform.position[2]) / 60f;
            inputs[4] = prevout1;
            inputs[5] = prevout2;

            float[] output = net.FeedForward(inputs);
            float h = output[0];
            float v = output[1];
            //float h = Input.GetAxisRaw("Horizontal");
            //float v = Input.GetAxisRaw("Vertical");
            //if (Mathf.Abs(h) < 0.05f) h = 0;
            //if (Mathf.Abs(v) < 0.05f) v = 0;

            Move(h, v);
            Animating(h, v);
            //net.AddFitness( -(mindist / 100f));
        }
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        elephantRigidBody.MovePosition(transform.position + movement);
        if(movement != new Vector3(0f,0f,0f))
            elephantRigidBody.MoveRotation(Quaternion.LookRotation(movement));
    }


    void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }

    public void Init(NeuralNetwork net, Transform[] food)
    {
        this.food = food;
        this.net = net;
    }

    void Timer()
    {
        canmove = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bunny"))
        {
            net.AddFitness(100f);
            canmove = false;
            Invoke("Timer", 1.8f);

        }
    }

}
