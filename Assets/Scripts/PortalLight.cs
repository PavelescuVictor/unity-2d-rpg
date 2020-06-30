using System;
using System.Collections;
using System.Collections.Generic;
//using System.Data.SqlClient;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
using Random = System.Random;

public class PortalLight : MonoBehaviour
{

    public enum WaveForm{
        sin, tri, sqr, saw, inv, noise
    };

    public WaveForm waveForm = WaveForm.sin;

    public float baseStart; // start
    public float amplitude; // amplitude of the wave
    public float phase; // start point inside an wave cycle
    public float frequency; //cycle frequency per second

    // Keep a copy of the original color
    private float originalColor;
    private Light2D light;
    
    void Start()
    {
        light = GetComponent<Light2D>();
        // Store the original color
        originalColor = light.intensity;
    }
    
    void Update()
    {
        float result = EvalWave();
        if (result >= 0.0f)
        {
            light.intensity = originalColor * (EvalWave());
        }
    }

    float EvalWave()
    {
        float x = (Time.time + phase) + frequency;
        float y;
        x = x - Mathf.Floor(x);
        
        if (waveForm == WaveForm.sin)
        {
            y = Mathf.Sin(x * 2 * Mathf.PI);
        }
        else if (waveForm == WaveForm.tri)
        {
            if (x < 0.5f)
            {
                y = 4.0f * x - 1.0f;
            }
            else
            {
                y = -4.0f * x + 3.0f;
            }
        }
        else if (waveForm == WaveForm.sqr)
        {
            if (x < 0.5f)
            {
                y = 1.0f;
            }
            else
            {
                y = -1.0f;
            }
        }
        else if (waveForm == WaveForm.saw)
        {
            y = x;
        }
        else if (waveForm == WaveForm.inv)
        {
            y = 1.0f - x;
        }
        else if (waveForm == WaveForm.noise)
        {
            Random r = new Random();
            int genRand = r.Next();
            y = 1f - (genRand * 2);
        }
        else
        {
            y = 1.0f;
        }

        return (y * amplitude) + baseStart;
    }
}
