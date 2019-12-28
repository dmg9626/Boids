using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidManager : MonoBehaviour
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
    private int numberOfBoids = 1;

    /// <summary>
    /// Boid movement speed
    /// </summary>
    [SerializeField]
    private float moveSpeed = 15;


    // Start is called before the first frame update
    void Start()
    {
        // Instantiate boids in scene
        for(int i = 0; i < numberOfBoids; i++) {
            Boid boid = Instantiate(boidPrefab);

            // Set boid speed
            boid.SetMoveSpeed(moveSpeed);

            // TODO: set boid position
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
