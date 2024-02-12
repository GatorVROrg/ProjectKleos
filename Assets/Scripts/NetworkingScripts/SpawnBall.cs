using Unity.Netcode;
using UnityEngine;

public class SpawnBall : NetworkBehaviour
{
    public GameObject prefab;

    void Start()
    {
        if (IsServer || IsHost)
        {
            Debug.Log("Spawnin");
            GameObject go = Instantiate(prefab, gameObject.transform.position, Quaternion.identity);                
            go.GetComponent<NetworkObject>().Spawn();
        }
    }
}