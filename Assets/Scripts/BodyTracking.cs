using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTracking : MonoBehaviour
{
   public Transform targetObject;  // The object to follow and match rotation
    public float distanceBelow; // The distance to stay below the target
    public float smoothSpeed;   // The smoothness of movement

    void Update()
    {
        if (targetObject != null)
        {
            // Get the target's position
            Vector3 targetPosition = targetObject.position;

            // Calculate the new position with the constant vertical offset
            Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y - distanceBelow, targetPosition.z - 0.1f);

            // Smoothly interpolate towards the new position
            transform.position = Vector3.Lerp(transform.position, newPosition, smoothSpeed * Time.deltaTime);
        }
    }
}
