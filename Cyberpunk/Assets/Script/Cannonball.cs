using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float animTime;
    [SerializeField] private bool flipX;
    [SerializeField] private AudioClip boom;

    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        StartCoroutine(DestroyTime(16.5f));
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + speed/100, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 4)
        {
            StartCoroutine(Destroy());
        }
    }

    private IEnumerator Destroy()
    {
         audioSource.PlayOneShot(boom);
         animator.SetBool("Destroy", true);
         yield return new WaitForSeconds(animTime);
         Destroy(gameObject);
    }

    private IEnumerator DestroyTime(float time)
    {
        audioSource.PlayOneShot(boom);
        animator.SetBool("Destroy", true);
        yield return new WaitForSeconds(animTime);
        Destroy(gameObject);
    }
}
