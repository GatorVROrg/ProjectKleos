using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public GameObject itemInSlot;
    public Image slotImage;
    Color originalColor;

    float isGrippingRightHand;
    float isGrippingLeftHand;
    public InputActionProperty RightGrip;
    public InputActionProperty LeftGrip;

    void Start()
    {
        slotImage = GetComponentInChildren<Image>();
        originalColor = slotImage.color;
    }

    private void Update()
    {
        isGrippingRightHand = RightGrip.action.ReadValue<float>();
        isGrippingLeftHand = LeftGrip.action.ReadValue<float>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (itemInSlot != null) return;
        GameObject obj = other.gameObject;
        if (!IsItem(obj)) return;
        if (isGrippingRightHand == 0)
        {
            InsertItem(obj);
        }
         
    }

    bool IsItem(GameObject obj)
    {
        return obj.GetComponent<InventoryItem>();
    }

    void InsertItem(GameObject obj)
    {
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.SetParent(gameObject.transform, true);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localEulerAngles = obj.GetComponent<InventoryItem>().slotRotation;
        obj.GetComponent<InventoryItem>().inSlot = true;
        obj.GetComponent<InventoryItem>().currentSlot = this;
        itemInSlot = obj;
        slotImage.color = Color.gray;
    }

    public void ResetColor()
    {
        slotImage.color = originalColor;
    }

    public void RemoveItem()
    {
        if (isGrippingRightHand == 1)
        {
            if (gameObject.GetComponent<InventoryItem>() == null) return;
            
            if (gameObject.GetComponent<InventoryItem>().inSlot)
            {
                gameObject.GetComponentInParent<InventorySlot>().itemInSlot = null;
                gameObject.transform.parent = null;
                gameObject.GetComponent<InventoryItem>().inSlot = false;
                gameObject.GetComponent<InventoryItem>().currentSlot.ResetColor();
                gameObject.GetComponent<InventoryItem>().currentSlot = null;

            }
        }
    }
}
