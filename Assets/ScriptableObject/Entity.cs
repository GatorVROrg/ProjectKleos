using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Scriptable Objects/Entity")]
public class Entity : ScriptableObject
{
    public string name;
    public float health;
    public int entityType;
}
