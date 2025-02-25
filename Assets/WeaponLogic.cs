using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLogic : MonoBehaviour
{
    public string name;
    public float damage;
    public float mass; //not sure if needed
    public Rigidbody weaponRB;

    public Weapon weaponData;

    //Audio
    public AudioSource audioSource;
    public AudioClip swordHitFlesh;
    public AudioClip swordHitDefault;

    void Start()
    {
        name = weaponData.name;
        damage = weaponData.damage;
        mass = weaponData.mass;
        audioSource = this.GetComponent<AudioSource>();
        weaponRB = this.GetComponent<Rigidbody>();
        weaponRB.mass = mass;
    }


    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        //Audio Volume
        float hitVol = col.relativeVelocity.magnitude / 50f;
        if (hitVol > 1f)
        {
            hitVol = 1f;
        }

        //Entity Check
        if (col.gameObject.tag == "Entity" && (col.relativeVelocity.magnitude > 1f)) //If entity and the sword is swung fast enough
        {
            col.gameObject.GetComponent<EntityLogic>().TakeDamage(damage * (0.05f * col.relativeVelocity.magnitude)); //Every entity has entity logic
            audioSource.PlayOneShot(swordHitFlesh, hitVol);
        }
        else if (col.relativeVelocity.magnitude > 1f)
        {
            audioSource.PlayOneShot(swordHitDefault, hitVol);
        }
    }

}
