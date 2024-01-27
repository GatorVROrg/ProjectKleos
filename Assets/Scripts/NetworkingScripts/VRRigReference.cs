using UnityEngine;

public class VRRigReference : MonoBehaviour
{
    public static VRRigReference Singleton;

    public Transform root;
    public Transform head;
    public Transform lefthand;
    public Transform righthand;

    private void Awake()
    {
        Singleton = this;
    }
}
