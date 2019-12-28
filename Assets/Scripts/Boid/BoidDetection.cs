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
    /// Frequency at which to perform detection
    /// </summary>
    [SerializeField]
    [Range(0, .5f)]
    private float raycastFrequency;

    /// <summary>
    /// Width (in degrees) of cone used for detection
    /// </summary>
    [SerializeField]
    [Range(0, 360)]
    private float detectionDegrees;

    /// <summary>
    /// Radius of cone used for detection
    /// </summary>
    [SerializeField]
    private float detectionRange;

    /// <summary>
    /// Returns all nearby boids, or null if none detected
    /// </summary>
    /// <param name="boid">Boid</param>
    /// <returns></returns>
    public List<Boid> NearbyBoids(Boid boid)
    {
        // Perform raycasts around detection slice, return any boids found
        float halfAngle = detectionDegrees / 2;

        // Degrees between each raycast
        float angleStep = detectionDegrees / raycastCount;
        
        // Get starting/ending angles for raycasts
        float startAngle = boid.transform.rotation.z - halfAngle;
        float currentAngle = startAngle;

        Vector3 origin = boid.transform.position;

        List<Boid> boids = new List<Boid>();
        
        // Perform each raycast, revolving from left to right
        for(int i = 0; i < raycastCount; i++) {
            // Get direction of current raycast
            Quaternion rotation = Quaternion.Euler(0,0,boid.transform.rotation.z);
            Vector3 direction = rotation.eulerAngles;
            
            // TODO: raycast for nearby boids
            // if(Physics.Raycast(origin, direction, out RaycastHit hitInfo, detectionRange)) {
                
            // }
        }

        return boids;
    }
}
