using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSpell1 : Spell
{
    public ExampleSpell1Behavior spellPrefab;

    public override void TriggerPressCallback(SpellCasting caster)
    {
        ExampleSpell1Behavior spell = Instantiate(spellPrefab, caster.transform.position, caster.transform.rotation);
        spell.Initialize(caster.SpellSpawnPoint);
    }

    public override void TriggerReleaseCallback(SpellCasting caster)
    {

    }
}
