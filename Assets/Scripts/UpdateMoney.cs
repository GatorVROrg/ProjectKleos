using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateMoney : MonoBehaviour
{

    int amtMoney = 0;
    string message = "Money: ";
    public TextMeshProUGUI hud;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Resource") {
            Destroy(collision.gameObject);
            amtMoney++;
            hud.SetText(message + amtMoney);
        }
    }
}
