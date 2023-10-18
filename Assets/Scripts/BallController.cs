using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControler : MonoBehaviour
{

    private RigidBall ball;
    private GameObject cue; 

    // timer for cue
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ball = GetComponent<RigidBall>();
        if (ball == null)
        {
            Debug.Log("BallControler: ball is null");
        }

        cue = GameObject.Find("Cue");
        if (cue == null)
        {
            Debug.Log("BallControler: cue is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ball.velocity = ball.orientation * Vector3.forward * ball.impulse;
            ball.impulse = 0f;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ball.impulse += 5f;
            Debug.Log("impule is " + ball.impulse);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ball.impulse -= 5f;
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

        // hide cue when ball is moving
        if (ball.velocity.magnitude > 0.1f)
        {
            cue.SetActive(false);
        }
        else
        {
            if (timer > 2f)
            {
                cue.SetActive(true);
                timer = 0f;
            }
        }

        timer += Time.deltaTime;
    }
}
