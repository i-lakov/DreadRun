using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPhysics : MonoBehaviour
{
    #region Members
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private BoxCollider2D bc;

    public ParticleSystem dust;
    public Bullets bulletPrefab;
    public Transform launchOffset;
    public Ammo ammoInstance;

    public GameObject gun;
    private SpriteRenderer gunSR;
    private Transform gunTransform;

    private float dirX = 0;
    private bool hasPerformedDJump = false;
    private float yKnockbackModifier = 3f;
    private Vector3 gunTransformL = new Vector3(-0.35f, 0.04f, 0f);
    private Vector3 gunTransformR = new Vector3(0.35f, 0.04f, 0f);

    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float knockback;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource shotSound;

    private enum PlayerState
    {
        idle,
        running,
        jumping,
        falling
    }
    #endregion

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();

        gunSR = gun.GetComponent<SpriteRenderer>();
        gunTransform = gun.GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveSpeed * dirX * Time.deltaTime, 0, 0);
    }

    void Update()
    {
        CheckForWinCondition();

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
            hasPerformedDJump = false;
        }
        else if (Input.GetButtonDown("Jump") && !hasPerformedDJump)
        {
            Jump();
            hasPerformedDJump = true;
        }

        if(Input.GetButtonDown("Fire1") && ammoInstance.GetCanShoot())
        {
            shotSound.Play();
            Bullets bul = Instantiate( bulletPrefab, launchOffset.position, Quaternion.Euler(Vector3.zero) );
            ammoInstance.ShotFired();

            CalculateKnockback(bul);
        }

        UpdateEffects();
    }

    private void UpdateEffects()
    {
        PlayerState state;

        // Handle character facing the correct direction.
        if (dirX < 0f)
        {
            sr.flipX = true;
            gunSR.flipX = true;
            gunTransform.localPosition = gunTransformL;
        }
        else if (dirX > 0f)
        {
            sr.flipX = false;
            gunSR.flipX = false;
            gunTransform.localPosition = gunTransformR;
        }

        // Calculate gun rotation based on cursor position.
        CalculateGunRotation();

        // Handle switching between animations.
        if (dirX != 0f)
        {
            state = PlayerState.running;
            CreateDust();
        }
        else
        {
            state = PlayerState.idle;
            StopDust();
        }

        if (rb.velocity.y > 0.1f)
        {
            state = PlayerState.jumping;
            StopDust();
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = PlayerState.falling;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(bc.bounds.center, bc.bounds.size, 0f, Vector2.down, 0.2f, jumpableGround);
    }

    private void CreateDust()
    {
        dust.Play();
    }

    private void StopDust()
    {
        dust.Stop();
    }

    private void Jump()
    {
        jumpSound.Play();
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CalculateGunRotation()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (!sr.flipX)
        {
            float rotation_z = Mathf.Clamp(Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg, -90f, 90f);
            gunTransform.rotation = Quaternion.Euler(0f, 0f, rotation_z);
        }
        else
        {
            float rotation_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Clamp doesn't work for left side of the circle :[
            if (rotation_z < 90 && rotation_z >= 0) rotation_z = 90f;
            if (rotation_z < 0 && rotation_z > -90) rotation_z = -90f;
            gunTransform.rotation = Quaternion.Euler(0f, 0f, rotation_z + 180f);
        }
    }

    private void CalculateKnockback(Bullets bul)
    {
        Vector3 vec = bul.getBulletDirection();
        vec.y *= yKnockbackModifier;
        rb.AddForce(-vec * knockback, ForceMode2D.Impulse);
    }

    private void CheckForWinCondition()
    {
        if(transform.position.x >= 1490)
        {
            SceneManager.LoadScene("EndScreen");
        }
    }
}
