using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnter : MonoBehaviour
{
    private GameObject player;
    private NewPlayer players;
    private bool eneble;

    [Header("Horizontal")]
    [SerializeField] private float speed;

    [Header("Vertical")]
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float maxPosition;
    private float posiotionMax;
    private Vector2 direction;
    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    private void Start()
    {
        player = NewPlayer.Instance.gameObject;
        players = NewPlayer.Instance;
    }

    private void Movoment()
    {
        if (players.inCar)
        {
            if (Mathf.Abs(transform.position.y + verticalSpeed / 10 * direction.y) <= maxPosition)
                posiotionMax = 1;
            else
                posiotionMax = 0;

            transform.position = new Vector3(transform.position.x + speed / 10f, transform.position.y + verticalSpeed/10f * direction.y * posiotionMax);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Run")
        {
            player.SetActive(true);
            player.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        }
        else
        {
            touchStart = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Movoment();

        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            direction = Vector2.ClampMagnitude(offset, 1.0f);
        }
    }
   
}
