using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bounds : MonoBehaviour
{
    Camera camera;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    /// <summary>
    /// Fit quad to camera bounds (not working atm)
    /// </summary>
    void FitToCameraBounds()
    {
        Vector3 bottomLeftBound = camera.ViewportToWorldPoint(new Vector3(0,0,camera.nearClipPlane));
        Vector3 topRightBound = camera.ViewportToWorldPoint(new Vector3(1,1,camera.nearClipPlane));
        float width = Mathf.Abs(bottomLeftBound.x - topRightBound.x);
        float height = Mathf.Abs(bottomLeftBound.y - topRightBound.y);

        // Debug.Log("Camera.Main.orthographicSize: " + camera.orthographicSize);
        transform.localScale = new Vector3(width, height, 1);
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        // Get position in viewport-space
        Vector3 otherWorldPos = other.transform.position;
        Vector3 otherViewportPos = camera.WorldToViewportPoint(otherWorldPos);

        // Warp to other side of bounds
        otherViewportPos = WarpToBounds(otherViewportPos);

        // Convert back to world-space and update boid position
        otherWorldPos = camera.ViewportToWorldPoint(otherViewportPos);
        other.transform.position = otherWorldPos;
    }

    Vector3 WarpToBounds(Vector3 otherViewportPos)
    {
        if(otherViewportPos.y > 1) {
            otherViewportPos.y = 0;
        }
        else if(otherViewportPos.y < 0) {
            otherViewportPos.y = 1;
        }

        if(otherViewportPos.x > 1) {
            otherViewportPos.x = 0;
        }
        else if(otherViewportPos.x < 0) {
            otherViewportPos.x = 1;
        }
        return otherViewportPos;
    }
}
