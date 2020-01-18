using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour Type;

    /// <summary>
    /// Reference to object prefab
    /// </summary>
    [SerializeField]
    private GameObject Object;

    /// <summary>
    /// Amount of objects to initialize pool with
    /// </summary>
    [SerializeField]
    private int startCount;

    /// <summary>
    /// When user requests more objects and pool is full, expand pool capacity by this much
    /// </summary>
    [SerializeField]   
    private int sizeIncrement;

    /// <summary>
    /// Object pool
    /// </summary>
    private List<GameObject> pool;

    void Start()
    {
        // Initialize pool with objects
        for(int i = 0; i < startCount; i++) {
            GameObject obj = Instantiate(Object);
            pool.Add(obj);
        }
    }
}
