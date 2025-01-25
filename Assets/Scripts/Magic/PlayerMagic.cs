using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    public bool HasZeus = false;
    public bool HasPoseidon = false;
    public bool HasHades = false;

    public void ObtainPowersOfZeus()
    {
        HasZeus = true;
    }

    public void ObtainPowersOfPoseidon()
    {
        HasPoseidon = true;
    }

    public void ObtainPowersOfHades()
    {
        HasHades = true;
    }
}
