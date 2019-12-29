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

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate boids in scene
        for(int i = 0; i < numberOfBoids; i++) {
            Boid boid = Instantiate(boidPrefab);
            boid.name = "Boid " + (i+1);

            // Set random boid rotation
            boid.SetRotation(Random.Range(0f,360f));

            // Set random boid position 
            float distanceFromCamera = 10f;
            Vector3 viewportPos = new Vector3(Random.value, Random.value, distanceFromCamera);
            Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
            boid.transform.position = worldPos;
            Debug.Log("Instantiating boid at position " + worldPos);
        }
    }
}
