using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : Singleton<BoidManager>
{
    /// <summary>
    /// Reference to boid prefab (used for instantiation)
    /// </summary>
    [SerializeField]
    private Boid boidPrefab;

    /// <summary>
    /// Number of boids to spawn
    /// </summary>
    [SerializeField]
    private int numberOfBoids;

    /// <summary>
    /// Boid movement speed
    /// </summary>
    public float moveSpeed = 15;

    /// <summary>
    /// Boid rotation speed
    /// </summary>
    public float rotationSpeed = 360;

    float distanceFromCamera = 10f;

    void Start()
    {
        // Instantiate boids in scene
        for(int i = 0; i < numberOfBoids; i++) {
            // Spawn bird at random position
            Boid boid = SpawnBoid();
            boid.name = "Boid " + (i+1);
        }
    }

    void Update()
    {
        // Click to spawn boid at mouse position
        if(Input.GetMouseButtonDown(0)) {
            Vector3 screenPos = Input.mousePosition;
            Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
            viewportPos.z = distanceFromCamera;

            Debug.Log("Spawning boid at " + viewportPos);
            Boid boid = SpawnBoid(viewportPos);
        }
    }

    /// <summary>
    /// Spawn boid at given position in viewport space (x-y coordinates between 0 and 1)
    /// </summary>
    /// <param name="viewportPosition"></param>
    /// <returns></returns>
    Boid SpawnBoid(Vector3 viewportPosition) 
    {
        Boid boid = Instantiate(boidPrefab);

        // Set random boid rotation
        boid.SetRotation(Random.Range(0f,360f));
        
        // Set given position
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPosition);
        boid.transform.position = worldPos;
        
        return boid;
    }
    
    /// <summary>
    /// Spawn boid at random position on screen
    /// </summary>
    /// <returns></returns>
    Boid SpawnBoid()
    {
        Vector3 viewportPos = new Vector3(Random.value, Random.value, distanceFromCamera);
        return SpawnBoid(viewportPos);
    }
}