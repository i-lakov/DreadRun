using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private bool isCollected = false;
    private Animator animator;
    [SerializeField] private SpriteRenderer gun;
    private AudioSource collectSound;
    [SerializeField] private Ammo ammoInstance;

    void Start()
    {
        animator = GetComponent<Animator>();
        collectSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isCollected)
        {
            collectSound.Play();
            animator.SetBool("chestOpen", true);
            ammoInstance.RefillAmmo();
            gun.enabled = true;
            isCollected = true;
        }
    }
}
