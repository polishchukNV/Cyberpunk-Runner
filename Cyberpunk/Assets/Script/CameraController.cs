using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 position;
    private NewPlayer player;
    private float speedController;

    private void Start()
    {
        player = NewPlayer.Instance;
    }

    private void FixedUpdate()
    {
        if (transform.position.x - 2.4f <= player.transform.position.x)
            speedController = 1.6f;
        else
            speedController = 1f;

        position = new Vector3(transform.position.x + speed * speedController, 0, -10);
        position.y = 0;
        position.z = -10f;
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * 8);
    }

}
