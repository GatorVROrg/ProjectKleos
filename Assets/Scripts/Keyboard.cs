using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public GameObject keyBoard;
    private string inputString;
    private bool isCapsLockOn = false;
    private bool KeyboardOpen = false;
    private TMP_InputField inputField;
    
    void Update()
    {
        if(KeyboardOpen)
        {
            inputField.text = inputString;
        }
    }

    public void OpenKeyboard(TMP_InputField InputField)
    {
        KeyboardOpen = true;
        inputField = InputField;
        keyBoard.SetActive(true);
    }
    
    public void CloseKeyboard()
    {
        KeyboardOpen = false;
        inputString = "";
        keyBoard.SetActive(false);
    }

    public void EnterKeyCapsLock()
    {
        isCapsLockOn = !isCapsLockOn;
    }
    
    public void EnterKeyBackspace()
    {
        if (inputString.Length > 0)
        {
            inputString = inputString.Substring(0, inputString.Length - 1);
        }
    }

    public void EnterKeySpace()
    {
        inputString += " ";
    }

    private string GetLetter(char letter)
    {
        if (isCapsLockOn)
        {
            isCapsLockOn = false;
            return char.ToUpper(letter).ToString();
        }
        else
        {
            isCapsLockOn = false;
            return char.ToLower(letter).ToString();
        }
    }

    public void EnterKeyA()
    {
        Debug.Log("Pressed");
        inputString += GetLetter('a');
    }

    public void EnterKeyB()
    {
        inputString += GetLetter('b');
    }

    public void EnterKeyC()
    {
        inputString += GetLetter('c');
    }

    public void EnterKeyD()
    {
        inputString += GetLetter('d');
    }

    public void EnterKeyE()
    {
        inputString += GetLetter('e');
    }

    public void EnterKeyF()
    {
        inputString += GetLetter('f');
    }

    public void EnterKeyG()
    {
        inputString += GetLetter('g');
    }

    public void EnterKeyH()
    {
        inputString += GetLetter('h');
    }

    public void EnterKeyI()
    {
        inputString += GetLetter('i');
    }

    public void EnterKeyJ()
    {
        inputString += GetLetter('j');
    }

    public void EnterKeyK()
    {
        inputString += GetLetter('k');
    }

    public void EnterKeyL()
    {
        inputString += GetLetter('l');
    }

    public void EnterKeyM()
    {
        inputString += GetLetter('m');
    }

    public void EnterKeyN()
    {
        inputString += GetLetter('n');
    }

    public void EnterKeyO()
    {
        inputString += GetLetter('o');
    }

    public void EnterKeyP()
    {
        inputString += GetLetter('p');
    }

    public void EnterKeyQ()
    {
        inputString += GetLetter('q');
    }

    public void EnterKeyR()
    {
        inputString += GetLetter('r');
    }

    public void EnterKeyS()
    {
        inputString += GetLetter('s');
    }

    public void EnterKeyT()
    {
        inputString += GetLetter('t');
    }

    public void EnterKeyU()
    {
        inputString += GetLetter('u');
    }

    public void EnterKeyV()
    {
        inputString += GetLetter('v');
    }

    public void EnterKeyW()
    {
        inputString += GetLetter('w');
    }

    public void EnterKeyX()
    {
        inputString += GetLetter('x');
    }

    public void EnterKeyY()
    {
        inputString += GetLetter('y');
    }

    public void EnterKeyZ()
    {
        inputString += GetLetter('z');
    }

    public void EnterKey0()
    {
        inputString += GetLetter('0');
    }

    public void EnterKey1()
    {
        inputString += GetLetter('1');
    }

    public void EnterKey2()
    {
        inputString += GetLetter('2');
    }

    public void EnterKey3()
    {
        inputString += GetLetter('3');
    }

    public void EnterKey4()
    {
        inputString += GetLetter('4');
    }

    public void EnterKey5()
    {
        inputString += GetLetter('5');
    }

    public void EnterKey6()
    {
        inputString += GetLetter('6');
    }

    public void EnterKey7()
    {
        inputString += GetLetter('7');
    }

    public void EnterKey8()
    {
        inputString += GetLetter('8');
    }

    public void EnterKey9()
    {
        inputString += GetLetter('9');
    }
}
