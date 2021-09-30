using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePatterns;

    [SerializeField] private float startTimeBetweenSpawn;
    [SerializeField] private float decreaseTime;
    [SerializeField] private float minTime = 0.65f;
    private float timeBetweenSpawn;
    private bool becameVisible;
    private NewPlayer player;

    private void Start()
    {
        player = NewPlayer.Instance;
    }

    private void FixedUpdate()
    {
        if (player.inCar == true && !becameVisible)
        {
            if (timeBetweenSpawn <= 0)
            {
                int rand = Random.Range(0, obstaclePatterns.Length);
                Instantiate(obstaclePatterns[rand], transform.position, Quaternion.identity);
                timeBetweenSpawn = startTimeBetweenSpawn;
                if (startTimeBetweenSpawn > minTime)
                {
                    startTimeBetweenSpawn -= decreaseTime;
                }
            }
            else
            {
                timeBetweenSpawn -= Time.deltaTime;
            }
        }
    }

    private void OnBecameVisible()
    {
        becameVisible = enabled;
    }
}
