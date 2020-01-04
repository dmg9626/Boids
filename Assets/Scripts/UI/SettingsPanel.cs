using System;
using System.Collections;
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

    #region ModalButtons
    [Header("Move Speed")]

    [SerializeField]
    private Button increaseMoveSpeed;

    [SerializeField]
    private Button decreaseMoveSpeed;

    /// <summary>
    /// Shows current value for boid movement speed
    /// </summary>
    [SerializeField]
    private Text movementSpeed;

    [Header("Rotation Speed")]
    [SerializeField]
    private Button increaseRotationSpeed;

    [SerializeField]
    private Button decreaseRotationSpeed;
    
    /// <summary>
    /// Shows current value for boid rotation speed
    /// </summary>
    [SerializeField]
    private Text rotationSpeed;

    #endregion
    
    #region MiscButtons
    [Header("Misc Buttons")]

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
        UpdateText();
    }

    private void UpdateText()
    {
        rotationSpeed.text = BoidManager.Instance.rotationSpeed.ToString();
        movementSpeed.text = BoidManager.Instance.moveSpeed.ToString();
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

        // Set up movement/rotation speed adjustment buttons
        increaseMoveSpeed.onClick.AddListener(() => ChangeMoveSpeed(1));
        decreaseMoveSpeed.onClick.AddListener(() => ChangeMoveSpeed(-1));

        increaseRotationSpeed.onClick.AddListener(() => ChangeRotationSpeed(30));
        decreaseRotationSpeed.onClick.AddListener(() => ChangeRotationSpeed(-30));

        // Set up reset button to restore default flocking values
        resetButton.onClick.AddListener(() => ResetValues());
    }

    /// <summary>
    /// Changes rotation speed and updates UI
    /// </summary>
    /// <param name="change"></param>
    private void ChangeRotationSpeed(float change)
    {
        BoidManager.Instance.rotationSpeed += change;
        UpdateText();
    }

    /// <summary>
    /// Changes movement speed and updates UI
    /// </summary>
    /// <param name="change"></param>
    private void ChangeMoveSpeed(float change)
    {
        BoidManager.Instance.moveSpeed += change;
        UpdateText();
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
