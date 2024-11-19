using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicRig : MonoBehaviour
{

    public Transform playerHead;
    public Transform leftController;
    public Transform rightController;

    public ConfigurableJoint headJoint;
    public ConfigurableJoint leftHandJoint;
    public ConfigurableJoint rightHandJoint;

    public CapsuleCollider bodyCollider;

    public float bodyHeightMin = 0.5f;
    public float bodyHeightMax = 2.0f;


    // Update is called once per frame
    void FixedUpdate()
    {
        bodyCollider.height = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        bodyCollider.center = new Vector3(playerHead.localPosition.x, bodyCollider.height / 2, playerHead.localPosition.z);

        leftHandJoint.targetPosition = leftController.localPosition;
        leftHandJoint.targetRotation = leftController.localRotation;
        
        rightHandJoint.targetPosition = rightController.localPosition;
        rightHandJoint.targetRotation = rightController.localRotation;
        
        headJoint.targetPosition = playerHead.localPosition;

        // Adjust the body collider height and center
        //float clampedHeight = Mathf.Clamp(playerHead.localPosition.y, bodyHeightMin, bodyHeightMax);
        //bodyCollider.height = clampedHeight;
        //bodyCollider.center = new Vector3(playerHead.localPosition.x, clampedHeight / 2, playerHead.localPosition.z);
        //
        //// Set joint target positions and rotations
        //leftHandJoint.targetPosition = leftHandJoint.transform.InverseTransformPoint(leftController.position);
        //leftHandJoint.targetRotation = Quaternion.Inverse(leftHandJoint.transform.rotation) * leftController.rotation;
        //
        //rightHandJoint.targetPosition = rightHandJoint.transform.InverseTransformPoint(rightController.position);
        //rightHandJoint.targetRotation = Quaternion.Inverse(rightHandJoint.transform.rotation) * rightController.rotation;
        //
        //headJoint.targetPosition = headJoint.transform.InverseTransformPoint(playerHead.position);


    }
}
