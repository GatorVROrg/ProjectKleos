using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpellCasting : MonoBehaviour
{
    public InputActionProperty CastingButton; // Begins and stops spell selection
    public InputActionProperty DismissSpellButton; // Dismisses the active spell


    public GameObject RootNode; // Root node of the spell tree. It's children are the first layer of spell selection.
    public GameObject CastingVisual; // This is what interacts with the nodes to select spells.

    private GameObject _currentNode;
    public Spell _activeSpell; //TODO: private var. temp public for testing

    [Header("Transforms")]
    public Transform SpellSpawnPoint;
    public float CastingRadius = 0.5f;

    private Vector3 _initialCastingPosition;
    private Quaternion _initialCastingOrientation;
    private DateTime _lastCastTime;

    void Start()
    {
        _lastCastTime = DateTime.UtcNow;
    }

    void Update()
    {
        if (CastingButton.action.WasPressedThisFrame())
        {
            if (_activeSpell != null) 
                _activeSpell.TriggerPressCallback(this);
            else
                StartCasting();
        }
        else if (CastingButton.action.WasReleasedThisFrame())
        {
            if (_activeSpell != null)
                _activeSpell.TriggerReleaseCallback(this);
            else
                StopCasting();
        }
        else if (DismissSpellButton.action.WasPressedThisFrame())
        {
            _activeSpell = null;
        }
    }

    private void StartCasting()
    {
        Debug.Log("Started Casting");

        CastingVisual.SetActive(true);
        _initialCastingOrientation = CastingVisual.transform.rotation;
        _initialCastingPosition = CastingVisual.transform.position;
        ActivateBranch(RootNode);
    }

    private void StopCasting()
    {
        // Ends casting when no spell is on the wand

        Debug.Log("Stopped Casting");

        CastingVisual.SetActive(false);
        if (_currentNode != null)
        {
            for (int i = 0; i < _currentNode.transform.childCount; i++)
            {
                // TODO: inefficient, update later
                if (_currentNode.transform.GetChild(i).GetComponent<SpellNode>() == null) continue;
                DeactivateNode(_currentNode.transform.GetChild(i).gameObject);
            }
        }
    }

    private void ActivateBranch(GameObject node) 
    {
        //Activates and positions all children of a branch node

        // Deactivate all current nodes
        if (_currentNode != null)
        {
            for (int i = 0; i < _currentNode.transform.childCount; i++)
            {
                // TODO: inefficient, update later
                if (_currentNode.transform.GetChild(i).GetComponent<SpellNode>() == null) continue;
                DeactivateNode(_currentNode.transform.GetChild(i).gameObject);
            }
        }

        _currentNode = node;
        int childCount = _currentNode.transform.childCount;
        float angleStep = 360f / childCount; // Divide the circle into equal parts based on the number of children

        // Determine the rotation to align the circle's normal with the CastingVisual's up vector
        Quaternion rotationToMatchCastingVisual = Quaternion.FromToRotation(Vector3.forward, _initialCastingOrientation * Vector3.up);

        for (int i = 0; i < childCount; i++)
        {
            GameObject child = _currentNode.transform.GetChild(i).gameObject;
            SpellNode spellNode = child.GetComponent<SpellNode>();
            if (spellNode == null) continue;
            ActivateNode(child);

            // Calculate position in a circle on the XY plane
            // TODO: use children list later
            float angle = angleStep * i * Mathf.Deg2Rad; // Convert angle to radians
            Vector3 localPos = new Vector3(Mathf.Cos(angle) * CastingRadius, Mathf.Sin(angle) * CastingRadius, 0);

            // Rotate the position to align with the CastingVisual's orientation
            Vector3 worldPos = rotationToMatchCastingVisual * localPos;

            // Position the child object relative to the CastingVisual, adjusted for rotation
            child.transform.position = worldPos + _initialCastingPosition;

            // Adjust the child's rotation to face outwards from the circle's center
            Vector3 direction = (child.transform.position - _initialCastingPosition).normalized;
            // Ensure the up vector of the child matches that of the CastingVisual for consistent orientation
            child.transform.rotation = Quaternion.LookRotation(direction, _initialCastingOrientation * Vector3.right);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Is responsible for navigating down the spell tree when a node is selected
        Debug.Log("Collision detected");

        SpellNode spellNode = other.gameObject.GetComponent<SpellNode>();
        if (spellNode == null) return;

        if ((float)(DateTime.UtcNow - _lastCastTime).TotalSeconds < 0.1f) return; //enure no accidental double tap
        _lastCastTime = DateTime.UtcNow;

        if (spellNode.Type == SpellNodeType.Branch)
        {
            ActivateBranch(other.gameObject);
        }
        else if (spellNode.Type == SpellNodeType.Leaf)
        {
            _activeSpell = other.gameObject.GetComponent<Spell>();
            StopCasting();
        }
    }

    // TODO: below
    // Temp inefficient methods for activation and deactivation. A better method would be to have a reference to
    // the colliders and bodies in the spellNode script and manually set references instead of searching for them each time.
    private void DeactivateNode(GameObject node)
    {
        node.GetComponent<Collider>().enabled = false;
        node.GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    private void ActivateNode(GameObject node)
    {
        node.GetComponent<Collider>().enabled = true;
        node.GetComponentInChildren<MeshRenderer>().enabled = true;
    }

}
