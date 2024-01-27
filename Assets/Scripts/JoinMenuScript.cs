using UnityEngine;

public class JoinMenuScript : MonoBehaviour
{
    public Keyboard keyboard;

    public void clickJoinButton()
    {
        keyboard.CloseKeyboard();
        gameObject.SetActive(false);
    }

    public void clickHostButton()
    {
        gameObject.SetActive(false);
    }
}