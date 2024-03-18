using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateMoney : MonoBehaviour
{

    int amtMoney = 0;
    string message = "Money: ";
    public TextMeshProUGUI hud;
    private Collider playerCollider;
    

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (hit.gameObject.tag == "Resource") {
            Debug.Log(playerCollider.name);
            Destroy(hit.gameObject);
            amtMoney++;
            hud.SetText(message + amtMoney);
        }
    }
}
