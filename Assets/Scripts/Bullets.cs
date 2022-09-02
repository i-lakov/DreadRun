using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector3 direction;

    private void Awake()
    {
        direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    void FixedUpdate()
    {
        transform.position += direction.normalized * Time.fixedDeltaTime * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public Vector3 getBulletDirection()
    {
        return direction.normalized;
    }
}
