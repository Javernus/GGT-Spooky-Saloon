using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleMovement : MonoBehaviour
{
    // Create an array for initial y values for all child candles
    private float[] initialYs;

    // Create random speed, amplitude and delay for each candle
    private float[] speeds;
    private float[] amplitudes;
    private float[] delays;
    private float[] offsets;


    // Start is called before the first frame update
    void Start()
    {
        // Store the initial Y position of the child candles
        initialYs = new float[transform.childCount];

        // Create random speed, amplitude and delay for each candle
        speeds = new float[transform.childCount];
        amplitudes = new float[transform.childCount];
        delays = new float[transform.childCount];
        offsets = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            initialYs[i] = transform.GetChild(i).position.y;
            speeds[i] = Random.Range(0.5f, 2.5f);
            amplitudes[i] = Random.Range(0.125f, 0.25f);
            delays[i] = Random.Range(0.0f, 5.0f);
            offsets[i] = Random.Range(0.125f, 0.5f);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
        }
    }

    void MoveCandle(Transform candle, float initialY, float speed, float amplitude, float delay, float offset) {
        float time = Mathf.Clamp(Time.time - delay, 0.0f, Mathf.Infinity);
        float extraUp = Mathf.Clamp(Time.time - delay, 0.0f, 1.0f) * offset;

        candle.position = new Vector3(candle.position.x, initialY + (1.0f - Mathf.Cos(time * speed)) * 0.5f * amplitude + extraUp, candle.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        // Move the candles up and down
        for (int i = 0; i < transform.childCount; i++)
        {
            MoveCandle(transform.GetChild(i), initialYs[i], speeds[i], amplitudes[i], delays[i], offsets[i]);
        }
    }
}
