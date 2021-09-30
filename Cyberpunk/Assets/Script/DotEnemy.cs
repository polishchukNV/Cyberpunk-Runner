using UnityEngine;

public class DotEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool OnRight;
    [SerializeField] private Transform[] Point = new Transform[2];
    [SerializeField] private AudioClip stepSound;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        FlipX();
    }

    private void FlipX()
    {
        PlayStepSound();
        spriteRenderer.flipX = OnRight;
        if (gameObject.transform.position.x < Point[0].position.x)
        {
            OnRight = true;
        }
        else if ((gameObject.transform.position.x > Point[1].position.x))
        {
            OnRight = false;
        }

        if (OnRight)
        {
            rigidbody.velocity = new Vector2(speed, rigidbody.velocity.y);
        }
        else
        {
            rigidbody.velocity = new Vector2(-speed, rigidbody.velocity.y);
        }
    }

    private void PlayStepSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.pitch = (UnityEngine.Random.Range(0.6f,0.8f));
            audioSource.PlayOneShot(stepSound);
        }
    }

}
       
