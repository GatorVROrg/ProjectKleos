using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellNodeType 
{
    Root,
    Leaf, // Contains an actual spell
    Sequence, // Part of a spell casting animation/gesture
    Branch // Contains other nodes

}

public class SpellNode : MonoBehaviour
{
    public List<SpellNode> Children;
    public SpellNodeType Type;
    public GameObject Visual;
    public Collider Collider;
    public Spell spell;
}
