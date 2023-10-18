using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
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
            if (!Is_in_table(ballCollider.bounds.center))
            {
                Debug.Log("Ball is out of table");

            }
        }
    }

}