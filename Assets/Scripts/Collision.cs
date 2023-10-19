using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private BoxCollider tableCollider; // get table AABB
    private List<RigidBall> balls = new List<RigidBall>();
    private float radius;
    private Vector3[] originalPositions;

    // Start is called before the first frame update
    void Start()
    {
        tableCollider = transform.GetChild(1).GetComponent<BoxCollider>();

        Transform ballsContainer = transform.GetChild(0);
        originalPositions = new Vector3[ballsContainer.childCount];

        for (int i = 0; i < ballsContainer.childCount; i++) {
            balls.Add(ballsContainer.GetChild(i).GetComponent<RigidBall>());
            originalPositions[i] = balls[i].position;
        }

        radius = balls[0].transform.localScale.x / 2f;

        Debug.Log(tableCollider.bounds.min.x + ", " + tableCollider.bounds.min.z + ", " + tableCollider.bounds.max.x + ", " + tableCollider.bounds.max.z);
    }

    float CalculatePercentage(float value, float distance) {
        return -Mathf.Abs(distance / value);
    }

    void PlayTableCollisionSound() {
        // TODO play sound
    }

    void PlayBallCollisionSound() {
        // TODO play sound
    }

    void HandleTableCollision(RigidBall ball) {
        Vector3 position = ball.position;
        Vector2 velocity = ball.velocity;
        float deltaTime = Time.deltaTime;

        Vector2 normalizedVelocity = velocity.normalized * radius;

        float distanceFromMinX = (position.x - radius) - tableCollider.bounds.min.x;
        float distanceFromMaxX = tableCollider.bounds.max.x - (position.x + radius);
        float distanceFromMinZ = (position.z - radius) - tableCollider.bounds.min.z;
        float distanceFromMaxZ = tableCollider.bounds.max.z - (position.z + radius);

        if (distanceFromMinX < 0f) {
            float percentage = CalculatePercentage(velocity.x, distanceFromMinX);
            ball.move(velocity * percentage);
            ball.invertVelocityX();
            PlayTableCollisionSound();
        } else if (distanceFromMaxX < 0f) {
            float percentage = CalculatePercentage(velocity.x, distanceFromMaxX);
            ball.move(velocity * percentage);
            ball.invertVelocityX();
            PlayTableCollisionSound();
        }

        if (distanceFromMinZ < 0f) {
            float percentage = CalculatePercentage(velocity.y, distanceFromMinZ);
            ball.move(velocity * percentage);
            ball.invertVelocityY();
            PlayTableCollisionSound();
        } else if (distanceFromMaxZ < 0f) {
            float percentage = CalculatePercentage(velocity.y, distanceFromMaxZ);
            ball.move(velocity * percentage);
            ball.invertVelocityY();
            PlayTableCollisionSound();
        }
    }

    float CalculateOvershoot(Vector2 velocity, Vector3 ballPosition, Vector3 otherBallPosition, float distance) {
        Vector3 velocity3D = new Vector3(velocity.x, 0f, velocity.y).normalized;
        Vector3 positionDifference = otherBallPosition - ballPosition;
        Vector3 projected = Vector3.Project(positionDifference, velocity3D);
        float distanceFromVelocity = Vector3.Distance(positionDifference, projected);

        float vValue = projected.magnitude;
        float distanceBack = Mathf.Sqrt(Mathf.Pow(2f * radius, 2f) - Mathf.Pow(distanceFromVelocity, 2f)) - vValue;

        Debug.Log("distanceFromVelocity: " + distanceFromVelocity);
        Debug.Log("vValue: " + vValue);
        Debug.Log("distanceBack: " + distanceBack);
        Debug.Log("velocity: " + velocity);
        Debug.Log("velocity.magnitude: " + velocity.magnitude);
        Debug.Log("distance: " + distance);
        Debug.Log("distance / velocity.magnitude: " + distance / velocity.magnitude);

        return -distanceBack / velocity.magnitude;
    }

    void HandleBallCollision(RigidBall ball) {
        for (int i = 0; i < balls.Count; i++) {
            RigidBall otherBall = balls[i];

            if (ball == otherBall) {
                continue;
            }

            Vector3 position = ball.position;
            Vector2 velocity = ball.velocity;
            float deltaTime = Time.deltaTime;

            Vector3 otherPosition = otherBall.position;
            Vector2 otherVelocity = otherBall.velocity;

            Vector3 positionDifference = otherPosition - position;

            Vector3 collisionNormal = (new Vector2(positionDifference.x, positionDifference.z)).normalized;
            float distance = Vector3.Distance(position, otherPosition);

            if (distance < radius * 2f) {
                Vector3 direction = (position - otherPosition).normalized;
                Vector2 velocityDifference = velocity - otherVelocity;
                float dotProduct = Vector2.Dot(velocityDifference, direction);

                Vector2 impulse = -collisionNormal * Vector2.Dot(velocityDifference, collisionNormal);
                ball.addCollision(impulse);

                impulse = -collisionNormal * Vector2.Dot(-velocityDifference, collisionNormal);
                Debug.Log(impulse);
                otherBall.addCollision(impulse);

                // TODO: calculate movement back properly (see notes)
                float percentage = CalculateOvershoot(velocity, position, otherPosition, distance);
                ball.move(velocity * percentage);
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Loop over all balls
        for (int i = 0; i < balls.Count; i++) {
            RigidBall ball = balls[i];

            if (ball.canMove) {
                continue;
            }

            HandleTableCollision(ball);
            HandleBallCollision(ball);
        }

        // if (Input.GetKeyDown(KeyCode.R)) {
        //     for (int i = 0; i < balls.Count; i++) {
        //         balls[i].setPosition(originalPositions[i]);
        //         balls[i].resetImpulse();
        //     }
        // }
    }
}
