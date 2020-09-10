using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidSpawner : Singleton<BoidSpawner>
{
    private BoidManager boidManager => BoidManager.Instance;

    /// <summary>
    /// Reference to boid prefab (used for instantiation)
    /// </summary>
    [SerializeField]
    private Boid boidPrefab;

    [SerializeField]
    private Button clickDetection;

    float distanceFromCamera = 10f;

    /// <summary>
    /// Starting boids in scene
    /// </summary>
    [SerializeField]
    private int startingBoids;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate boids in scene
        for (int i = 0; i < startingBoids; i++)
        {
            // Spawn bird at random position
            Boid boid = SpawnBoid(new Vector3(Random.value, Random.value, distanceFromCamera));
        }

        // Spawn boid at click position if haven't exceeded max count
        clickDetection.onClick.AddListener(() => {
            if (boidManager.numberOfBoids < boidManager.maxBoids)
            {
                Vector3 screenPos = Input.mousePosition;
                Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
                viewportPos.z = distanceFromCamera;

                Boid boid = SpawnBoid(viewportPos);
            }
        });
    }

    /// <summary>
    /// Spawn boid at given position in viewport space (x-y coordinates between 0 and 1)
    /// </summary>
    /// <param name="viewportPosition">Spawn position in viewport</param>
    /// <returns></returns>
    public Boid SpawnBoid(Vector3 viewportPosition)
    {
        Boid boid = Instantiate(boidPrefab);
        boid.name = "Boid " + BoidManager.Instance.numberOfBoids;

        // Set given position
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPosition);
        boid.transform.position = worldPos;

        boidManager.numberOfBoids++;
        return boid;
    }
}
