using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDetection : Singleton<BoidDetection>
{
    /// <summary>
    /// Number of raycasts to perform for detection
    /// </summary>
    [SerializeField]
    private int raycastCount;

    /// <summary>
    /// Width (in degrees) of cone used for detection
    /// </summary>
    [SerializeField]
    [Range(0, 360)]
    private float degrees;

    /// <summary>
    /// Radius of cone used for detection
    /// </summary>
    [SerializeField]
    private float range;

    /// <summary>
    /// Acceleration used when calculating separation
    /// </summary>
    [SerializeField]
    private float maxAcceleration = 15;

    /// <summary>
    /// Returns all nearby boids, or null if none detected
    /// </summary>
    /// <param name="self">Boid</param>
    /// <returns></returns>
    public List<Boid> NearbyBoids(Boid self)
    {
        // Perform raycasts around detection slice, return any boids found
        float halfAngle = degrees / 2;

        // Degrees between each raycast
        float angleStep = degrees / raycastCount;
        
        // Get starting/ending angles for raycasts
        float startAngle = self.transform.rotation.z - halfAngle;
        float currentAngle = startAngle;

        Vector3 origin = self.transform.position;

        List<Boid> nearbyBoids = new List<Boid>();
        
        // Perform each raycast, revolving from left to right
        for(int i = 0; i < raycastCount; i++) {
            
            // Create ray with current angle
            Quaternion rotation = Quaternion.Euler(0,0,currentAngle);
            Vector3 direction = rotation * self.transform.up;
            Ray ray = new Ray(origin, direction);

            // Show ray in editor
            Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

            // Perform raycast on Boid layer, store all hits returned
            int layermask = 1 << LayerMask.NameToLayer("Boid");
            RaycastHit[] hits = Physics.RaycastAll(ray, range, layermask, QueryTriggerInteraction.Collide);

            foreach(RaycastHit hit in hits) {
                // If raycast hit a boid, add to list
                if(hit.transform.TryGetComponent(out Boid otherBoid)) {
                    nearbyBoids.Add(otherBoid);
                }
            }

            // Increment angle and repeat
            currentAngle += angleStep;
        }

        return nearbyBoids;
    }

    /// <summary>
    /// Returns vector of separation for given boid
    /// </summary>
    /// <param name="self">Boid to calculate separation for</param>
    /// <param name="neighbors">Neighboring boids</param>
    /// <returns></returns>
    public Vector3 GetSeparation(Boid self, List<Boid> neighbors)
    {
        Vector3 sum = Vector3.zero;
        foreach(Boid boid in neighbors) {
            // Get direction of separation
            Vector3 direction = (self.transform.position - boid.transform.position);

            // Apply inverse square law to get strength of repulsion force
            float distance = direction.magnitude;
            float strength = maxAcceleration * (range - distance) / distance;

            Vector3 separation = direction * strength;
            
            // Get sum of vectors away from each boid
            sum += separation;
        }
        return sum;
    }

    /// <summary>
    /// Returns vector of alignment (average movement direction of neighbors) for given boid
    /// </summary>
    /// <param name="self">Boid to calculate separation for</param>
    /// <param name="neighbors">Neighboring boids</param>
    /// <returns></returns>
    public Vector3 GetAlignment(Boid self, List<Boid> neighbors)
    {
        // Calculate average direction of neighbors
        Vector3 sum = self.transform.up;
        foreach(Boid boid in neighbors) {
            // Get direction of boid
            sum += boid.transform.up;
        }

        return sum / neighbors.Count;
    }

    /// <summary>
    /// Returns vector of cohesion (average position of neighbors) for given boid
    /// </summary>
    /// <param name="self">Boid to calculate separation for</param>
    /// <param name="neighbors">Neighboring boids</param>
    /// <returns></returns>
    public Vector3 GetCohesion(Boid self, List<Boid> neighbors)
    {
        // Calculate average position of neighbors
        Vector3 sum = Vector3.zero;
        foreach(Boid boid in neighbors) {
            // Get direction of boid
            sum += boid.transform.position;
        }
        Vector3 averagePosition = sum / neighbors.Count;

        // Return vector towards average position
        return averagePosition - self.transform.position;
    }
}
