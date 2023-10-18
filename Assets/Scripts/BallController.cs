using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControler : MonoBehaviour
{
    private Transform camera;
    private RigidBall ball;
    private GameObject cue;

    // timer for cue
    private float timer = 0f;
    private float impulse = 0.25f;
    private float alt = 0.25f;
    private float orientation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        ball = transform.GetComponent<RigidBall>();

        if (ball == null) {
            Debug.Log("Ball not found");
        } else {
            Debug.Log("Ball found");
        }

        camera = GameObject.FindWithTag("MainCamera").transform;

        cue = GameObject.FindWithTag("Cue");
        if (cue == null)
        {
            Debug.Log("BallControler: cue is null");
        }
    }

    void ChangeOrientation() {
        ball.setOrientationWithRadians(orientation);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!ball.canMove) {
                Debug.Log("Ball cannot move");
                return;
            } else {
                Debug.Log("Ball can move");
                ball.canMove = false;
            }

            ball.addImpulse(impulse);
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            impulse = Mathf.Clamp(impulse + alt, 0f, 2f);
            Debug.Log("impulse is " + impulse);
        }

        if (Input.GetKeyDown(KeyCode.S)) {
            impulse = Mathf.Clamp(impulse - alt, 0f, 2f);
            Debug.Log("impulse is " + impulse);
        }

        if (Input.GetKeyDown(KeyCode.A)) {
            orientation -= Mathf.PI / 32f * alt;
            ChangeOrientation();
            Debug.Log("orientation is " + orientation);
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            orientation += Mathf.PI / 32f * alt;
            ChangeOrientation();
            Debug.Log("orientation is " + orientation);
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            alt = 0.025f;
            Debug.Log("alt is on");
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt)) {
            alt = 0.25f;
            Debug.Log("alt is off");
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            // Set the camera position to Vector3(-0.0430000015,1.08399987,2.6730001) and rotation to Vector3(15.8655329,180,0)
            camera.position = new Vector3(-0.0430000015f, 1.08399987f, 2.6730001f);
            camera.rotation = Quaternion.Euler(15.8655329f, 180f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.U)) {
            // Set the camera position to Vector3(0.00400000019,1.76800001,-0.0250000004) and rotation to Vector3(90,180,0)
            camera.position = new Vector3(0.00400000019f, 1.76800001f, -0.0250000004f);
            camera.rotation = Quaternion.Euler(90f, 180f, 0f);
        }

        // hide cue when ball is moving
        if (ball.velocity.magnitude > 0.1f) {
            cue.SetActive(false);
        } else {
            if (timer > 2f) {
                cue.SetActive(true);
                timer = 0f;
            }
        }

        timer += Time.deltaTime;
    }
}
