using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidDetection : MonoBehaviour
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
    /// Returns all nearby boids, or null if none detected
    /// </summary>
    /// <param name="boid">Boid</param>
    /// <returns></returns>
    public List<Boid> NearbyBoids(Boid boid)
    {
        // Perform raycasts around detection slice, return any boids found
        float halfAngle = degrees / 2;

        // Degrees between each raycast
        float angleStep = degrees / raycastCount;
        
        // Get starting/ending angles for raycasts
        float startAngle = boid.transform.rotation.z - halfAngle;
        float currentAngle = startAngle;

        Vector3 origin = boid.transform.position;

        List<Boid> nearbyBoids = new List<Boid>();
        
        // Perform each raycast, revolving from left to right
        for(int i = 0; i < raycastCount; i++) {
            
            // Create ray with current angle
            Quaternion rotation = Quaternion.Euler(0,0,currentAngle);
            Vector3 direction = rotation * boid.transform.up;
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
}
