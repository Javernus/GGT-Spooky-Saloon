using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// we can use sphereCollider to check if the ball has collided with another object
public class RigidBall : MonoBehaviour
{
    public float impulse;
    public Vector2 position;
    
    public Quaternion orientation;
    public float velocity;
    public float velocity_constraint;
    public float drag;

    
    // Start is called before the first frame update
    void Awake()
    {
        position = transform.position;
        orientation = transform.rotation;
        velocity = 0f;
        impulse = 0f;
        velocity_constraint = 2f;
        drag = 0.2f;
    }

    void FixedUpdate()
    {
        if (velocity > velocity_constraint)
        {
            velocity = velocity_constraint;
        }

        if (velocity < -velocity_constraint)
        {
            velocity = -velocity_constraint;
        }

        if (velocity > 0f)
        {
            velocity -= drag;
        }

        if (velocity < 0f)
        {
            velocity += drag;
        }

        // if velocity is close to 0, set it to 0
        if (velocity < 0.1f && velocity > -0.1f)
        {
            velocity = 0f;
        }

        position += velocity * 
                new Vector2(
                    Mathf.Cos(orientation.eulerAngles.y * Mathf.Deg2Rad),
                    Mathf.Sin(orientation.eulerAngles.y * Mathf.Deg2Rad)
                 );
        transform.position = position;
    }

    
}
