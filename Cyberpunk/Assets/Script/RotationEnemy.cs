using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEnemy : MonoBehaviour
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + speed);
    }
}
