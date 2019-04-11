using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Spaceship : MonoBehaviour
{
    // Spaceship rigidbody
    private Rigidbody _rigidbody = null;

    // Spaceship spawn point
    public Transform spaceshipSpawn = null;

    void Awake()
    {
        // Check rigidbody
        if (_rigidbody == null)
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        Assert.IsNotNull(_rigidbody, "[Spaceship]: Spaceship rigidbody is null");

        // Check spaceship spawn position
        Assert.IsNotNull(spaceshipSpawn, "[Spaceship]: Spaceship spawn is null");
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody.transform.position = spaceshipSpawn.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
