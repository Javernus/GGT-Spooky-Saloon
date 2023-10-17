using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    private Color[] beforeColours;
    private Color[] afterColours;

    private float[] differences;
    private float[] durations;

    Color GenerateNewColour() {
        float hue = Random.Range(0.02f, 0.11f);
        float saturation = Random.Range(0.5f, 1.0f);
        float value = Random.Range(0.875f, 1.0f);

        return Color.HSVToRGB(hue, saturation, value);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Fill beforeColours with current colours of child candles
        beforeColours = new Color[transform.childCount];

        // Create a random candlish colour for each candle as the afterColour
        afterColours = new Color[transform.childCount];

        // Create a random duration for each candle and set difference to 0
        differences = new float[transform.childCount];
        durations = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            beforeColours[i] = transform.GetChild(i).GetChild(2).GetComponent<Light>().color;

            afterColours[i] = GenerateNewColour();

            differences[i] = 0.0f;
            durations[i] = Random.Range(0.0125f, 0.5f);

        }
    }

    void UpdateCandle(Transform candle, Color beforeColour, Color afterColour, float difference, float duration, int i) {
        float mixRatio = Mathf.Clamp(Time.time - difference, 0.0f, Mathf.Infinity) / duration;

        // Change the colour of the candle's first material and the light's colour
        candle.GetChild(1).GetComponent<MeshRenderer>().materials[0].SetVector("_EmissionColor", Color.Lerp(beforeColour, afterColour, mixRatio));
        candle.GetChild(2).GetComponent<Light>().color = Color.Lerp(beforeColour, afterColour, mixRatio);

        if (mixRatio >= 1.0f) {
            Debug.Log("New colour " + i + ", " + Time.time + ", " + difference + ", " + duration + ", " + mixRatio);
            differences[i] = Time.time;
            durations[i] = Random.Range(0.0125f, 0.5f);

            beforeColours[i] = afterColour;
            afterColours[i] = GenerateNewColour();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the colours of the candles
        for (int i = 0; i < transform.childCount; i++)
        {
            UpdateCandle(transform.GetChild(i), beforeColours[i], afterColours[i], differences[i], durations[i], i);
        }
    }
}
