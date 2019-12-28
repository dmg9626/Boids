using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoidDetection))]
public class Boid : MonoBehaviour
{
    /// <summary>
    /// Used to detect nearby boids for separation/alignment calculation
    /// </summary>
    [SerializeField]
    private BoidDetection boidDetection;

    /// <summary>
    /// Boid movement speed (assigned by BoidManager)
    /// </summary>
    private float moveSpeed;
    
    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    /// <summary>
    /// Sets boid rotation (in degrees)
    /// </summary>
    /// <param name="degrees"></param>
    public void SetRotation(float degrees)
    {
        // Set z-rotation to given angle
        transform.rotation = Quaternion.Euler(0,0,degrees);
    }

    void Update()
    {
        // Get movement in straight line
        Vector3 forward = transform.up;

        // TODO: Check for collision with other boids and return avoidance vector
        // TODO: Check movement direction of nearby boids and return vector with average of directions

        // Get averages of all vectors
        Vector3 sum = (forward).normalized;

        // Apply movement to boid (scaled by movement speed)
        sum *= moveSpeed * Time.fixedDeltaTime;
        transform.position += sum;

    }
}
