using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Members
    [SerializeField] private float moveTargetDistance;
    [SerializeField] private float moveDistance;
    private BoxCollider2D col;
    private Rigidbody2D rb;
    [SerializeField] private float thrust = 6f;
    [SerializeField] private AudioSource deathSound;
    [SerializeField] private AudioSource movementSound;
    private Ammo ammoInstance;
    #endregion

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        ammoInstance = GameObject.FindObjectOfType<Ammo>();

        StartCoroutine(JumpLogic());
        StartCoroutine(MoveLogic());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("bullet"))
        {
            DestroySelf();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("player"))
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        deathSound.Play();
        ammoInstance.RefillAmmo();
        rb.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        col.enabled = false;
        Destroy(gameObject, 2f);
    }

    IEnumerator JumpLogic()
    {
        float minWaitJump = 2f;
        float maxWaitJump = 5f;
        float minForce = 8f;
        float maxForce = 11f;

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitJump, maxWaitJump));
            movementSound.Play();
            float force = Random.Range(minForce, maxForce);
            rb.AddForce(transform.up * force, ForceMode2D.Impulse);
        }
    }

    IEnumerator MoveLogic()
    {
        float minWaitMove = 2f;
        float maxWaitMove = 4f;
        float minMoveDist = 5f;
        float maxMoveDist = 8f;
        

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minWaitMove, maxWaitMove));
            movementSound.Play();
            float force = Random.Range(minMoveDist, maxMoveDist);
            int directionModifier = Random.Range(0, 2);
            if(directionModifier == 0)
            {
                directionModifier = -1;
            }
            rb.AddForce(transform.right * directionModifier * force, ForceMode2D.Impulse);
        }
    }
}
