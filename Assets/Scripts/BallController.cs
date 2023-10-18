using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControler : MonoBehaviour
{

    private RigidBall ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<RigidBall>();
        if (ball == null)
        {
            Debug.Log("BallControler: ball is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ball.velocity += ball.impulse;
            ball.impulse = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ball.impulse += 1f;
            Debug.Log("impule is " + ball.impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ball.impulse -= 1f;
            Debug.Log("impule is " + ball.impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ball.orientation = Quaternion.Euler(0, ball.orientation.eulerAngles.y + 45, 0);
            ball.transform.Rotate(0, -45, 0);
            Debug.Log("orientation is " + ball.orientation);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ball.orientation = Quaternion.Euler(0, ball.orientation.eulerAngles.y - 45, 0);
            ball.transform.Rotate(0, 45, 0);
            Debug.Log("orientation is " + ball.orientation);
        }
    }
}
