using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleSpell1Behavior : MonoBehaviour
{
    public Vector3 MovingAxis;
    public float Speed = 5.0f;
    private Vector3 _movingWorldAxis;


    public void Initialize(Transform spellSpawnPoint)
    {
        _movingWorldAxis = spellSpawnPoint.TransformDirection(MovingAxis);
        Destroy(gameObject, 10.0f);
    }


    void Update()
    {
        transform.position += Time.deltaTime * Speed * _movingWorldAxis;
    }
}
