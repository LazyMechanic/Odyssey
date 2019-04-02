using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SpaceshipControl : MonoBehaviour
{
    public Rigidbody rigidbody;

    public float maxSpeed = 5.0f;
    public float acceleration = 0.00001f;
    [SerializeField]
    private bool canJump = false;
    private Vector3 originPosition;

    // Start is called before the first frame update
    void Start()
    {
        Assert.IsNotNull(rigidbody, "[SpaceshipControl]: Spaceship rigidbody is null");
        originPosition = rigidbody.position;

    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity + transform.forward * acceleration, maxSpeed);
    }
}
