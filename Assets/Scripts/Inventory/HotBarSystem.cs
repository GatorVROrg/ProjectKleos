using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarSystem : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject LeftHeldItem = null;
    public GameObject RightHand;
    public GameObject RightHeldItem = null;

    public InputActionProperty primaryButtonRightAction;
    public InputActionProperty primaryButtonLeftAction;
    public InputActionProperty RightGrip;
    public InputActionProperty LeftGrip;
    private float RightGripping;
    private float LeftGripping;
    private bool RAButton;
    private bool LXButton;

    private List<GameObject> FullHotBar = new(6);
    public List<GameObject> LeftHotBarItems = new(3);
    public List<GameObject> RightHotBarItems = new(3);

    void Update()
    {
        //read grip input
        RightGripping = RightGrip.action.ReadValue<float>();
        LeftGripping = LeftGrip.action.ReadValue<float>();
        
        //get primary button inputs
        if (primaryButtonRightAction.action.ReadValue<float>() == 1)
            RAButton = true;
        else
            RAButton = false;

        if (primaryButtonLeftAction.action.ReadValue<float>() == 1)
            LXButton = true;
        else
            LXButton = false;

        //open hotbars
        if(LeftGripping > 0 && LXButton)
        {
            OpenLeftHotBar();
        }
        else
        {
            if(LeftHand.transform.childCount > 0)
                Destroy(LeftHand.transform.GetChild(0).gameObject);
        }
        if(RightGripping > 0 && RAButton)
        {
            OpenRightHotBar();
        }
        else
        {
            if(RightHand.transform.childCount > 0)
                Destroy(RightHand.transform.GetChild(0).gameObject);
        }

        if(LeftHeldItem != null)
        {
            SetLeftHeldItem();
        }

        if(RightHeldItem != null)
        {
            SetRightHeldItem();
        }
    }

    public void UpdateHotBars()
    {
        // Get the first 3 items from the inventory and assign them to the left hotbar
        for (int i = 0; i < 3; i++)
        {
            LeftHotBarItems[i] = FullHotBar[i];
        }

        // Assign the last 3 items from the full hotbar to the right hotbar
        for (int i = FullHotBar.Count - 3, j = 0; i < FullHotBar.Count; i++, j++)
        {
            RightHotBarItems[j] = FullHotBar[i];
        }
    }

    public void OpenLeftHotBar()
    {
        if(!GameObject.Find("LeftHotBar"))
            Instantiate(Resources.Load<GameObject>("Inventory/LeftHotBar"), LeftHand.GetNamedChild("InventorySpawn").transform.position, LeftHand.GetNamedChild("InventorySpawn").transform.rotation);
    }
    public void OpenRightHotBar()
    {
        if(!GameObject.Find("RightHotBar"))
            Instantiate(Resources.Load<GameObject>("Inventory/RightHotBar"), RightHand.GetNamedChild("InventorySpawn").transform.position, RightHand.GetNamedChild("InventorySpawn").transform.rotation);
    }

    public void SetLeftHeldItem()
    {
        if(LeftHand.GetNamedChild("LeftHeldItem").transform.childCount == 0)
        {
            Instantiate(LeftHeldItem, LeftHand.GetNamedChild("LeftHeldItem").transform.position, LeftHand.GetNamedChild("LeftHeldItem").transform.rotation).transform.parent = LeftHand.GetNamedChild("LeftHeldItem").transform;
        }
    } 

    public void SetRightHeldItem()
    {
        if(RightHand.GetNamedChild("RightHeldItem").transform.childCount == 0)
        {
            Instantiate(RightHeldItem, RightHand.GetNamedChild("RightHeldItem").transform.position, RightHand.GetNamedChild("RightHeldItem").transform.rotation).transform.parent = RightHand.GetNamedChild("RightHeldItem").transform;
        }
    }
}