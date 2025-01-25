using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public string name;
    public float damage;
    public float mass; //not sure if needed

    public Weapon weaponData;

    void Start()
    {
        name = weaponData.name;
        damage = weaponData.damage;
        mass = weaponData.mass;
    }


    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Entity" && (col.relativeVelocity.magnitude > 1f)) //If entity and the sword is swung fast enough
        {
            col.gameObject.GetComponent<EntityLogic>().TakeDamage(damage * (0.05f * col.relativeVelocity.magnitude)); //Every entity has entity logic
        }
    }

}
