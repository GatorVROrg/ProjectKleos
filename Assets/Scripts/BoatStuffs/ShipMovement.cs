using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public Transform Ship;
    public float value;
    public float speed = 0;
    public float rotationSpeed = 0;
    public float acceleration = 0;

    public Transform rope;
    public Transform socket;
    public Transform Tether;
    public LineRenderer ropeVisual;
    public Transform ropeHold;
    private Animator animator;
    private GameObject player;
    public bool ropeGrabbed;
    public bool tethered;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        // if (IsPlayerOnBoat())
        // {
        //     MakePlayerChildOfBoat();
        // }
        // else
        // {
        //     ReleasePlayerFromBoat();
        // }

        float originalMin = 0.0f;
        float originalMax = 1f;

        float newMin = -0.5f;
        float newMax = 0.5f;

        float mappedValue = (value - originalMin) / (originalMax - originalMin) * (newMax - newMin) + newMin;

        float distance = Vector3.Distance(rope.position, socket.position);

        float distanceToTether = Vector3.Distance(rope.position, Tether.position);
        if(distanceToTether <= 1)
        {
            tethered = true;
            acceleration = Mathf.Abs(acceleration);
            animator.SetBool("SailDown", true);
            animator.SetBool("SailUp", false);
        }
        else
        {
            tethered = false;
        }

        if(ropeGrabbed || tethered)
        {
            speed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), 0, distance * 1000);
            // ropeVisual.SetPosition(0, ropeHold.position);
            // ropeVisual.SetPosition(1, rope.position);
        }
        else
        {
            // ropeVisual.SetPosition(0, ropeHold.position);
            // ropeVisual.SetPosition(1, ropeHold.position);
            speed = 0;
        }

        // Calculate the rotation amount based on the knob value
        float rotationAmount = (Mathf.Abs(mappedValue) + 1) * rotationSpeed * speed * Time.deltaTime;

        if (mappedValue > 0.2f)
        {
            // Rotate the ship to the right
            Ship.Rotate(Vector3.up, rotationAmount);
        }
        else if (mappedValue < -0.2f)
        {
            // Rotate the ship to the left
            Ship.Rotate(Vector3.up, -rotationAmount);
        }
        if (speed != 0)
        {
            Ship.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void GrabbedRope()
    {
        ropeGrabbed = true;
        acceleration = Mathf.Abs(acceleration);
        
    }
    public void ReleasedRope()
    {
        ropeGrabbed = false;
        acceleration = -acceleration;
        animator.SetBool("SailDown", false);
        animator.SetBool("SailUp", true);
    }

    public void Tethered()
    {
        tethered = true;
        acceleration = Mathf.Abs(acceleration);
        animator.SetBool("SailDown", true);
        animator.SetBool("SailUp", false);
    }

    public void UnTethered()
    {
        tethered = false;
    }

    private bool IsPlayerOnBoat()
    {
        if (Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            // Check if the raycast hit the boat's collider
            if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ship"))
            {
                // The player is standing on the boat
                Debug.Log("Standing On da Boat");
                return true;
            }
        }
        // The player is not standing on the boat
        return false;
    }

    private void MakePlayerChildOfBoat()
    {
        player.transform.SetParent(Ship);
    }

    private void ReleasePlayerFromBoat()
    {
        player.transform.SetParent(null);
    }
}
