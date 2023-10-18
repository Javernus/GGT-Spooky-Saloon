using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class TableCollider : MonoBehaviour
{
    // table box collider
    public BoxCollider tableCollider;

    // list of balls
    public SphereCollider[] ballColliders;

    // Start is called before the first frame update
    void Start()
    {
        // get table game object
        GameObject poolTabel = GameObject.Find("PoolTafel");

        // get table collider
        tableCollider = poolTabel.GetComponent<BoxCollider>();

        if (tableCollider == null)
        {
            Debug.Log("TableCollider: tableCollider is null");
        }

        // get ball colliders
        ballColliders = GetComponentsInChildren<SphereCollider>();

        if (ballColliders == null)
        {
            Debug.Log("TableCollider: ballColliders is null");
        }
    }

    bool Is_in_table(Vector3 position)
    {
        return (
            position.x > tableCollider.bounds.min.x &&
            position.x < tableCollider.bounds.max.x &&
            position.z > tableCollider.bounds.min.z &&
            position.z < tableCollider.bounds.max.z
        );
    }

    void FixedUpdate()
    {
        // for every ball, check if it is in table
        foreach (SphereCollider ballCollider in ballColliders)
        {
            RigidBall ball = ballCollider.GetComponent<RigidBall>();

            // calculate position of ball in next frame
            Vector3 position = ball.position + ball.velocity * Time.fixedDeltaTime * 
                new Vector3(
                    Mathf.Cos(ball.orientation.eulerAngles.y * Mathf.Deg2Rad),
                    0f,
                    Mathf.Sin(ball.orientation.eulerAngles.y * Mathf.Deg2Rad)
                );
                
            if (!Is_in_table(position))
            {
                // clamp position
                ball.position = new Vector3(
                    Mathf.Clamp(ball.position.x, tableCollider.bounds.min.x, tableCollider.bounds.max.x),
                    ball.position.y,
                    Mathf.Clamp(ball.position.z, tableCollider.bounds.min.z, tableCollider.bounds.max.z)
                );
                
                // revert velocity
                ball.velocity *= -1f;

            }
        }
    }

}