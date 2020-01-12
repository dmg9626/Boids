using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoidDetection
{
    /// <summary>
    /// Returns all nearby boids, or null if none detected
    /// </summary>
    /// <param name="self">Boid</param>
    /// <returns></returns>
    public static List<Boid> NearbyBoids(Boid self, Boid.Settings settings)
    {
        // Perform raycasts around detection slice, return any boids found
        float halfAngle = settings.degrees / 2;

        // Degrees between each raycast
        float angleStep = settings.degrees / settings.raycastCount;
        
        // Get starting/ending angles for raycasts
        float startAngle = self.transform.rotation.z - halfAngle;
        float currentAngle = startAngle;

        Vector3 origin = self.transform.position;

        List<Boid> nearbyBoids = new List<Boid>();
        
        // Instantiate array to hold hits returned by each raycast (required for Physics.RaycastNonAlloc)
        int boidsPerRaycast = 2;
        RaycastHit[] hits = new RaycastHit[boidsPerRaycast];

        // Perform each raycast, revolving from left to right
        for(int i = 0; i < settings.raycastCount; i++) {
            
            // Create ray with current angle
            Quaternion rotation = Quaternion.Euler(0,0,currentAngle);
            Vector3 direction = rotation * self.transform.up;
            Ray ray = new Ray(origin, direction);

            // Show ray in editor
            Debug.DrawRay(ray.origin, ray.direction * settings.range, Color.red);

            // Perform raycast on Boid layer, store all hits returned
            int layermask = 1 << LayerMask.NameToLayer("Boid");
            int numHits = Physics.RaycastNonAlloc(ray, hits, settings.range, layermask, QueryTriggerInteraction.Collide);

            // If raycast hit any boids, add them to list
            for(int j = 0; j < numHits; j++) {
                RaycastHit hit = hits[j];
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
    public static Vector3 GetSeparation(Boid self, List<Boid> neighbors, Boid.Settings settings)
    {
        Vector3 sum = Vector3.zero;
        foreach(Boid boid in neighbors) {
            // Get direction of separation
            Vector3 direction = (self.transform.position - boid.transform.position);

            // Apply inverse square law to get strength of repulsion force
            float distance = direction.magnitude;
            float strength = settings.maxAcceleration * (settings.range - distance) / distance;

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
    public static Vector3 GetAlignment(Boid self, List<Boid> neighbors, Boid.Settings settings)
    {
        if (neighbors.Count == 0) {
            return Vector3.zero;
        }

        // Calculate average direction of neighbors
        Vector3 sum = self.transform.up;
        foreach(Boid boid in neighbors) {
            sum += boid.transform.up;
        }

        return (sum / neighbors.Count);
    }

    /// <summary>
    /// Returns vector of cohesion (average position of neighbors) for given boid
    /// </summary>
    /// <param name="self">Boid to calculate separation for</param>
    /// <param name="neighbors">Neighboring boids</param>
    /// <returns></returns>
    public static Vector3 GetCohesion(Boid self, List<Boid> neighbors, Boid.Settings settings)
    {
        if (neighbors.Count == 0) {
            return Vector3.zero;
        }

        // Calculate average position of neighbors
        Vector3 sum = Vector3.zero;
        foreach(Boid boid in neighbors) {
            sum += boid.transform.position;
        }
        Vector3 averagePosition = sum / neighbors.Count;

        // Return vector towards average position
        return (averagePosition - self.transform.position);
    }
}
