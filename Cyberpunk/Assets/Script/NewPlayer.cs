using System.Collections;
using UnityEngine;
using System;

public class NewPlayer : MonoBehaviour
{
    
    [Header("Horizontal Movement")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 direction;
    [SerializeField] private float maxSpeed = 7f;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpSpeed = 15f;
    [SerializeField] private float jumpDelay = 0.25f;
    private float jumpTimer;

    [Header("Components")]
    [SerializeField] private LayerMask groundLayer;
    private GameObject characterHolder;

    private Rigidbody2D rigibody;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Collision")]
    [SerializeField] private bool onGround;
    [SerializeField] private float groundLength;
    [SerializeField] private Vector3 colliderOffset;
    [SerializeField] private GameObject colliderRun;
    [SerializeField] private ParticleSystem dustJump;
    [SerializeField] private ParticleSystem dustGravity;
    

    [Header("Audio")]
    [SerializeField] private AudioClip stepSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;

    [Space]
    [Header("GameMode")]
    [SerializeField] private GameMode GameMod;
    private enum GameMode { Run, RunDown, GravityZone, Car }
    public bool inCar;
    private bool facingRightY;

    private static NewPlayer instance;
	public static NewPlayer Instance
	{
		get
		{
			if (instance == null) instance = GameObject.FindObjectOfType<NewPlayer>();
			return instance;
		}
	}

    private void Start() => characterHolder = gameObject;

    private void Awake()
    {
        rigibody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        bool wasOnGround = onGround;
        onGround = (Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer)) || (Physics2D.Raycast(transform.position + colliderOffset, Vector2.up, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.up, groundLength, groundLayer));
        
        if (Input.GetMouseButtonDown(0))
        {
            ActiveMode();
        }

        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.1f, 0.75f, 0.15f));
        }

        animator.SetBool("onGround", onGround);
        direction = new Vector2(1, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        moveCharacter(direction.x);
    }

    private void moveCharacter(float horizontal)
    {
        if (Mathf.Abs(rigibody.velocity.x) < maxSpeed)
        {
            rigibody.velocity = new Vector2(Mathf.Sign(rigibody.velocity.x) * maxSpeed, rigibody.velocity.y);
        }
        if(Math.Abs(rigibody.velocity.x) > 1 && onGround) PlayStepSound();

        animator.SetFloat("vertical", rigibody.velocity.y);
    }

    private void Jump()
    {
        rigibody.velocity = new Vector2(rigibody.velocity.x, 0);
        rigibody.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        jumpTimer = 0;
        animator.SetBool("Crouch", false);
        CreateDast(dustJump);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Винисти окремо
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            Application.LoadLevel(Application.loadedLevel);
        }

        if (collision.gameObject.tag == "GravityZone")
        {
            GameMod = GameMode.GravityZone;
        }

        if (collision.gameObject.tag == "Run")
        {
            GameMod = GameMode.Run;
            rigibody.gravityScale = Mathf.Abs(rigibody.gravityScale);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            spriteRenderer.flipY = false;
        }

        if (collision.gameObject.tag == "Car")
        {
            inCar = true;
            gameObject.SetActive(false);
        }
    }

    private void GravityZone()
    {
        rigibody.gravityScale = -rigibody.gravityScale;
        CreateDast(dustGravity);
        facingRightY = !facingRightY;
        spriteRenderer.flipY = true;
        transform.rotation = Quaternion.Euler(facingRightY ? 0 : 180, 0 , 0);
    }

    private void ActiveMode()
    {

        switch (GameMod)
        {
            case GameMode.Run:

                jumpTimer = Time.time + jumpDelay;
                if (jumpTimer > Time.time && onGround)
                {
                    Jump();
                    PlayJumpSound();
                }
             break;

            case GameMode.GravityZone:

                GravityZone();

            break;

            case GameMode.RunDown:

            break;

        }
    }

    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    private void CreateDast(ParticleSystem particle)
    {
        particle.Play();
    }

    private void PlayStepSound()
    {
        audioSource.pitch = (UnityEngine.Random.Range(0.6f, 1f));
        if(!audioSource.isPlaying)audioSource.PlayOneShot(stepSound);
    }

    private void PlayJumpSound()
    {
        audioSource.pitch = (UnityEngine.Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(jumpSound);
    }
    private void PlayAttackSound()
    {
        audioSource.pitch = (UnityEngine.Random.Range(0.6f, 1f));
        audioSource.PlayOneShot(attackSound);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }

    

}
