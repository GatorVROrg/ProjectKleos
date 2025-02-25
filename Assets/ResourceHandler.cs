using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ResourceHandler : MonoBehaviour
{

    public TextMeshProUGUI moneyHUD;
    private int gold = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHUDAmt() {
        moneyHUD.text = "Money: " + gold;
    }

    public void IncrementGold(int income) {
        gold = gold + income;
        UpdateHUDAmt();
    }

    public void DecrementGold(int cost) {
        gold = gold - cost;
        UpdateHUDAmt();
    }
}
