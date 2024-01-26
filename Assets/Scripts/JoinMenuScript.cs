using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JoinMenuScript : MonoBehaviour
{
    public RelayStuff RS;
    public Keyboard keyboard;

    public void Start()
    {
        RS = GameObject.Find("Relay").GetComponent<RelayStuff>();
    }

    public void clickJoinButton()
    {
        keyboard.CloseKeyboard();
        gameObject.SetActive(false);
        RS.JoinGame();
    }

    public void clickHostButton()
    {
        gameObject.SetActive(false);
        RS.CreateGame();
    }
}