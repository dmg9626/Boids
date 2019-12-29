using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    /// <summary>
    /// Boid movement speed (assigned by BoidManager)
    /// </summary>
    private float moveSpeed {
        get {
            return BoidManager.Instance.moveSpeed;
        }
        set{}
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
        Vector3 avoidance = Vector3.zero;
        
        List<Boid> boids = BoidDetection.Instance.NearbyBoids(this);
        if(boids.Count > 0) {
            Debug.LogWarningFormat("{0} | detected {1} nearby boids", name, boids.Count);
            avoidance = BoidDetection.AvoidBoids(this, boids).normalized;
        }
        

        // TODO: Check movement direction of nearby boids and return vector with average of directions

        // Get averages of all vectors
        Vector3 sum = (forward + avoidance).normalized;

        Vector3 movement = sum * moveSpeed;
        Debug.DrawRay(transform.position, movement, Color.green);

        // Apply movement to boid (scaled by movement speed)
        sum *= moveSpeed * Time.fixedDeltaTime;
        transform.position += sum;
    }
}
