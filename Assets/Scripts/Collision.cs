using System.Collections.Generic;
using UnityEngine;


public class Collision : MonoBehaviour
{
    public Collider tableCollider; // get table AABB

    // list of balls
    public List<SphereCollider> ballColliders = new List<SphereCollider>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // for every ball, check every other ball and table
        for (int i = 0; i < ballColliders.Count; i++)
        {
            for (int j = 0; j < ballColliders.Count; j++)
            {
                if (i != j)
                {
                    if (hasCollided(ballColliders[i], ballColliders[j]))
                    {
                        // resolve collision
                        handleCollision(ballColliders[i], ballColliders[j]);
                    }
                }
            }
            if (hasCollided(ballColliders[i], tableCollider))
            {
                // handle collision
                handleCollision(ballColliders[i], tableCollider);
            }
        }
        
    }

    private bool hasCollided(Collider A, Collider B)
    {
        return A.bounds.Intersects(B.bounds);
    }

    private void handleCollision(Collider A, Collider B)
    {
        return;
    }
}
