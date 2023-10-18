using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private BoxCollider tableCollider; // get table AABB
    private List<RigidBall> balls = new List<RigidBall>();
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        tableCollider = transform.GetChild(1).GetComponent<BoxCollider>();

        Transform ballsContainer = transform.GetChild(0);

        for (int i = 0; i < ballsContainer.childCount; i++) {
            balls.Add(ballsContainer.GetChild(i).GetComponent<RigidBall>());
        }

        radius = balls[0].transform.localScale.x / 2f;

        Debug.Log(tableCollider.bounds.min.x + ", " + tableCollider.bounds.min.z + ", " + tableCollider.bounds.max.x + ", " + tableCollider.bounds.max.z);
    }

    float CalculatePercentage(float value, float distance) {
        return -Mathf.Abs((value - distance) / value);
    }

    void PlayTableCollisionSound() {
        // TODO play sound
    }

    void HandleTableCollision(RigidBall ball) {
        Vector3 position = ball.transform.position;
        Vector2 velocity = ball.velocity;
        float deltaTime = Time.deltaTime;

        Vector2 normalizedVelocity = velocity.normalized * radius;

        float distanceFromMinX = (position.x - radius) - tableCollider.bounds.min.x;
        float distanceFromMaxX = tableCollider.bounds.max.x - (position.x + radius);
        float distanceFromMinZ = (position.z - radius) - tableCollider.bounds.min.z;
        float distanceFromMaxZ = tableCollider.bounds.max.z - (position.z + radius);

        if (distanceFromMinX < 0f) {
            ball.invertVelocityX();
            float percentage = CalculatePercentage(radius, distanceFromMinX);
            ball.move(normalizedVelocity * percentage * radius);
            PlayTableCollisionSound();
        } else if (distanceFromMaxX < 0f) {
            ball.invertVelocityX();
            float percentage = CalculatePercentage(radius, distanceFromMaxX);
            ball.move(normalizedVelocity * percentage * radius);
            PlayTableCollisionSound();
        }

        if (distanceFromMinZ < 0f) {
            ball.invertVelocityY();
            float percentage = CalculatePercentage(radius, distanceFromMinZ);
            ball.move(normalizedVelocity * percentage * radius);
            PlayTableCollisionSound();
        } else if (distanceFromMaxZ < 0f) {
            ball.invertVelocityY();
            float percentage = CalculatePercentage(radius, distanceFromMaxZ);
            ball.move(normalizedVelocity * percentage * radius);
            PlayTableCollisionSound();
        }
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
                float percentage = CalculatePercentage(radius * 2f, distance);
                ball.move(collisionNormal * percentage);
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
    }
}
