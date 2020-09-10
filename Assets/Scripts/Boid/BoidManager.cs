using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidManager : Singleton<BoidManager>
{
    public Boid.Settings settings;
    
    [Header("Boid Count Settings")]

    /// <summary>
    /// Current number of boids in scene
    /// </summary>
    public int numberOfBoids;

    /// <summary>
    /// Maximum number of boids the scene can handle
    /// </summary>
    public int maxBoids { get { return 400; } }
}