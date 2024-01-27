using UnityEngine;
using Unity.Netcode;

public class NetworkPlayer : NetworkBehaviour
{
    public Transform root;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;
    public Renderer[] meshToDisable;

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            foreach (var item in meshToDisable)
            {
                item.enabled = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(IsOwner)
        {   
            root.position = VRRigReference.Singleton.root.position;
            root.rotation = VRRigReference.Singleton.root.rotation;

            head.position = VRRigReference.Singleton.head.position;
            head.rotation = VRRigReference.Singleton.head.rotation;

            lefthand.position = VRRigReference.Singleton.lefthand.position;
            lefthand.rotation = VRRigReference.Singleton.lefthand.rotation;

            righthand.position = VRRigReference.Singleton.righthand.position;
            righthand.rotation = VRRigReference.Singleton.righthand.rotation;
        }
    }
}