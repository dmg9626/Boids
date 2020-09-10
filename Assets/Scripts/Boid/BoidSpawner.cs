using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoidSpawner : Singleton<BoidSpawner>
{
    private BoidManager boidManager => BoidManager.Instance;

    /// <summary>
    /// Parent object that holds all boids spawned
    /// </summary>
    private GameObject boidContainer;

    /// <summary>
    /// Reference to boid prefab (used for instantiation)
    /// </summary>
    [SerializeField]
    private Boid boidPrefab;

    float distanceFromCamera = 10f;

    /// <summary>
    /// Starting boids in scene
    /// </summary>
    [SerializeField]
    private int startingBoids;

    /// <summary>
    /// UI element that detects when user clicks screen
    /// </summary>
    [SerializeField]
    private Button clickDetection;

    #region TapIndicator
    [Header("Tap Indicator Settings")]

    [SerializeField]
    private GameObject tapIndicatorPrefab;

    /// <summary>
    /// Speed at which tap indicator animates
    /// </summary>
    [Range(0, 2)]
    [SerializeField]
    private float tapIndicatorSpeed;

    [SerializeField]
    private Color tapIndicatorColor;
    #endregion

    [SerializeField]
    private float boidSpawnWobbleSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Create container to hold all spawned boids
        boidContainer = new GameObject("Boids");

        // Instantiate boids in scene at random positions
        for (int i = 0; i < startingBoids; i++)
            SpawnBoid(new Vector3(Random.value, Random.value, distanceFromCamera));

        // Spawn boid at click position if haven't exceeded max count
        clickDetection.onClick.AddListener(() => {
            if (boidManager.numberOfBoids < boidManager.maxBoids)
            {
                Vector3 screenPos = Input.mousePosition;
                Vector3 viewportPos = Camera.main.ScreenToViewportPoint(screenPos);
                viewportPos.z = distanceFromCamera;

                // Spawn boid
                Boid boid = SpawnBoid(viewportPos);

                // Create circular pulse at spawn position
                StartCoroutine(CreatePulse(boid.transform.position));

                // Bounce its scale
                StartCoroutine(WobbleBoidScale(boid));
            }
        });
    }

    /// <summary>
    /// Bounces boid scale from zero to full, with some wobble at the end
    /// </summary>
    /// <param name="boid"></param>
    /// <returns></returns>
    IEnumerator WobbleBoidScale(Boid boid)
    {
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = boidPrefab.transform.localScale;
        for(float t = 0; t < 1; t += Time.deltaTime * boidSpawnWobbleSpeed)
        {
            boid.transform.localScale = Mathfx.Berp(startScale, endScale, t);
            yield return null;
        }
        boid.transform.localScale = endScale;
    }

    /// <summary>
    /// Spawn boid at given position in viewport space (x-y coordinates between 0 and 1)
    /// </summary>
    /// <param name="viewportPosition">Spawn position in viewport</param>
    /// <returns></returns>
    public Boid SpawnBoid(Vector3 viewportPosition)
    {
        Boid boid = Instantiate(boidPrefab, boidContainer.transform);
        boid.name = "Boid " + BoidManager.Instance.numberOfBoids;

        // Set given position
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPosition);
        boid.transform.position = worldPos;

        boidManager.numberOfBoids++;
        return boid;
    }

    // TODO: consider optimizing this to use an object pool
    /// <summary>
    /// Coroutine animation that indicates position tapped by user
    /// </summary>
    /// <param name="tapPosition"></param>
    /// <returns></returns>
    private IEnumerator CreatePulse(Vector3 tapPosition)
    {
        // Set tap indicator to initial state (active = true, at tap position, scale = 0)
        GameObject tapIndicator = Instantiate(tapIndicatorPrefab);
        tapIndicator.transform.position = tapPosition;
        tapIndicator.transform.localScale = Vector3.zero;

        // Set starting color
        SpriteRenderer spriteRenderer = tapIndicator.GetComponent<SpriteRenderer>();
        spriteRenderer.color = tapIndicatorColor;
        Color color = spriteRenderer.color;

        for(float t = 0; t < 1; t += Time.deltaTime * tapIndicatorSpeed) {
            // Move scale value up the curve (grow from 0 to 1)
            tapIndicator.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            
            // Move color alpha value down the curve (shrink from 1 to 0)
            color.a = Mathf.Lerp(1, 0, t);
            spriteRenderer.color = color;

            yield return null;
        }

        // Destroy when animation finished
        Destroy(tapIndicator);
    }
}
