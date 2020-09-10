using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        /// <summary>
        /// Range of colors that newly-spawned boids can have
        /// </summary>
        public Gradient colorRange;

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

        /// <summary>
        /// Weight used to control random wandering force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float noise;

        #endregion

        #region DetectionSettings

        [Header("Detection Settings")]

        /// <summary>
        /// Number of raycasts to perform for detection
        /// </summary>
        [Range(3,30)]
        public int raycastCount;

        /// <summary>
        /// Width (in degrees) of cone used for detection
        /// </summary>
        [Range(0, 360)]
        public float degrees;

        /// <summary>
        /// Radius of cone used for detection
        /// </summary>
        [Range(0,10)]
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

    private Settings settings => BoidManager.Instance.settings;

    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Dictates whether this boid should recalculate its path.
    /// Value alternates between true/false each frame.
    /// </summary>
    private bool update;

    void Start()
    {
        // Randomly determine which frame to update on
        update = Random.Range(0, 2) == 0;

        // Set random boid rotation
        float degrees = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0,0,degrees);

        // Pick random color from colorRange gradient
        if (TryGetComponent(out spriteRenderer)) {
            spriteRenderer.color = settings.colorRange.Evaluate(Random.value);
        }
    }

    void FixedUpdate()
    {
        // Flip value of update
        update = !update;

        // Only recalculate direction every other frame
        if (!update)
        {
            // Turn in direction based on surrounding boids
            Vector3 newDirection = RecalculateDirection();
            RotateTowards(newDirection);
        }
        // Debug.DrawRay(transform.position, sum * settings.moveSpeed, Color.green);

        // Move in direction of new rotation (scaled by movement speed)
        transform.position += transform.up * settings.moveSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Generates new rotation vector based on nearby boids
    /// </summary>
    /// <returns></returns>
    Vector3 RecalculateDirection()
    {
        // Get movement in straight line
        Vector3 forward = transform.up;

        List<Boid> boids = BoidDetection.NearbyBoids(this, settings);

        Vector3 separation = BoidDetection.GetSeparation(this, boids, settings).normalized * settings.separation;
        Vector3 alignment = BoidDetection.GetAlignment(this, boids, settings).normalized * settings.alignment;
        Vector3 cohesion = BoidDetection.GetCohesion(this, boids, settings).normalized * settings.cohesion;

        // Generate random movement vector
        Vector3 noise = Random.insideUnitCircle * settings.noise;

        // Return average of all vectors
        return (forward + separation + alignment + cohesion + noise).normalized;
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

        // Double rotation speed, since we update direction every other frame
        float rotationSpeed = settings.rotationSpeed * 2;
        
        // Smooth rotation towards target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed);
    }
}
