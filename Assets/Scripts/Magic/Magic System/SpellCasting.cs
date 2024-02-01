using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting : MonoBehaviour
{
    public InputActionProperty CastingButton;

    public GameObject RootNode; // Root node of the spell tree. It's children are the first layer of spell selection.
    public GameObject CastingVisual; // This is what interacts with the nodes to select spells.

    private GameObject _currentNode;
    private Spell _activeSpell;

    [Header("Transforms")]
    public Transform SpellSpawnPoint;
    public float CastingRadius = 0.5f;


    void Update()
    {
        if (CastingButton.action.WasPressedThisFrame())
        {
            if (_activeSpell != null) 
                _activeSpell.TriggerPressCallback();
            else
                StartCasting();
        }
        else if (CastingButton.action.WasReleasedThisFrame())
        {
            if (_activeSpell != null)
                _activeSpell.TriggerReleaseCallback();
            else
                StopCasting();
        }
    }

    private void StartCasting()
    {
        // Begins the process of selecting spells
        Debug.Log("Started Casting");

        _currentNode = RootNode;
        CastingVisual.SetActive(true);

        int childCount = _currentNode.transform.childCount;
        float angleStep = 360f / childCount; // Divide the circle into equal parts based on the number of children

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = _currentNode.transform.GetChild(i).gameObject;
            child.SetActive(true);

            // Calculate position in a circle
            float angle = angleStep * i * Mathf.Deg2Rad; // Convert angle to radians
            float x = Mathf.Cos(angle) * CastingRadius;
            float y = Mathf.Sin(angle) * CastingRadius;

            // Position the child object
            child.transform.position = new Vector3(x, y, 0) + CastingVisual.transform.position;

            Vector3 direction = (child.transform.position - CastingVisual.transform.position).normalized;
            child.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }


    private void StopCasting()
    {
        // Ends casting when no spell is on the wand

        Debug.Log("Stopped Casting");

        CastingVisual.SetActive(false);
        for (int i = 0; i < _currentNode.transform.childCount; i++)
        {
            _currentNode.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
