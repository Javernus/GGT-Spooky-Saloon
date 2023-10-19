using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// we can use sphereCollider to check if the ball has collided with another object
public class RigidBall : MonoBehaviour {
    public bool canMove = true;
    public Vector2 velocity;
    public Vector3 position;

    public void addImpulse(float impulse) {
        velocity += impulse * orientation;
        setOrientationWithVector(velocity);
        canMove = false;
    }

    public void addCollision(Vector2 impulse) {
        velocity += impulse;
        setOrientationWithVector(velocity);
        canMove = false;
    }

    public void setCollision(Vector2 impulse) {
        velocity = impulse;
        setOrientationWithVector(velocity);
        canMove = false;
    }

    public void resetImpulse() {
        velocity = new Vector2(0f, 0f);
        orientation = new Vector2(1f, 0f);
        canMove = true;
    }

    public void invertVelocityX() {
        velocity = new Vector2(velocity.x * -1f, velocity.y);
        setOrientationWithVector(velocity);
    }

    public void invertVelocityY() {
        velocity = new Vector2(velocity.x, velocity.y * -1f);
        setOrientationWithVector(velocity);
    }

    public void move(Vector2 p) {
        position += new Vector3(p.x, 0f, p.y);
        transform.position = originalPosition + position;
    }

    public void setPosition(Vector3 p) {
        position = p;
        transform.position = originalPosition + position;
    }

    public void setOrientationWithVector(Vector2 o) {
        transform.rotation = Quaternion.Euler(0f, -Mathf.Atan2(o.y, o.x) * Mathf.Rad2Deg, 0f);
    }

    public void setOrientationWithRadians(float rotationInRadians) {
        orientation = new Vector2(Mathf.Cos(rotationInRadians), -Mathf.Sin(rotationInRadians));
        transform.rotation = Quaternion.Euler(0f, rotationInRadians * Mathf.Rad2Deg, 0f);
    }

    private Vector3 originalPosition;
    private Vector2 orientation = new Vector2(1f, 0f);
    private float velocity_constraint = 2f;
    private float drag = 0.25f;

    // Start is called before the first frame update
    void Start() {
        Vector3 tableZeroPos = transform.parent.parent.GetChild(1).position;
        originalPosition = tableZeroPos;
        position = transform.position - tableZeroPos;
        velocity = new Vector2(0f, 0f);
    }

    void FixedUpdate() {
        if (velocity.magnitude > velocity_constraint) {
            velocity = velocity.normalized * velocity_constraint;
        } else if (!canMove && velocity.magnitude < 0.02f) {
            velocity = new Vector2(0f, 0f);
            canMove = true;
        }

        velocity -= drag * velocity * Time.fixedDeltaTime;
        Vector2 delta = velocity * Time.fixedDeltaTime;
        move(delta);
    }
}
