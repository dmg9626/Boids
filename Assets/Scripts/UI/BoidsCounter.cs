using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class BoidsCounter : MonoBehaviour
{
    /// <summary>
    /// Boids count text
    /// </summary>
    private Text boidCountText;

    private BoidManager boidManager;
    
    void Start()
    {
        boidManager = BoidManager.Instance;
        boidCountText = GetComponent<Text>();
    }

    void Update()
    {
        // Update boid count
        boidCountText.text = "Boids: " + boidManager.numberOfBoids.ToString();
    }
}
