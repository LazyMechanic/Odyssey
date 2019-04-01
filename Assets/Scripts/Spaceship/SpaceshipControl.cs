using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpaceshipControl : MonoBehaviour
{
    public Rigidbody rigidbody;

    public float speed = 100.0f;
    [SerializeField]
    private bool canJump = false;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(rigidbody, "[SpaceshipControl]: Spaceship rigidbody is null");

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = rigidbody.transform.forward * speed;
    }
}
