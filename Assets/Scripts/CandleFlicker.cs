using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleMovement : MonoBehaviour
{
    public float speed = 1.0f;
    public float amplitude = 0.5f;

    // Define private colour
    private Color colour;

    // Start is called before the first frame update
    void Start()
    {
        colour = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the candle up and down
        // transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time * speed) * amplitude, transform.position.z);

        // Change the colour of the candle
        colour.r = Mathf.Sin(Time.time * speed) * amplitude;
        colour.g = Mathf.Sin(Time.time * speed) * amplitude;
        colour.b = Mathf.Sin(Time.time * speed) * amplitude;
        GetComponent<Renderer>().material.color = colour;
    }
}
