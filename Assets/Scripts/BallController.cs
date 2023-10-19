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
    private bool isShooting = false;
    private float impulse = 0.25f;
    private float alt = 0.25f;
    private float orientation = 0f;

    private Vector3 originalCuePosition;
    float cueAngleConstant = 0.1051f;

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

        if (cue == null) {
            Debug.Log("BallControler: cue is null");
        }

        originalCuePosition = cue.transform.position;
        SetCuePosition(impulse);
    }

    void GetCurrentAngleFromBall() {
        orientation = ball.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
    }

    void ChangeOrientation() {
        ball.setOrientationWithRadians(orientation);
        SetCuePosition(impulse);
    }

    void SetCuePosition(float i) {
        float distanceFromBall = -(0.3f + i) * 0.1f;

        Vector3 angleVector = new Vector3(
            Mathf.Cos(orientation),
            0f,
            -Mathf.Sin(orientation)
        );

        Vector3 cuePosition = ball.transform.position + angleVector * distanceFromBall + new Vector3(0f, -distanceFromBall * cueAngleConstant, 0f);

        cue.transform.position = cuePosition;

        cue.transform.rotation = Quaternion.Euler(
            0f,
            orientation * Mathf.Rad2Deg,
            -6f
        );
    }

    void Shoot() {
        isShooting = true;
        timer = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!ball.canMove) return;

            ball.canMove = false;
            GetCurrentAngleFromBall();
            Shoot();
        }

        if (Input.GetKey(KeyCode.W)) {
            impulse = Mathf.Clamp(impulse + alt, 0f, 2f);
            SetCuePosition(impulse);
        }

        if (Input.GetKey(KeyCode.S)) {
            impulse = Mathf.Clamp(impulse - alt, 0f, 2f);
            SetCuePosition(impulse);
        }

        if (Input.GetKey(KeyCode.A)) {
            GetCurrentAngleFromBall();
            orientation -= Mathf.PI / 8f * alt;
            ChangeOrientation();
        }

        if (Input.GetKey(KeyCode.D)) {
            GetCurrentAngleFromBall();
            orientation += Mathf.PI / 8f * alt;
            ChangeOrientation();
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            alt = 0.025f;
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt)) {
            alt = 0.25f;
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

        if (ball.canMove) {
            GetCurrentAngleFromBall();
            SetCuePosition(impulse);
            cue.SetActive(true);
        } else {
            cue.SetActive(false);
        }

        if (isShooting) {
            cue.SetActive(true);
            if (timer > 0.5f) {
                isShooting = false;
                cue.SetActive(false);
                ball.addImpulse(impulse);
            } else {
                timer += Time.deltaTime;

                // Move the cue to the ball
                SetCuePosition(impulse * (0.5f - timer) * 2f);
            }
        }
    }
}
