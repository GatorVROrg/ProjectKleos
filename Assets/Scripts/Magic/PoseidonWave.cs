using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PoseidonWave : MonoBehaviour
{
    public Transform Player;

    public InputActionProperty LeftTrigger;

    private PlayerMagic PM;
    private bool onCooldown = false;

    public GameObject wavePrefab;
    public float waveSpeed = 10f;
    public float waveDuration = 5f; 

    void Start()
    {
        PM = gameObject.GetComponent<PlayerMagic>();
    }

    void Update()
    {
        if(PM.HasPoseidon)
        {
            if(LeftTrigger.action.ReadValue<float>() == 1 && !onCooldown)
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        if (Player != null)
        {
            Vector3 spawnPosition = Player.transform.position + Player.transform.forward; // Slightly in front of the player
            GameObject waveInstance = Instantiate(wavePrefab, spawnPosition, Player.transform.Find("Model").rotation);
            
            // Start moving the wave forward
            waveInstance.GetComponent<Rigidbody>().velocity = Player.transform.Find("Model").forward * waveSpeed;
            
            // Destroy wave after its duration ends
            Destroy(waveInstance, waveDuration);
            Reload();
        }
    }

    IEnumerator Reload()
    {
        onCooldown = true;
        yield return new WaitForSeconds(1f);
        onCooldown = false;
    }
}
