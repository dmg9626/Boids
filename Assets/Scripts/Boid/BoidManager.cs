using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidManager : Singleton<BoidManager>
{
    public Boid.Settings settings;

    /// <summary>
    /// Reference to boid prefab (used for instantiation)
    /// </summary>
    [SerializeField]
    private Boid boidPrefab;
    
    [SerializeField]    
    private Button clickDetection;

    [Header("Boid Count Settings")]

    /// <summary>
    /// Starting boids in scene
    /// </summary>
    [SerializeField]
    private int startingBoids;

    /// <summary>
    /// Current number of boids in scene
    /// </summary>
    public int numberOfBoids {get; private set;}

    /// <summary>
    /// Maximum number of boids the scene can handle
    /// </summary>
    [SerializeField]
    private int maxBoids = 400;


    float distanceFromCamera = 10f;

    void Start()
    {
        // Instantiate boids in scene
        for(int i = 0; i < startingBoids; i++) {
            // Spawn bird at random position
            Boid boid = SpawnBoid(new Vector3(Random.value, Random.value, distanceFromCamera));
        }

        // Spawn boid at click position if haven't exceeded max count
        clickDetection.onClick.AddListener(() => {
            if(numberOfBoids < maxBoids) {
                Vector3 screenPos = Input.mousePosition;
                Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
                viewportPos.z = distanceFromCamera;

                // Debug.Log("Spawning boid at " + viewportPos);
                Boid boid = SpawnBoid(viewportPos);
            }
        });
    }



    /// <summary>
    /// Spawn boid at given position in viewport space (x-y coordinates between 0 and 1)
    /// </summary>
    /// <param name="viewportPosition">Spawn position in viewport</param>
    /// <returns></returns>
    Boid SpawnBoid(Vector3 viewportPosition) 
    {
        // Increment number of boids
        numberOfBoids++;
        
        Boid boid = Instantiate(boidPrefab);
        boid.name = "Boid " + numberOfBoids;

        // Set given position
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPosition);
        boid.transform.position = worldPos;

        return boid;
    }
}