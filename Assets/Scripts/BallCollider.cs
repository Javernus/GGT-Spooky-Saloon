using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class BallCollider : MonoBehaviour
{

    // list of balls
    public SphereCollider[] ballColliders;

    // Start is called before the first frame update
    void Start()
    {
        // get ball colliders
        ballColliders = GetComponentsInChildren<SphereCollider>();

        if (ballColliders == null)
        {
            Debug.Log("BallCollider: ballColliders is null");
        }
        
    }

    bool HasCollided(SphereCollider A, SphereCollider B)
    {
        if (A == B)
        {
            return false;
        }

        // get distance between centers of spheres
        float distance = Vector3.Distance(A.transform.position, B.transform.position);

        // get sum of radii
        float sum_radii = A.radius + B.radius;

        // if distance is lte sum of radii, then collision has occurred
        return distance <= sum_radii;
    }

    Vector3 GetCollisionNormal(SphereCollider A, SphereCollider B)
    {
        // get vector from center of A to center of B
        Vector3 center_A_to_center_B = B.transform.position - A.transform.position;

        // normalize vector
        Vector3 normal = center_A_to_center_B.normalized;

        return normal;
    }

    void TransferMomentum(SphereCollider A, SphereCollider B)
    {
        RigidBall ball_A = A.GetComponent<RigidBall>();
        RigidBall ball_B = B.GetComponent<RigidBall>();

        // get normal of collision
        Vector3 normal = GetCollisionNormal(A, B);

        // get velocity of A in direction of normal
        float velocity_A = Vector3.Dot(ball_A.velocity, normal);

        // get velocity of B in direction of normal
        float velocity_B = Vector3.Dot(ball_B.velocity, normal);

        ball_A.velocity = ball_A.velocity - velocity_A * normal + velocity_B * normal;
        ball_B.velocity = ball_B.velocity - velocity_B * normal + velocity_A * normal;
    }


    void FixedUpdate()
    {
        for (int i = 0; i < ballColliders.Length; i++)
        {
            for (int j = 0; j < ballColliders.Length; j++)
            {
                if (HasCollided(ballColliders[i], ballColliders[j]))
                {
                    Debug.Log("Collision detected");
                    Debug.Log("Ball " + i + " collided with ball " + j);
                    Debug.Log("Normal: " + GetCollisionNormal(ballColliders[i], ballColliders[j]));

                    // transfer momentum
                    TransferMomentum(ballColliders[i], ballColliders[j]);
                }
            }
        }
        
    }

}
