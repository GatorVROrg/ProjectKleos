using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkPlayer : MonoBehaviour
{

    // These are on the network player prefab
    public Transform root;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;
    private PhotonView photonView;

    void Start() 
    {
        photonView = GetComponent<PhotonView>();

        if(photonView.IsMine)
        {
            // we set all the network objects false
            righthand.gameObject.GetComponent<Renderer>().enabled = false;
            lefthand.gameObject.GetComponent<Renderer>().enabled = false;
            head.gameObject.GetComponent<Renderer>().enabled = false;   
        }
    }

    void Update()
    {       
        if(photonView.IsMine)
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