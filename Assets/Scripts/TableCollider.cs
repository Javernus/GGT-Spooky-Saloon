using UnityEngine;
public class TableCollider : MonoBehaviour
{
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Collision detected");
    }

}