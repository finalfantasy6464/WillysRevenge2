﻿using UnityEngine;
using UnityEngine.UI;

public class RadialGauge : MonoBehaviour, IPausable
{
    // Public UI References
    public Image fillImage;

    // Trackers for min/max values
    protected float maxValue = 2f, minValue = 0f;

    // Create a property to handle the slider's value
    private float currentValue = 0f;
    public float CurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            // Ensure the passed value falls within min/max range
            currentValue = Mathf.Clamp(value, minValue, maxValue);

            // Calculate the current fill percentage and display it
            float fillPercentage = currentValue / maxValue;
            fillImage.fillAmount = fillPercentage;
            fillImage.color = Color.Lerp(Color.red, Color.green, fillPercentage);
        }
    }

    public bool isPaused { get; set; }

    public void OnDestroy()
    { }

    public void OnPause()
    { }

    public void OnUnpause()
    { }

    public void PausedUpdate()
    { }
    public void UnPausedUpdate()
    {
        CurrentValue += 0.0086f;
    }

    void Start()
    {
        CurrentValue = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            UnPausedUpdate();
        }
    }
}
