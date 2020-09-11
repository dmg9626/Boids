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
    private float openedPanelHeight = 0;
    [SerializeField]
    private float closedPanelHeight = -650;

    /// <summary>
    /// Speed at which settings panel slides open/closed
    /// </summary>
    [Range(0, 5)]
    [SerializeField]
    private float slideSpeed = 2.5f;

    [SerializeField]
    private AnimationCurve slideCurve;

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

        // Set panel to starting position
        StartCoroutine(SetPanelOpen(panelOpen, false));
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
        // Toggle menu button slides panel open/closed (when not already moving)
        toggleMenuButton.onClick.AddListener(() =>
        {
            if(setPanelCoroutine == null) {
                panelOpen = !panelOpen;
                setPanelCoroutine = StartCoroutine(SetPanelOpen(panelOpen));
            }
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


    private Coroutine setPanelCoroutine;

    /// <summary>
    /// Hides/shows settings panel
    /// </summary>
    private IEnumerator SetPanelOpen(bool open, bool animate = true)
    {
        // Flip button sprite to face up/down when closed/open
        toggleMenuButton.transform.localScale = new Vector3(1, (open ? 1 : -1), 1);

        // Slide panel with animation
        setPanelCoroutine = StartCoroutine(SlidePanel(open, animate));
        while (setPanelCoroutine != null)
            yield return null;
    }

    private IEnumerator SlidePanel(bool open, bool animate)
    {
        RectTransform panelRect = transform as RectTransform;
        Vector2 startPosition = new Vector2(panelRect.position.x, !open ? openedPanelHeight : closedPanelHeight);
        Vector2 endPosition = new Vector2(panelRect.position.x, open ? openedPanelHeight : closedPanelHeight);

        if (animate) {
            for (float t = 0; t < 1; t += Time.deltaTime * slideSpeed) {
                // Slide panel up/down a bit
                float height = Mathf.LerpUnclamped(startPosition.y, endPosition.y, slideCurve.Evaluate(t));
                panelRect.position = new Vector2(panelRect.position.x, height);
                yield return null;
            }
        }
        panelRect.position = endPosition;

        setPanelCoroutine = null;
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
