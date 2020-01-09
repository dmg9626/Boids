using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        #region BoidBehaviorSettings

        [Header("Boid Behavior Settings")]
        /// <summary>
        /// Weight used to control separation force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float separation;
        
        /// <summary>
        /// Weight used to control alignment force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float alignment;
        
        /// <summary>
        /// Weight used to control cohesion force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float cohesion;

        #endregion

        #region DetectionSettings

        [Header("Detection Settings")]

        /// <summary>
        /// Number of raycasts to perform for detection
        /// </summary>
        public int raycastCount;

        /// <summary>
        /// Width (in degrees) of cone used for detection
        /// </summary>
        [Range(0, 360)]
        public float degrees;

        /// <summary>
        /// Radius of cone used for detection
        /// </summary>
        public float range;

        /// <summary>
        /// Acceleration used when calculating separation
        /// </summary>
        public float maxAcceleration = 15;

        #endregion

        #region MovementSettings
        
        [Header("Movement")]
        /// <summary>
        /// Boid movement speed
        /// </summary>
        public float moveSpeed = 15;

        /// <summary>
        /// Boid rotation speed
        /// </summary>
        public float rotationSpeed = 360;

        #endregion
    }

    private Settings settings;

    /// <summary>
    /// Sets boid rotation (in degrees)
    /// </summary>
    /// <param name="degrees"></param>
    public void SetRotation(float degrees)
    {
        // Set z-rotation to given angle
        transform.rotation = Quaternion.Euler(0,0,degrees);
    }

    void Start()
    {
        settings = BoidManager.Instance.settings;
    }

    void Update()
    {
        // Get movement in straight line
        Vector3 forward = transform.up;

        List<Boid> boids = BoidDetection.NearbyBoids(this, settings);
        
        Vector3 separation = BoidDetection.GetSeparation(this, boids, settings).normalized;
        Vector3 alignment = BoidDetection.GetAlignment(this, boids, settings).normalized;
        Vector3 cohesion = BoidDetection.GetCohesion(this, boids, settings).normalized;

        // Get averages of all vectors
        Vector3 sum = (forward + separation + alignment + cohesion).normalized;
        Debug.DrawRay(transform.position, sum * settings.moveSpeed, Color.green);

        // Apply resulting rotation to boid
        RotateTowards(sum);

        // Move in direction of new rotation (scaled by movement speed)
        transform.position += transform.up * settings.moveSpeed * Time.fixedDeltaTime;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * settings.rotationSpeed);
    }
}
