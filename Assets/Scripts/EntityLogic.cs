using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityLogic : MonoBehaviour
{
    public string name;
    public float health;
    public int entityType;

    public Entity entityData;

    //TEMPORARY
    public TextMeshProUGUI healthText;

    //Just a formality
    public enum EntityTypes
    {
        passive, // 0 - bascially destroyable gameobjects
        active // 1 - enemies
    };

    void Start()
    {
        name = entityData.name;
        health = entityData.health;
        entityType = entityData.entityType;
        healthText.text = health.ToString("F2");
    }

    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health < 0)
        {
            health = 0;
            Destroy(this.gameObject);
        }
        healthText.text = health.ToString("F2");
    }
}
