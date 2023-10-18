using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// we can use sphereCollider to check if the ball has collided with another object
public class RigidBall : MonoBehaviour
{
    public float impulse;
    public Vector3 position;
    
    public Quaternion orientation;
    public Vector3 velocity;
    public float velocity_constraint;
    public float drag;

    
    // Start is called before the first frame update
    void Awake()
    {
        position = transform.position;
        orientation = transform.rotation;
        velocity = new Vector3(0f, 0f, 0f);
        impulse = 0f;
        velocity_constraint = 15f;
        drag = 0.2f; // TODO make this a function of speed
    }

    void FixedUpdate()
    {
        if (velocity.magnitude > velocity_constraint)
        {
            velocity = velocity.normalized * velocity_constraint;
        }

        if (velocity.magnitude < -velocity_constraint)
        {
            velocity = velocity.normalized * -velocity_constraint;
        }

        if (velocity.magnitude > 0f)
        {
            velocity -= velocity.normalized * drag;
        }

        if (velocity.magnitude < 0f)
        {
            velocity += velocity.normalized * drag;
        }

        // if velocity is close to 0, set it to 0
        if (velocity.magnitude < drag && velocity.magnitude > -drag)
        {
            velocity = new Vector3(0f, 0f, 0f);
        }

        // update position
        velocity -= velocity.normalized * drag * Time.fixedDeltaTime;
        position += velocity * Time.fixedDeltaTime;
        transform.position = position;
    }

    
}
