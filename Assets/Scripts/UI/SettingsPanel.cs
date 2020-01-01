using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    #region Sliders
    [SerializeField]
    private Slider separationSlider;

    [SerializeField]
    private Slider alignmentSlider;

    [SerializeField]
    private Slider cohesionSlider;
    #endregion

    private BoidDetection boidDetection;

    // Start is called before the first frame update
    void Start()
    {
        boidDetection = BoidDetection.Instance;

        // Initialize sliders with default values
        separationSlider.value = boidDetection.separationWeight;
        alignmentSlider.value = boidDetection.alignmentWeight;
        cohesionSlider.value = boidDetection.cohesionWeight;

        // Set up slider events to update boid settings with changed values
        separationSlider.onValueChanged.AddListener((value) => boidDetection.separationWeight = value);
        alignmentSlider.onValueChanged.AddListener((value) => boidDetection.alignmentWeight = value);
        cohesionSlider.onValueChanged.AddListener((value) => boidDetection.cohesionWeight = value);
    }

    
    public void ResetValues()
    {
        // TODO: add method to reset values to defaults
    }
}
