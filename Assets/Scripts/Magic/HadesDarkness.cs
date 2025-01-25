using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HadesDarkness : MonoBehaviour
{
    public SkinnedMeshRenderer LHand;
    public SkinnedMeshRenderer RHand;
    public MeshRenderer Head;

    public InputActionProperty LeftTrigger;
    public InputActionProperty RightTrigger;

    private PlayerMagic PM;
    private bool onCooldown = false;

    public Material Invisible;
    private Dictionary<string, Material> defaultMaterial;
    public float duration;

    void Start()
    {
        PM = gameObject.GetComponent<PlayerMagic>();
    }

    void Update()
    {
        if(PM.HasPoseidon)
        {
            if(LeftTrigger.action.ReadValue<float>() == 1 && RightTrigger.action.ReadValue<float>() == 1&& !onCooldown)
            {
                StartCoroutine(TurnInvisible());
            }
        }
    }

    public IEnumerator TurnInvisible()
    {
        //logic for enemies to stop caring

        defaultMaterial.Add(RHand.name, RHand.material);
        defaultMaterial.Add(LHand.name, LHand.material);
        defaultMaterial.Add(Head.name, Head.material);


        RHand.material = Invisible;
        LHand.material = Invisible;
        Head.material = Invisible;
        
        yield return new WaitForSeconds(duration);

        RHand.material = defaultMaterial[RHand.name];
        LHand.material = defaultMaterial[LHand.name];
        Head.material = defaultMaterial[Head.name];
        //enemies see player again

        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        onCooldown = true;
        yield return new WaitForSeconds(1f);
        onCooldown = false;
    }
}
