using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void TriggerPressCallback(SpellCasting caster) {

    }

    public virtual void TriggerReleaseCallback(SpellCasting caster) {

    }
}