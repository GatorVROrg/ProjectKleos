using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollectable : MonoBehaviour
{
    // will be updated once the item is selected.
    private int value = 0;
    public ResourceHandler rsh;

    private void SetValue() {
        value = UnityEngine.Random.Range(10, 20);
        Debug.Log(value);
    }

    public void OnSelect() {
        SetValue();
        rsh.IncrementGold(value);
        Destroy(gameObject);
    }

}
