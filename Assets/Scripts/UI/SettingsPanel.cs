﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    #region Sliders
    [Header("Sliders")]

    [SerializeField]
    private Slider separationSlider;

    [SerializeField]
    private Slider alignmentSlider;

    [SerializeField]
    private Slider cohesionSlider;
    #endregion

    #region Buttons
    [Header("Buttons")]

    /// <summary>
    /// Hides settings panel when clicked
    /// </summary>
    [SerializeField]
    private Button minimizeButton;

    /// <summary>
    /// Brings up settings panel when clicked
    /// </summary>
    [SerializeField]
    private Button maximizeButton;
    
    /// <summary>
    /// Resets flocking parameters to default values
    /// </summary>
    [SerializeField]
    private Button resetButton;
    #endregion

    private float defaultSeparationWeight;
    private float defaultAlignmentWeight;
    private float defaultCohesionWeight;

    void Start()
    {
        InitializeSliders();
        InitializeButtons();
    }

    private void InitializeSliders()
    {
        BoidDetection boidDetection = BoidDetection.Instance;

        // Initialize sliders with default values
        separationSlider.value = boidDetection.separation;
        alignmentSlider.value = boidDetection.alignment;
        cohesionSlider.value = boidDetection.cohesion;

        // Store default values so we can restore them later
        defaultSeparationWeight = boidDetection.separation;
        defaultAlignmentWeight = boidDetection.alignment;
        defaultCohesionWeight = boidDetection.cohesion;

        // Set up slider events to update boid settings with changed values
        separationSlider.onValueChanged.AddListener((value) => boidDetection.separation = value);
        alignmentSlider.onValueChanged.AddListener((value) => boidDetection.alignment = value);
        cohesionSlider.onValueChanged.AddListener((value) => boidDetection.cohesion = value);
    }

    private void InitializeButtons()
    {
        // Set up events for minimize/maximize buttons
        minimizeButton.onClick.AddListener(() => Minimize());
        maximizeButton.onClick.AddListener(() => Maximize());

        // Set up reset button to restore default flocking values
        resetButton.onClick.AddListener(() => ResetValues());
    }

    /// <summary>
    /// Hides settings panel
    /// </summary>
    private void Minimize()
    {
        gameObject.SetActive(false);
        maximizeButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows settings panel
    /// </summary>
    private void Maximize()
    {
        gameObject.SetActive(true);
        maximizeButton.gameObject.SetActive(false);
    }

    public void ResetValues()
    {
        BoidDetection boidDetection = BoidDetection.Instance;

        // Reset sliders with default values
        separationSlider.value = defaultSeparationWeight;
        alignmentSlider.value = defaultAlignmentWeight;
        cohesionSlider.value = defaultCohesionWeight;

        // Restore default values for each setting
        boidDetection.separation = defaultSeparationWeight;
        boidDetection.alignment = defaultAlignmentWeight;
        boidDetection.cohesion = defaultCohesionWeight;
    }
}
