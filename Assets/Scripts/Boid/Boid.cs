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
    /// Maximum rotation speed (in degrees-per-seceond)
    /// </summary>
    /// <value></value>
    private float rotationSpeed {
        get {
            return BoidManager.Instance.rotationSpeed;
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

        Vector3 separation = Vector3.zero;
        
        List<Boid> boids = BoidDetection.Instance.NearbyBoids(this);
        if(boids.Count > 0) {
            separation = BoidDetection.Instance.GetSeparation(this, boids).normalized;
        }
        
        // TODO: Check movement direction of nearby boids and return vector with average of directions
        Vector3 alignment = BoidDetection.Instance.GetAlignment(this, boids).normalized;

        // TODO: CHeck postiions of nearby boids and return vector towards center of flock (avg. position)
        Vector3 cohesion = BoidDetection.Instance.GetCohesion(this, boids).normalized;

        // Get averages of all vectors
        Vector3 sum = (forward + separation + alignment + cohesion).normalized;
        Debug.DrawRay(transform.position, sum * moveSpeed, Color.green);

        // Apply resulting rotation to boid
        RotateTowards(sum);

        // Move in direction of new rotation (scaled by movement speed)
        transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Rotates boid in given direction
    /// </summary>
    /// <param name="direction">Target rotation</param>
    void RotateTowards(Vector3 direction)
    {
        // Get angle to target (in degrees)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate rotation towards target direction
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // Smooth rotation towards target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
