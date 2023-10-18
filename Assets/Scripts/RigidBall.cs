using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// we can use sphereCollider to check if the ball has collided with another object
public class RigidBall : MonoBehaviour
{
    private float impulse;
    private Vector2 position;
    private float orientation; // angle
    private Vector2 velocity;
    private float velocity_constraint = 3;
    private float drag = 0;

    
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        velocity = new Vector2(0, 0);
        orientation = 0;
        impulse = 0;
    }

    void FixedUpdate()
    {
        if (velocity.magnitude > velocity_constraint)
        {
            velocity = velocity.normalized * velocity_constraint;
        }

        velocity -= drag * velocity * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
    }

    
}
