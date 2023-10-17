using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// we can use sphereCollider to check if the ball has collided with another object
public class RigidBall : MonoBehaviour
{
    // rigid body physics
    private Vector3 position;
    private Vector3 velocity;
    private float velocity_constraint = 10;
    private float drag = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        velocity = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        velocity -= drag * velocity * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }
}
