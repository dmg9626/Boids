using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boid : MonoBehaviour
{
    /// <summary>
    /// Boid movement speed (assigned by BoidManager)
    /// </summary>
    private float moveSpeed;

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    void Start()
    {
        float angle = Random.Range(0,360);
        Quaternion rotation = Quaternion.Euler(0,0,angle);
        transform.rotation = rotation;
    }

    void Update()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        Debug.Log("Viewport position: " + viewportPosition);

        // TODO: Fly in straight line
        transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;

        // TODO: Check for collision with other boids and return avoidance vector

        // TODO: Check movement direction of nearby boids and return vector with average of directions

        // TODO: Warp to other side of bounds if reached edge
    }
}
