using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    [SerializeField] private GameObject bomb;
    [SerializeField] private Transform positionCannon;
    [SerializeField] private float speedSpawnTime;
    [SerializeField] private float speedAnimTime;
    [SerializeField] private AudioClip shootAudio;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine (TimeShoot(speedSpawnTime));   
    }

    IEnumerator TimeShoot(float second)
    {
        while (true)
        {
            yield return new WaitForSeconds(second);
            audioSource.PlayOneShot(shootAudio);
            Instantiate(bomb, positionCannon);
        }
    }
}
