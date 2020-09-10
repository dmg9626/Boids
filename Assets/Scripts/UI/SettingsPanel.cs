using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    /// <summary>
    /// Contains UI elements that control settings
    /// </summary>
    [SerializeField]
    private GameObject bodyContainer;

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
    /// Hides/shows settings panel when clicked
    /// </summary>
    [SerializeField]
    private Button toggleMenuButton;
    
    /// <summary>
    /// Resets flocking parameters to default values
    /// </summary>
    [SerializeField]
    private Button resetButton;
    #endregion

    #region UISettings
    [Header("Panel Settings")]
    [SerializeField]
    private float openedPanelHeight = 750;
    [SerializeField]
    private float closedPanelHeight = 100;

    /// <summary>
    /// True if panel is open, false if minimized
    /// </summary>
    [SerializeField]
    private bool panelOpen;
    #endregion


    private float defaultSeparationWeight;
    private float defaultAlignmentWeight;
    private float defaultCohesionWeight;

    private Boid.Settings settings;

    void Start()
    {
        settings = BoidManager.Instance.settings;

        InitializeSliders();
        InitializeButtons();
        UpdateText();
    }

    private void UpdateText()
    {
        rotationSpeed.text = settings.rotationSpeed.ToString();
        movementSpeed.text = settings.moveSpeed.ToString();
    }

    private void InitializeSliders()
    {
        // Initialize sliders with default values
        separationSlider.value = settings.separation;
        alignmentSlider.value = settings.alignment;
        cohesionSlider.value = settings.cohesion;

        // Store default values so we can restore them later
        defaultSeparationWeight = settings.separation;
        defaultAlignmentWeight = settings.alignment;
        defaultCohesionWeight = settings.cohesion;

        // Set up slider events to update boid settings with changed values
        separationSlider.onValueChanged.AddListener((value) => settings.separation = value);
        alignmentSlider.onValueChanged.AddListener((value) => settings.alignment = value);
        cohesionSlider.onValueChanged.AddListener((value) => settings.cohesion = value);
    }

    private void InitializeButtons()
    {
        // Set up events for minimize/maximize buttons
        toggleMenuButton.onClick.AddListener(() =>
        {
            panelOpen = !panelOpen;
            SetPanelOpen(panelOpen);
        });

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
        settings.rotationSpeed += change;
        UpdateText();
    }

    /// <summary>
    /// Changes movement speed and updates UI
    /// </summary>
    /// <param name="change"></param>
    private void ChangeMoveSpeed(float change)
    {
        settings.moveSpeed += change;
        UpdateText();
    }

    /// <summary>
    /// Hides/shows settings panel
    /// </summary>
    private void SetPanelOpen(bool open)
    {
        // Hide/show menu settings
        bodyContainer.SetActive(open);

        // Expand/minimize menu panel size
        RectTransform panelRect = transform as RectTransform;
        panelRect.sizeDelta = new Vector2(panelRect.sizeDelta.x, (open ? openedPanelHeight : closedPanelHeight));

        // Flip button sprite to face up/down when closed/open
        toggleMenuButton.transform.localScale = new Vector3(1, (open ? 1 : -1), 1);
    }

    public void ResetValues()
    {
        // Reset sliders with default values
        separationSlider.value = defaultSeparationWeight;
        alignmentSlider.value = defaultAlignmentWeight;
        cohesionSlider.value = defaultCohesionWeight;

        // Restore default values for each setting
        settings.separation = defaultSeparationWeight;
        settings.alignment = defaultAlignmentWeight;
        settings.cohesion = defaultCohesionWeight;
    }
}
