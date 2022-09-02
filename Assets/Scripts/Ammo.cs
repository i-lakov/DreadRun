using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ammoA;
    [SerializeField] private SpriteRenderer ammoB;
    [SerializeField] private SpriteRenderer ammoC;
    private AudioSource refillSound;

    private int ammoAmount = 0;
    private void Start()
    {
        renderAmmo();
        refillSound = GetComponent<AudioSource>();
    }

    private void renderAmmo()
    {
        switch (ammoAmount)
        {
            case 3:
                {
                    ammoA.color = new Color(1f, 1f, 1f, 1f);
                    ammoB.color = new Color(1f, 1f, 1f, 1f);
                    ammoC.color = new Color(1f, 1f, 1f, 1f);
                    break;
                }
            case 2:
                {
                    ammoA.color = new Color(1f, 1f, 1f, 0.5f);
                    ammoB.color = new Color(1f, 1f, 1f, 1f);
                    ammoC.color = new Color(1f, 1f, 1f, 1f);
                    break;
                }
            case 1:
                {
                    ammoA.color = new Color(1f, 1f, 1f, 0.5f);
                    ammoB.color = new Color(1f, 1f, 1f, 0.5f);
                    ammoC.color = new Color(1f, 1f, 1f, 1f);
                    break;
                }
            case 0:
                {
                    ammoA.color = new Color(1f, 1f, 1f, 0.5f);
                    ammoB.color = new Color(1f, 1f, 1f, 0.5f);
                    ammoC.color = new Color(1f, 1f, 1f, 0.5f);
                    break;
                }
        }
    }

    public bool GetCanShoot()
    {
        return ammoAmount == 0 ? false : true;
    }

    public void ShotFired()
    {
        ammoAmount--;
        renderAmmo();
    }

    public void RefillAmmo()
    {
        refillSound.Play();
        ammoAmount = 3;
        renderAmmo();
    }
}
