using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSpell2 : Spell
{
    public ExampleSpell1Behavior spellPrefab;

    private ExampleSpell1Behavior _currentSpell;
    private Coroutine _growSpellCoroutine;
    private SpellCasting _caster;


    public override void TriggerPressCallback(SpellCasting caster)
    {
        _caster = caster;
        if (_currentSpell == null)
        {
            _currentSpell = Instantiate(spellPrefab, caster.transform.position, caster.transform.rotation);
            _growSpellCoroutine = StartCoroutine(GrowSpell(_currentSpell.transform));
        }

    }

    public override void TriggerReleaseCallback(SpellCasting caster)
    {
        if (_currentSpell != null)
        {
            _currentSpell.Initialize(caster.SpellSpawnPoint);
            _currentSpell = null;
            StopCoroutine(_growSpellCoroutine);
        }
    }

    IEnumerator GrowSpell(Transform spellTransform)
    {
        Vector3 initialScale = spellTransform.localScale;
        Vector3 targetScale = spellTransform.localScale + new Vector3(20f, 20f, 20f);
        float duration = 100f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            _currentSpell.transform.position = _caster.SpellSpawnPoint.position;
            _currentSpell.Speed += Time.deltaTime * 1.5f;
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            spellTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
    }
}
