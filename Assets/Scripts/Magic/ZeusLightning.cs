using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ZeusLightning : MonoBehaviour
{
    public Transform playerHead;
    public Transform firepoint;
    public LightingEffect lightingEffect;
    private Ray ray;
    private RaycastHit hitData;
    private Vector3 Target;

    public InputActionProperty RightTrigger;

    private bool isShooting = false;
    private bool Charged = false;


    // Update is called once per frame
    void Update()
    {
        // Gets Inputs
        if (RightTrigger.action.ReadValue<float>() == 1)
        {
            if(firepoint.position.y > playerHead.position.y && !Charged)
            {
                Charge();
                Charged = false;
            }
            else if(Charged)
            {
                shoot();
            }
        }

        ray = new Ray(firepoint.position, firepoint.forward);
        Debug.DrawRay(ray.origin, ray.direction * 10);
        if (Physics.Raycast(ray, out hitData, 25))
        {
            // Set the target pose
            Target = hitData.point;

            // // Check if it is a target and we are shooting
            // if (hitData.collider.tag == "Enemy" && isShooting)
            // {
            //     hitData.collider.GetComponent<EnemyHealth>().damage();
            // }
        }
        else
        {
            Target = ray.origin + ray.direction * 25;
        }
    }

    public void shoot()
    {
        if (!isShooting)
        {
            isShooting =true;
            lightingEffect.ZapTarget(Target);
            StartCoroutine(Reload());
        }
    }

    public void Charge()
    {
        Charged = true;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        isShooting = false;
    }
}
