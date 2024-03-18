using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningScript : MonoBehaviour
{
    public GameObject obj;
    int randNum;
    public Transform spawnDest1, spawnDest2, spawnDest3, spawnDest4;
    public bool spawningBool = true;
    public float spawnTime;

    void Start() {
        StartCoroutine(spawning());
    }

    IEnumerator spawning() {
        while (spawningBool == true) {
            yield return new WaitForSeconds(spawnTime);
            // randNum = Random.Range(0, 4);
            // if (randNum == 0) {
            //     Instantiate(obj, spawnDest1.position, spawnDest1.rotation);
            // }
            // if (randNum == 1) {
            //     Instantiate(obj, spawnDest2.position, spawnDest2.rotation);
            // }
            // if (randNum == 2) {
            //     Instantiate(obj, spawnDest3.position, spawnDest3.rotation);
            // }
            // if (randNum == 3) {
            //     Instantiate(obj, spawnDest4.position, spawnDest4.rotation);
            // }

            // DEBUG: ALL SPAWNERS SPAWN AT SAME TIME
            // DEBUG: DELETE NEXT 4 LINES, UNCOMMENT 20-32 WHEN PUBLISHING

            Instantiate(obj, spawnDest1.position, spawnDest1.rotation);
            Instantiate(obj, spawnDest2.position, spawnDest2.rotation);
            Instantiate(obj, spawnDest3.position, spawnDest3.rotation);
            Instantiate(obj, spawnDest4.position, spawnDest4.rotation);
        }
    }

}