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
            ball.velocity += 
                ball.impulse * 
                new Vector2(Mathf.Cos(ball.orientation), Mathf.Sin(ball.orientation));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ball.impulse += 1;
            Debug.Log("impule is " + ball.impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ball.impulse -= 1;
            Debug.Log("impule is " + ball.impulse);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            ball.orientation += Mathf.PI / 2;
            Debug.Log("orientation is " + ball.orientation);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            ball.orientation -= Mathf.PI / 2;
            Debug.Log("orientation is " + ball.orientation);
        }
    }
}
