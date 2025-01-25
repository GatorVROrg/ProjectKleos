using UnityEngine;

public class LeftHotBar : MonoBehaviour
{
    private HotBarSystem HBS;
    public Transform[] spots;

    void Start() 
    {
        HBS = GameObject.Find("Player").GetComponent<HotBarSystem>();
        int Index = 0;
        foreach(GameObject item in HBS.LeftHotBarItems)
        {
            Instantiate(item, spots[Index].position, item.transform.rotation);
            Index++;
        }
    }

    void Update()
    {
        foreach (Transform spot in spots)
        {
            float distance = Vector3.Distance(HBS.LeftHand.transform.position, spot.position);
            if (distance < 1)
            {
                HBS.LeftHeldItem = spot.GetChild(0).gameObject;
                Destroy(this);
            }
            else
            {
                HBS.LeftHeldItem = null;
            }
        }
    }
}
