using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ZeusLightning : MonoBehaviour
{
    public Transform playerHead;
    public Transform firepoint;
    public GameObject Sword;
    public LightingEffect lightingEffect;

    public Material defaultMaterial;
    public Material chargedMaterial;

    private Ray ray;
    private RaycastHit hitData;
    private Vector3 Target;

    public InputActionProperty RightTrigger;

    private bool isShooting = false;
    private bool Charged = false;

    private PlayerMagic PM;

    void Start()
    {
        PM = gameObject.GetComponent<PlayerMagic>();
    }

    void Update()
    {
        if(PM.HasZeus)
        {
            // Gets Inputs
            if (RightTrigger.action.ReadValue<float>() == 1)
            {
                if(firepoint.position.y > playerHead.position.y && !Charged)
                {
                    Charge();
                    
                }
                else if(Charged)
                {
                    Shoot();
                    Charged = false;
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
    }

    public void Shoot()
    {
        if (!isShooting)
        {
            isShooting =true;
            lightingEffect.ZapTarget(Target);
            Sword.GetComponent<MeshRenderer>().material = defaultMaterial;
            StartCoroutine(Reload());
        }
    }

    public void Charge()
    {
        Charged = true;
        Sword.GetComponent<MeshRenderer>().material = chargedMaterial;
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1f);
        isShooting = false;
    }
}
